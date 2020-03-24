﻿using ESTQuestFilling.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace ESTQuestFilling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ApplicationViewModel();
        }
    }
}
