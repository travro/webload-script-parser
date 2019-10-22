using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                try
                {
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = cnn;

                        switch (attribute)
                        {
                            case ScriptAttribute.TestNames: cmd.CommandText = "USE WLScriptsDB SELECT DISTINCT test_name FROM Tests"; ; break;
                            case ScriptAttribute.BuildNames: cmd.CommandText = "USE WLScriptsDB SELECT DISTINCT build_version FROM Tests"; ; break;
                            default: cmd.CommandText = "USE WLScriptsDB SELECT DISTINCT test_name FROM TESTS"; ; break;
                        }
                        cnn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(reader.GetString(0));
                            }
                        }
                    }
                }
                finally
                {
                    cnn.Close();
                }

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
                try
                {
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

                        cnn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                scriptNames.Add(reader.GetString(0));
                                scriptDates.Add(reader.GetDateTime(1));
                            }
                        }
                    }
                }
                finally
                {
                    cnn.Close();
                }
            }
        }

        /**
         * Returns: Id column value from a test record
         * Params:
         *  testName: the name parameter for the SQL query
         *  buildVersion: the build paraemter for the SQL query
         * **/
        public static int GetTestId(string testName, string buildVersion, SqlConnection cnn)
        {
            object id;
            using (var cmd = new SqlCommand("SELECT id FROM Tests WHERE test_name = @testName and build_version = @buildVersion", cnn))
            {
                cmd.Parameters.AddRange(new SqlParameter[]
                {
                            new SqlParameter(){ ParameterName = "@testName", SqlDbType = SqlDbType.NVarChar, Value = testName},
                            new SqlParameter(){ ParameterName = "@buildVersion", SqlDbType = SqlDbType.NVarChar, Value = buildVersion}
                });
                id = cmd.ExecuteScalar();
            }
            return (id != null) ? (Int32)id : -1;
        }

        public static int PushNewTest(string testName, string buildVersion, SqlConnection cnn)
        {
            object id;
            using (var cmd = new SqlCommand("INSERT INTO Tests values (@testName, @buildVersion)", cnn))
            {
                cmd.Parameters.AddRange(new SqlParameter[]
                {
                            new SqlParameter(){ ParameterName = "@testName", SqlDbType = SqlDbType.NVarChar, Value = testName},
                            new SqlParameter(){ ParameterName = "@buildVersion", SqlDbType = SqlDbType.NVarChar, Value = buildVersion}
                });
                try
                {
                    id = cmd.ExecuteScalar();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return (id != null) ? (int)id : -1;
        }

        public static int PushNewScript(Script script, string scriptName, DateTime dt, int testId, SqlConnection cnn, SqlTransaction sqlTrn)
        {
            object scriptId;
            using (var cmd = new SqlCommand("INSERT INTO Scripts OUTPUT INSERTED.ID VALUES (@scriptName, @recordingDate, @testId) ", cnn, sqlTrn))
            {
                cmd.Parameters.AddRange(new SqlParameter[]
                {
                            new SqlParameter(){ ParameterName = "@scriptName", SqlDbType = SqlDbType.NVarChar, Value = scriptName},
                            new SqlParameter(){ ParameterName = "@recordingDate", SqlDbType = SqlDbType.Date, Value = dt},
                            new SqlParameter(){ ParameterName = "@testId", SqlDbType = SqlDbType.Int, Value = testId }
                });

                try
                {
                    scriptId = cmd.ExecuteScalar();
                    PushNewTransactions(script.Transactions, (Int32)scriptId, cnn, sqlTrn);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return (scriptId != null) ? (int)scriptId : -1;
        }

        #region helpermethods
        private static int GetTestId(string transactionName, SqlConnection cnn, SqlTransaction sqlTrn)
        {
            object transNameScopeId;
            using (var cmd = new SqlCommand("SELECT id FROM TransactionNames WHERE trans_name = @transName", cnn, sqlTrn))
            {
                cmd.Parameters.AddWithValue("@transName", transactionName);

                transNameScopeId = cmd.ExecuteScalar();

                if (transNameScopeId == null)
                {
                    cmd.CommandText = "INSERT INTO TransactionNames (trans_name) OUTPUT INSERTED.ID VALUES (@transName)";
                    transNameScopeId = cmd.ExecuteScalar();
                }
            }
            return (Int32)transNameScopeId;
        }
        private static void PushNewTransactions(List<Transaction> transactions, int scriptId, SqlConnection cnn, SqlTransaction sqlTrn)
        {
            using (var cmd = new SqlCommand("", cnn, sqlTrn))
            {
                cmd.Parameters.AddRange(new SqlParameter[]
                {
                        new SqlParameter(){ ParameterName = "@transNameId", SqlDbType = SqlDbType.Int},
                        new SqlParameter(){ ParameterName = "@scriptId", SqlDbType = SqlDbType.Int, Value = scriptId}
                });

                try
                {
                    foreach (var transaction in transactions)
                    {
                        cmd.Parameters["@transNameId"].Value = GetTestId(transaction.Name, cnn, sqlTrn);
                        cmd.CommandText = "INSERT INTO Transactions (trans_nm_id, script_id) OUTPUT INSERTED.ID VALUES (@transNameId, @scriptId)";

                        object transactionScopeId = cmd.ExecuteScalar();

                        if (transactionScopeId != null)
                        {
                            PushNewRequests(transaction.Requests.Where(r => r.Visible == true), (Int32)transactionScopeId, cnn, sqlTrn);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private static void PushNewRequests(IEnumerable<Request> requests, int transId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            using (var cmd = new SqlCommand("INSERT INTO Requests OUTPUT INSERTED.ID VALUES (@requestVerbId, @requestParams, @transId)", sqlConnection, sqlTransaction))
            {
                cmd.Parameters.AddRange(new SqlParameter[]
                {
                        new SqlParameter(){ ParameterName = "@requestVerbId", SqlDbType = SqlDbType.Int, },
                        new SqlParameter(){ ParameterName = "@requestParams", SqlDbType = SqlDbType.NVarChar},
                        new SqlParameter(){ ParameterName = "@transId", SqlDbType = SqlDbType.Int, Value = transId},
                });

                try
                {
                    foreach (var request in requests)
                    {
                        cmd.Parameters["@requestVerbId"].Value = (Int32)request.Verb;
                        cmd.Parameters["@requestParams"].Value = request.Parameters;

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
                cmd.CommandText = "INSERT INTO Correlations VALUES (@rule, @extLogic, @originalVal, @reqId)";
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
