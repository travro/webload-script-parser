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
using System.Windows.Shapes;
using Webload_Script_Parser_WPF.Models;
using Webload_Script_Parser_WPF.Controls;
using System.ComponentModel;

namespace Webload_Script_Parser_WPF.Windows
{
    /// <summary>
    /// Interaction logic for ScriptItemListWindow.xaml
    /// </summary>
    public partial class ScriptItemListWindow : Window, INotifyPropertyChanged
    {
        public string[] SelectableList { get; }
        public ScriptItemListWindow()
        {
            InitializeComponent();
        }
        public ScriptItemListWindow(ScriptItemControl.ScriptAttribute attribute)
        {
            InitializeComponent();
            DataContext = this;

            switch (attribute)
            {
                case ScriptItemControl.ScriptAttribute.TestNames: SelectableList = AttributesRepository.Repository.TestNames; break;
                case ScriptItemControl.ScriptAttribute.BuildNames: SelectableList = AttributesRepository.Repository.TestBuilds; break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void List_View_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string selection = (sender as ListView).SelectedValue.ToString();
            NotifyPropertyChanged(selection);
            Close();
        }

        public void NotifyPropertyChanged(string selection)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(selection));
            }
        }
    }
}