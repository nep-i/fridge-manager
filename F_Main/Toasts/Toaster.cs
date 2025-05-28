//using Android.Widget;
using CommunityToolkit.Maui.Alerts;

namespace F_Main.Toasts
{
    public class Toaster : IToast
    {
        public void MakeToast(string message)
        {
            Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Short, 17).Show();
            //MakeText(Platform.AppContext, message, ToastLength.Long).Show();
        }
    }
}