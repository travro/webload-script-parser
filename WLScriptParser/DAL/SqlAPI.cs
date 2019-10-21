using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using WLScriptParser.Models;
using WLScriptParser.Utilities;

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
        public static int GetTestId(string testName, string buildVersion)
        {
            object id;

            using (var cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;
                    cmd.CommandText = "SELECT id FROM Tests WHERE test_name = @testName and build_version = @buildVersion";
                    cmd.Parameters.AddRange(new SqlParameter[]
                    {
                            new SqlParameter(){ ParameterName = "@testName", SqlDbType = SqlDbType.NVarChar, Value = testName},
                            new SqlParameter(){ ParameterName = "@buildVersion", SqlDbType = SqlDbType.NVarChar, Value = buildVersion}
                    });
                    cnn.Open();
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
                try
                {
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = cnn;
                        cmd.CommandText = "INSERT INTO Tests values (@testName, @buildVersion)";
                        cmd.Parameters.AddRange(new SqlParameter[]
                        {
                            new SqlParameter(){ ParameterName = "@testName", SqlDbType = SqlDbType.NVarChar, Value = testName},
                            new SqlParameter(){ ParameterName = "@buildVersion", SqlDbType = SqlDbType.NVarChar, Value = buildVersion}
                        });
                        AppLogger.Logger.LogMessage($"Pushing {testName} {buildVersion} to Test table in WLScriptDB");

                        cnn.Open();
                        id = cmd.ExecuteScalar();

                        AppLogger.Logger.LogMessage($"Successfully pushed {testName} {buildVersion} to Test table in WLScriptDB");
                    }
                }
                finally
                {
                    cnn.Close();
                }
            }
            return (id != null) ? (int)id : -1;
        }

        public static void PushNewScript(Script script, string scriptName, DateTime dt, int testId)
        {
            using (var sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WLScriptsDB"].ConnectionString))
            {
                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    using (var cmd = new SqlCommand("INSERT INTO Scripts OUTPUT INSERTED.ID VALUES (@scriptName, @recordingDate, @testId) ", sqlConnection, sqlTransaction))
                    {
                        cmd.Parameters.AddRange(new SqlParameter[]
                        {
                            new SqlParameter(){ ParameterName = "@scriptName", SqlDbType = SqlDbType.NVarChar, Value = scriptName},
                            new SqlParameter(){ ParameterName = "@recordingDate", SqlDbType = SqlDbType.Date, Value = dt},
                            new SqlParameter(){ ParameterName = "@testId", SqlDbType = SqlDbType.Int, Value = testId }
                        });

                        try
                        {
                            AppLogger.Logger.LogMessage($"Pushing script {scriptName} that was recorded on {dt} to Scripts table of WLScriptDB");

                            sqlConnection.Open();
                            //command execution here
                            object scriptScopeId = cmd.ExecuteScalar();
                            PushTransactions(script.Transactions, (Int32)scriptScopeId, sqlConnection, sqlTransaction);
                            sqlTransaction.Commit();

                            AppLogger.Logger.LogMessage($"Successfully pushed script {scriptName} that was recorded on {dt} to Scripts table of WLScriptDB");
                        }
                        catch (Exception)
                        {
                            AppLogger.Logger.LogMessage("Error pushing script to DB");
                            sqlTransaction.Rollback();
                            throw;
                        }
                        finally
                        {
                            sqlConnection.Close();
                        }
                    }
                }

            }
        }

        #region helpermethods
        private static void PushTransactions(List<Transaction> transactions, int scriptId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            using (var cmd = new SqlCommand("", sqlConnection, sqlTransaction))
            {
                cmd.Parameters.AddRange(new SqlParameter[]
                {
                        new SqlParameter(){ ParameterName = "@transName", SqlDbType = SqlDbType.NVarChar},
                        new SqlParameter(){ ParameterName = "@transNameId", SqlDbType = SqlDbType.Int, Value = 0},
                        new SqlParameter(){ ParameterName = "@scriptId", SqlDbType = SqlDbType.Int, Value = scriptId}
                });

                try
                {
                    AppLogger.Logger.LogMessage($"Pushing transactions to DB on script_id {scriptId}");

                    foreach (var transaction in transactions)
                    {
                        cmd.Parameters["@transName"].Value = transaction.Name;
                        cmd.CommandText = "SELECT id FROM TransactionNames WHERE trans_name = @transName";                        

                        object transactionScopeId = cmd.ExecuteScalar();

                        if (transactionScopeId == null)
                        {
                            cmd.CommandText = "INSERT INTO TransactionNames (trans_name) OUTPUT INSERTED.ID VALUES (@transName)";
                            transactionScopeId = cmd.ExecuteScalar();
                        }

                        cmd.Parameters["@transNameId"].Value = (Int32)transactionScopeId;
                        cmd.CommandText = "INSERT INTO Transactions (trans_nm_id, script_id) OUTPUT INSERTED.ID VALUES (@transNameId, @scriptId)";

                        transactionScopeId = cmd.ExecuteScalar();

                        if (transactionScopeId != null)
                        {
                            PushRequests(transaction.Requests.Where(r => r.Visible == true), (Int32)transactionScopeId, sqlConnection, sqlTransaction);
                        }
                        else
                        {
                            throw new Exception("Error obtaining transaction id from Transaction table insertion");
                        }
                    }
                    AppLogger.Logger.LogMessage("Successfully pushed all transactions to Transaction table in WLScriptDB");
                }
                catch (Exception)
                {
                    AppLogger.Logger.LogMessage("Error pushing transactions to database");
                    throw;
                }
            }
        }

        private static void PushRequests(IEnumerable<Request> requests, int transId, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
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
                    AppLogger.Logger.LogMessage($"Pushing requests to DB on trans_id {transId}");
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
                    AppLogger.Logger.LogMessage($"Successfully pushed all requests to Request Table on trans_id {transId} in WLScriptDB");
                }
                catch (Exception)
                {
                    AppLogger.Logger.LogMessage("Error pushing requests to database");
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
