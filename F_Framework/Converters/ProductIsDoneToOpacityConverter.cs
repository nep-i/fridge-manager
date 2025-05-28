using System.Globalization;

namespace F_Framework.Converters
{
    public class ProductIsDoneToOpacityConverter : IValueConverter
    {
        private bool isDone = false;

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double resutlt = 0.5d;
            isDone = (bool)value;
            if (isDone == false)
            {
                resutlt = 1d;
            }
            return resutlt;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double resutlt = 0.5d;
            isDone = (bool)value;
            if (isDone == false)
            {
                resutlt = 1d;
            }
            return resutlt;
        }
    }
}