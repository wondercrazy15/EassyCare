using System;
using EasyCare.Interface;
using EasyCare.iOS.Service;
using Foundation;
using GlobalToast;

[assembly: Xamarin.Forms.Dependency(typeof(Toast_iOS))]
namespace EasyCare.iOS.Service
{
    public class Toast_iOS : IToast
    {
        public void Show(string message)
        {
            var toast = Toast.MakeToast(message).Show();

            //ShowAlert(message, LONG_DELAY);
            NSTimer.CreateScheduledTimer(2, false, delegate
            {
                toast.Dismiss();
                //Toast.ShowToast(message).Dismiss();
            });
        }
    }
}
