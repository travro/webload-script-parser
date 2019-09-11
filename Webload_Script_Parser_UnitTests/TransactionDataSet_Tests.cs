using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WLScriptParser.Models;
using WLScriptParser.Parsers;
using WLScriptParser.Utilities;

namespace Webload_Script_Parser_UnitTests
{
    [TestClass]
    public class TransactionDataSet_Tests
    {
        //arrange
        string filepath1 = ConfigurationData.FilePath1;
        string s1FileName = ConfigurationData.FileName1;
        string filepath2 = ConfigurationData.FilePath2;
        string s2FileName = ConfigurationData.FileName2;
        string extension = ConfigurationData.Extension;
        Script s1, s2;
        TransactionDataSet tdS;

        //act
        public TransactionDataSet_Tests()
        {
            s1 = ScriptTransactionParser.Parse(filepath1 + s1FileName + extension);
            s2 = ScriptTransactionParser.Parse(filepath2 + s2FileName + extension);
            tdS = new TransactionDataSet(s1, s2);
        }
        //assertions
        [TestMethod]
        public void TDS_DataSet_IsNotNull()
        {
            Assert.IsNotNull(tdS.DataSet);
        }
        [TestMethod]
        public void TDS_DataSet_Tables_IsNotNul()
        {
            Assert.IsNotNull(tdS.DataSet.Tables);
        }
        [TestMethod]
        public void TDS_DataSet_Tables_CountGreaterThanZero()
        {
            Assert.IsTrue(tdS.DataSet.Tables.Count > 0);
        }
        //[TestMethod]
        //public void TDS_DataSet_FirstTable_IsFileNameTable()
        //{
        //    Assert.IsNotNull(tdS.DataSet.Tables[0]);
        //}
        //[TestMethod]
        //public void TDS_DataSet_FileNameTable_FirstColumnNameCorrect()
        //{
        //    Assert.AreEqual(tdS.DataSet.Tables[0].Columns[0].ColumnName, s1FileName + extension);
        //}
        //[TestMethod]
        //public void TDS_DataSet_FileNameTable_SecondColumnNameCorrect()
        //{
        //    Assert.AreEqual(tdS.DataSet.Tables[0].Columns[1].ColumnName, s2FileName + extension);
        //}
        [TestMethod]
        public void TDS_DataSet_TransactionTablesNumber_EqualsTransactionsLength()
        {
            Assert.AreEqual(tdS.DataSet.Tables.Count, s1.Transactions.Count);
        }
        [TestMethod]
        public void TDS_DataSet_FirstTransactionTableName_EqualsFirstTransactionsName()
        {
            Assert.AreEqual(tdS.DataSet.Tables[0].TableName, s1.Transactions[0].Name);
        }
        [TestMethod]
        public void TDS_DataSet_LastTransactionTableName_EqualsLastTransactionsName()
        {
            var tableCollection = tdS.DataSet.Tables;
            Assert.AreEqual(tableCollection[tableCollection.Count - 1].TableName, s1.Transactions[s1.Transactions.Count - 1].Name);
        }
        [TestMethod]
        public void TDS_DataSet_TableFirstColumns_EqualFirstFileName()
        {
            var tableCollection = tdS.DataSet.Tables;
            Assert.AreEqual(tableCollection[0].Columns[0].ColumnName, s1FileName + extension);
        }
        [TestMethod]
        public void TDS_DataSet_TableSecondColumns_EqualSecondFileName()
        {
            var tableCollection = tdS.DataSet.Tables;
            Assert.AreEqual(tableCollection[0].Columns[1].ColumnName, s2FileName + extension);
        }

    }
}
