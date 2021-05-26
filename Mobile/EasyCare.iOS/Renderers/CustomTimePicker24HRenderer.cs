using System;
using EasyCare.Controls;
using EasyCare.iOS.Renderers;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomTimePicker24H), typeof(CustomTimePicker24HRenderer))]
namespace EasyCare.iOS.Renderers
{
    public class CustomTimePicker24HRenderer : TimePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var timePicker = (UIDatePicker)Control.InputView;
                timePicker.Locale = new NSLocale("no_nb");
                timePicker.MinimumDate = new DateTime(1, 1, 1).Add((e.NewElement as CustomTimePicker24H).MinimumTime).ToNSDate();


                if (Element != null && !Element.Time.Equals(default(TimeSpan)))
                    Control.Text = Element.Time.ToString(@"hh\:mm");
                else
                    Control.Text = "00:00";
            }
        }
    }
}