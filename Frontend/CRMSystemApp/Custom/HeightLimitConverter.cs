using System.Globalization;
using System.Windows.Data;

namespace CRMSystemApp.Custom
{
    public class HeightLimitConverter : IValueConverter
    {
        private const double ScreenThreshold = 1000;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double gridHeight)
            {
                if (gridHeight > ScreenThreshold)
                    return double.PositiveInfinity;
                else
                {
                    double maxHeight = 250;
                    
                    return Math.Max(200, maxHeight);
                }
            }

            return 250;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}