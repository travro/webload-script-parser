﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Webload_Script_Parser_WPF.Windows
{
    /// <summary>
    /// Interaction logic for ResolveDatabase.xaml
    /// </summary>
    public partial class ResolveDatabase : Window
    {
        string _dbName = "WLScriptsDB";
        public ResolveDatabase()
        {
            InitializeComponent();

            try
            {
                if (CheckDatabase(_dbName))
                {
                    Text_Block.Text = $"Database {_dbName} found";
                }
                else
                {
                    Text_Block.Text = $"Database {_dbName} not found";
                    Next_Button.IsEnabled = true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        //private void CreateDatabase(string databaseName)
        //{
        //    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["master"].ConnectionString))
        //    {
        //        cnn.Open();
        //        string create = "D";

        //        try
        //        {
        //            using (FileStream strm = File.OpenRead("./DAL/Scripts/create.sql"))
        //            {
        //                StreamReader reader = new StreamReader(strm);
        //                create = reader.ReadToEnd();
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }

                 
        //        string[] lines = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline).Split(create);


        //        using (SqlCommand cmd = cnn.CreateCommand())
        //        {
        //            foreach (string line in lines)
        //            {
        //                if (line.Length > 0)
        //                {
        //                    cmd.CommandText = line;
        //                    cmd.CommandType = CommandType.Text;

        //                    try
        //                    {
        //                        cmd.ExecuteNonQuery();
        //                    }
        //                    catch (Exception)
        //                    {
        //                        throw;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        private bool CheckDatabase(string databaseName)
        {
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["master"].ConnectionString))
            {
                cnn.Open();
                using (var cmd = new SqlCommand("select count(*) from master.dbo.sysdatabases where name=@database", cnn))
                {
                    cmd.Parameters.Add("@database", SqlDbType.NVarChar).Value = databaseName;
                    return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
                }
            }
        }
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
