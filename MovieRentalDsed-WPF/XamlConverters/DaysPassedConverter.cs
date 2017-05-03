using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MovieRentalDsed_WPF.XamlConverters
{
    class DaysPassedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var issuedDate = value is DateTime ? (DateTime) value : new DateTime();

            var daysPassed = DateTime.Now - issuedDate;

            return $"{daysPassed.Days} days ago";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
