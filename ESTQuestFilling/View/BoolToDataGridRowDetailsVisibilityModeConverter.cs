using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ESTQuestFilling.View
{
    class BoolToDataGridRowDetailsVisibilityModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value
                ? System.Windows.Controls.DataGridRowDetailsVisibilityMode.Visible
                : DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
