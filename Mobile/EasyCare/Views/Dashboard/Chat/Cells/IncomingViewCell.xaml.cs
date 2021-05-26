using System;
using System.Collections.Generic;
using EasyCare.Interface;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Chat.Cells
{
    public partial class IncomingViewCell : ViewCell
    {
        public IncomingViewCell()
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
                var ImagePath = imagePath.Text;
                var args = (TappedEventArgs)e;
                var path = args.Parameter.ToString();
                DependencyService.Get<CameraInterface>().openDocument(ImagePath);
            }
            catch (Exception ex)
            {

            }

        }


    }
}
