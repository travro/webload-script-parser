﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using WLScriptParser.Models;

namespace WLScriptParser.DAL
{
    public static class SqlAPI
    {
        /**
         * Returns: Collection of Test names
         * Params:
         *      attribute: the Script attribute that will determine the column of values to be returned
         *          default: TestNames
         * */
        public static ObservableCollection<string> GetTestCollections(ScriptAttribute attribute)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();


            using (var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString))
            {
                cnn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;

                    switch (attribute)
                    {
                        case ScriptAttribute.TestNames: cmd.CommandText = "USE WLScriptsDB SELECT DISTINCT test_name FROM Tests"; ; break;
                        case ScriptAttribute.BuildNames: cmd.CommandText = "USE WLScriptsDB SELECT DISTINCT build_version FROM Tests"; ; break;
                        default: cmd.CommandText = "USE WLScriptsDB SELECT DISTINCT test_name FROM TESTS"; ; break;
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString(0));
                        }
                    }
                }
                cnn.Close();
            }
            return list;
        }
        /**
         * Populates script collections given as parameters
         * Params:
         *  scriptNames: the observable string collection of script names to be populated
         *  scriptDates: the observable string collection of recorded dates to be populated
         *  testName: the name parameter for retrieving a test id
         *  buildVersion: the build parameter for retrieving a test id
         * 
         * **/
        public static void GetScriptCollections(ObservableCollection<string> scriptNames, ObservableCollection<DateTime> scriptDates, string testName, string buildVersion)
        {
            using (var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString))
            {
                cnn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = "SELECT script_name, recording_date FROM Scripts WHERE test_id = " +
                        "(SELECT id FROM Tests WHERE test_name = @testName and build_version = @buildVersion)";

                    cmd.Parameters.AddRange(new SqlParameter[]
                    {
                            new SqlParameter(){ ParameterName = "@testName", SqlDbType = SqlDbType.NVarChar, Value = testName},
                            new SqlParameter(){ ParameterName = "@buildVersion", SqlDbType = SqlDbType.NVarChar, Value = buildVersion}
                    });



                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            scriptNames.Add(reader.GetString(0));
                            scriptDates.Add(reader.GetDateTime(1));
                        }
                    }
                }
                cnn.Close();
            }
        }

        /**
         * Returns: Id column value from a test record
         * Params:
         *  testName: the name parameter for the SQL query
         *  buildVersion: the build paraemter for the SQL query
         * **/
        public static int GetTestId(string testName, string buildVersion)
        {
            object id;

            using (var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString))
            {
                cnn.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = "SELECT id FROM Tests WHERE test_name = @testName and build_version = @buildVersion";
                    cmd.Parameters.AddRange(new SqlParameter[]
                    {
                            new SqlParameter(){ ParameterName = "@testName", SqlDbType = SqlDbType.NVarChar, Value = testName},
                            new SqlParameter(){ ParameterName = "@buildVersion", SqlDbType = SqlDbType.NVarChar, Value = buildVersion}
                    });
                    id = cmd.ExecuteScalar();
                }
                cnn.Close();
            }
            return (id != null) ? (Int32)id : -1;
        }

        public static int PushNewTest(string testName, string buildVersion)
        {
            object id;

            using (var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString))
            {
                cnn.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = "INSERT INTO Tests values (@testName, @buildVersion)" +
                        "SELECT CONVERT(int, SCOPE_IDENTITY())";
                    cmd.Parameters.AddRange(new SqlParameter[]
                    {
                            new SqlParameter(){ ParameterName = "@testName", SqlDbType = SqlDbType.NVarChar, Value = testName},
                            new SqlParameter(){ ParameterName = "@buildVersion", SqlDbType = SqlDbType.NVarChar, Value = buildVersion}
                    });

                    id = cmd.ExecuteScalar();
                }
                cnn.Close();
            }
            return (id != null) ? (int)id : -1;
        }

        public static void PushNewScript(Script script, string scriptName, DateTime dt, int testId)
        {
            using (var sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString))
            {
                sqlConnection.Open();

                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    using (var cmd = new SqlCommand("INSERT INTO Scripts values (@scriptName, @recordingDate, @testId) SELECT CONVERT(int, SCOPE_IDENTITY())", sqlConnection, sqlTransaction))
                    {
                        cmd.Parameters.AddRange(new SqlParameter[]
                        {
                            new SqlParameter(){ ParameterName = "@scriptName", SqlDbType = SqlDbType.NVarChar, Value = scriptName},
                            new SqlParameter(){ ParameterName = "@recordingDate", SqlDbType = SqlDbType.Date, Value = dt},
                            new SqlParameter(){ ParameterName = "@testId", SqlDbType = SqlDbType.Int, Value = testId }
                        });

                        try
                        {
                            //command execution here
                            object scriptScopeId = cmd.ExecuteScalar();
                            PushTransactions(script.Transactions, (Int32)scriptScopeId, sqlConnection, sqlTransaction);
                            sqlTransaction.Commit();
                        }
                        catch (Exception)
                        {
                            sqlTransaction.Rollback();
                            throw;
                        }
                    } 
                }
                sqlConnection.Close();
            }
        }
        #region helpermethods
        private static void PushTransactions(List<Transaction> transactions, int scriptId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {            
                using (var cmd = new SqlCommand("INSERT INTO TRANSACTIONS VALUES (@transName, @scriptId) SELECT CONVERT(int, SCOPE_IDENTITY())", sqlConnection, sqlTransaction))
                {
                    cmd.Parameters.AddRange(new SqlParameter[]
                    {
                        new SqlParameter(){ ParameterName = "@transName", SqlDbType = SqlDbType.NVarChar},
                        new SqlParameter(){ ParameterName = "@scriptId", SqlDbType = SqlDbType.Int}
                    });

                    try
                    {
                        foreach (var transaction in transactions)
                        {
                            cmd.Parameters["@transName"].Value = transaction.Name;
                            cmd.Parameters["@scriptId"].Value = scriptId;

                            //command execution here
                            object transactionScopeId = cmd.ExecuteScalar();

                            if (transactionScopeId != null)
                            {
                                PushRequests(transaction.Requests.Where(r => r.Visible == true), (Int32)transactionScopeId, sqlConnection, sqlTransaction);
                            }

                        }
                        
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
        }

        private static void PushRequests(IEnumerable<Request> requests, int transId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            using (var cmd = new SqlCommand("INSERT INTO Requests VALUES (@requestVerb, @requestParams, @transId) SELECT CONVERT(int, SCOPE_IDENTITY())", sqlConnection, sqlTransaction))
            {
                cmd.Parameters.AddRange(new SqlParameter[]
                {
                        new SqlParameter(){ ParameterName = "@requestVerb", SqlDbType = SqlDbType.Int, },
                        new SqlParameter(){ ParameterName = "@requestParams", SqlDbType = SqlDbType.NVarChar},
                        new SqlParameter(){ ParameterName = "@transId", SqlDbType = SqlDbType.Int, Value = transId},
                });

                try
                {
                    foreach (var request in requests)
                    {
                        cmd.Parameters["@requestVerb"].Value = (int)request.Verb;
                        cmd.Parameters["@requestParams"].Value = request.Parameters;

                        //command execution here
                        object requestScopeId = cmd.ExecuteScalar();


                        //if (request.Correlations != null && request.Correlations.Length > 0)
                        //{
                        //    PushCorrelations(request.Correlations, (Int32)id, cnn);
                        //}
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private static void PushCorrelations(IEnumerable<Correlation> correlations, int reqId, SqlConnection cnn)
        {

            /**
             
             TO REFRACTOR TO USING(COMMAND = NEW SQLCOMM) etc......
             */

            SqlCommand cmd;

            foreach (var correlation in correlations)
            {
                cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = "INSERT INTO Correlations VALUES (@rule, @extLogic, @originalVal, @reqId)" +
                    "SELECT CONVERT(int, SCOPE_IDENTITY())";
                cmd.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter(){ ParameterName = "@rule", SqlDbType = SqlDbType.NVarChar, Value = correlation.Rule },
                    new SqlParameter(){ ParameterName = "@extLogic", SqlDbType = SqlDbType.NVarChar, Value = correlation.ExtractionLogic },
                    new SqlParameter(){ ParameterName = "@originalVal", SqlDbType = SqlDbType.NVarChar, Value = correlation.OriginalValue },
                    new SqlParameter(){ ParameterName = "@reqId", SqlDbType = SqlDbType.Int, Value = reqId},
                });

                try
                {
                    object id = cmd.ExecuteScalar();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        #endregion
    }
}
