using System.Globalization;

namespace F_Framework.Converters
{
    public class DateToOpacityConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double resutlt = 0.5d;

            if (value != null)
            {
                var expDate = (DateTime)value;
                var currentDate = DateTime.Now;
                if (expDate < currentDate)
                {
                    resutlt = 0.5d;
                    return resutlt;
                }
                else
                {
                    resutlt = 1d;
                    return resutlt;
                }
            }
            return resutlt;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var expDate = (DateTime)value;
                var currentDate = DateTime.Now;
                if (expDate < currentDate)
                {
                    return .4;
                }
                else
                {
                    return 1;
                }
            }
            return .4;
        }
    }
}