using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MayProject
{
    class AnchorPointToMarginConverter : IValueConverter
    {
        public object Convert(object value,
                              System.Type targetType,
                              object parameter,
                              System.Globalization.CultureInfo culture) => new Thickness(((Point)value).X, ((Point)value).Y, 0, 0);

        public object ConvertBack(object value,
                                  System.Type targetType,
                                  object parameter,
                                  System.Globalization.CultureInfo culture) => null;
    }
}
