using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace VacationMasters.Resources.Converters
{
    public class BooleanToImageConverter : IValueConverter
    {
        public ImageSource TrueIcon { get; set; }
        public ImageSource FalseIcon { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                var state = (bool)value;
                if (state)
                {
                    return TrueIcon;
                }
            }
            return FalseIcon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
