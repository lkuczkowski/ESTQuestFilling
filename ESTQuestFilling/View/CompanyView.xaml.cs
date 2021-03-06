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

namespace ESTQuestFilling.View
{
    /// <summary>
    /// Interaction logic for CompanyView.xaml
    /// </summary>
    public partial class CompanyView : UserControl
    {
        public CompanyView()
        {
            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent, Source = e.Source
            };

            ScrollViewer scv = (ScrollViewer)sender;
            scv.RaiseEvent(eventArg);
            e.Handled = true;
        }
    }
}
