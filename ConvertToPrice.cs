using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Supermarket.Model;

namespace Supermarket
{
    [ValueConversion(typeof(string), typeof(string))]
    public class ConvertToPrice : IValueConverter
    {
        Model.Model model = new Model.Model();
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (float.Parse(value.ToString()) * model.RetrieveProduct((int)parameter).Price).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "no";
        }
    }
}
