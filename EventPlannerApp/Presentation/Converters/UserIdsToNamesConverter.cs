using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerApp.Presentation.Converters
{
    public class UserIdsToNamesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Enumerable.Empty<string>();
            }

            if (value is IList<int> userIds)
            {
                try
                {
                    var filteredUSerids = userIds.Where(uid => uid != MainPage.UserId).ToList();
                    var userNames = GetNamesAsync(filteredUSerids);
                    if (userNames.Result.Count == 1)
                    {
                        return userNames.Result.First();
                    }
                    return userNames.Result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return Enumerable.Empty<string>();
                }
            }
            else if (value is int userId)
            {
                try
                {
                    if (userId == MainPage.UserId)
                        return "Sie";

                    var userName = MainPage.GetUserNameAsync(userId);
                    return userName.Result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return string.Empty;
                }
            }

            return Enumerable.Empty<string>();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private async Task<IList<string>> GetNamesAsync(IList<int> userIds)
        {
            var userNames = new List<string>();

            foreach (var userId in userIds)
            {
                var userName = await MainPage.GetUserNameAsync(userId);
                if (!string.IsNullOrEmpty(userName))
                {
                    userNames.Add(userName);
                }
            }

            return userNames;
        }
    }
}
