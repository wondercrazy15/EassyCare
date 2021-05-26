using System;
using System.Collections.Generic;
using EasyCare.Interface;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Chat.Cells
{
    public partial class OutgoingViewCell : ViewCell
    {
        public OutgoingViewCell()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {

            }

        }
        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                var args = (TappedEventArgs)e;
                var path = args.Parameter.ToString();
                DependencyService.Get<CameraInterface>().openDocument(path);
            }
            catch (Exception ex)
            {

            }

        }
    }
}
