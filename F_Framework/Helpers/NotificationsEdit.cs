using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;

namespace F_Framework.Helpers
{
    public static class NotificationsEdit
    {
        public static async Task<Result<Product>> CreateNotification(Product product)
        {
            var result = new Result<Product>();
            try
            {
                var request = new NotificationRequest()
                {
                    NotificationId = product.Id,
                    Title = $"Date of {product.Name} has expired!",
                    BadgeNumber = product.Id,
                    Android = new AndroidOptions()
                    {
                        Color = new AndroidColor(156)
                    },
                    Schedule = new NotificationRequestSchedule()
                    {
                        NotifyTime = DateTime.Now.AddMinutes(product.DaysForLife),
                        RepeatType = NotificationRepeat.No
                    }
                };

                _ = await LocalNotificationCenter.Current.Show(request);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                result.element = product;
                return result;
            }

            result.Succeeded = true;
            return result;
        }

        public static async Task<Result<Product>> CancelNotification(Product product)
        {
            var result = new Result<Product>();
            _ = await Task.Run(() => LocalNotificationCenter.Current.Clear(product.Id));
            result.Succeeded = true;
            return result;
        }
    }
}