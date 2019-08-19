﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Webload_Script_Parser_WPF.Models;
using System.Data;

namespace Webload_Script_Parser_WPF.Pages
{
    /// <summary>
    /// Interaction logic for CorrelationTablePage1.xaml
    /// </summary>
    public partial class CorrelationTablePage : Page
    {
        public CorrelationTablePage()
        {
            InitializeComponent();
        }
        public CorrelationTablePage(TransactionRepository repo)
        {
            InitializeComponent();

            var correlations = repo.Transactions.SelectMany(t => t.Requests).SelectMany(r => r.Correlations);

            DataTable corrTable = new DataTable("Correlations");
            corrTable.Columns.Add(new DataColumn("rule"));
            corrTable.Columns.Add(new DataColumn("ext_logic"));
            corrTable.Columns.Add(new DataColumn("original_val"));

            foreach(Correlation c in correlations)
            {
                var newCorrRow = corrTable.NewRow();
                newCorrRow["rule"] = c.Rule;
                newCorrRow["ext_logic"] = c.ExtractionLogic;
                newCorrRow["original_val"] = c.OriginalValue;
                corrTable.Rows.Add(newCorrRow);
            }

            Data_Grid_Correlations.ItemsSource = corrTable.DefaultView;
        }
    }
}