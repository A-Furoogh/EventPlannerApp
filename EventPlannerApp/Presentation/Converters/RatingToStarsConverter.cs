using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Presentation.Converters
{
    class RatingToStarsConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int rating)
            {
                rating = Math.Clamp(rating, 0, 5);

                return new String('★', rating) + new String('☆', 5 - rating);
            }
            return "☆☆☆☆☆";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
