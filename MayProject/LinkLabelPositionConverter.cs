using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MayProject
{
    class LinkLabelPositionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, 
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            double maxX = Math.Max(((Point)values[0]).X, ((Point)values[1]).X);
            double minX = Math.Min(((Point)values[0]).X, ((Point)values[1]).X);
            double maxY = Math.Max(((Point)values[0]).Y, ((Point)values[1]).Y);
            double minY = Math.Min(((Point)values[0]).Y, ((Point)values[1]).Y);

            return new Point { X = minX + (maxX - minX) / 2, Y = minY + (maxY - minY) / 2 };
        }

        public object[] ConvertBack(object value,
                                    Type[] targetTypes,
                                    object parameter,
                                    CultureInfo culture) => null;
    }
}
