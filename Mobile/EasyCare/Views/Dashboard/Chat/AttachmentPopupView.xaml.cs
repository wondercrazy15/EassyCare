using System;
using System.Collections.Generic;
using EasyCare.DataService;
using Syncfusion.XForms.Chat;
using Syncfusion.XForms.PopupLayout;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Chat
{
    public partial class AttachmentPopupView : CustomGrid
    {
        public AttachmentPopupView()
        {
            try
            {
                InitializeComponent();



            }
            catch (Exception ex)
            {

            }
        }

        private async void OpenGalleryTapped(object sender, EventArgs args)
        {
            await DependencyService.Get<IImageChooser>().StartImageChooser();
            
        }

        private async void AttachmentTapped(object sender, EventArgs args)
        {
            if (this.ViewModel.attachmentPopup.IsOpen)
            {
                this.ViewModel.attachmentPopup.Dismiss();
            }

            this.ViewModel.AttachmentChatCommand.Execute(sender);

        }
        private async void CalenderTapped(object sender, EventArgs args)
        {
            if (this.ViewModel.attachmentPopup.IsOpen)
            {
                this.ViewModel.attachmentPopup.Dismiss();
            }

            this.ViewModel.CalenderChatCommand.Execute(sender);

        }


        private async void ActivityTapped(object sender, EventArgs args)
        {
            //if (this.ViewModel.attachmentPopup.IsOpen)
            //{
            //    this.ViewModel.attachmentPopup.Dismiss();
            //}

            //this.ViewModel.ActivityCommand.Execute(sender);

        }


        private async void LocationTapped(object sender, EventArgs args)
        {
            //if (this.ViewModel.attachmentPopup.IsOpen)
            //{
            //    this.ViewModel.attachmentPopup.Dismiss();
            //}

            //this.ViewModel.LocationCommand.Execute(sender);

        }
        private async void MedicalTapped(object sender, EventArgs args)
        {
            //if (this.ViewModel.attachmentPopup.IsOpen)
            //{
            //    this.ViewModel.attachmentPopup.Dismiss();
            //}

            //this.ViewModel.ActivityCommand.Execute(sender);

        }
        private async void StepsTapped(object sender, EventArgs args)
        {
            //if (this.ViewModel.attachmentPopup.IsOpen)
            //{
            //    this.ViewModel.attachmentPopup.Dismiss();
            //}

            //this.ViewModel.StepsCommand.Execute(sender);

        }
        private async void BatteryTapped(object sender, EventArgs args)
        {
            //if (this.ViewModel.attachmentPopup.IsOpen)
            //{
            //    this.ViewModel.attachmentPopup.Dismiss();
            //}

            //this.ViewModel.BatteryCommand.Execute(sender);

        }

        private async void PulseTapped(object sender, EventArgs args)
        {
            //if (this.ViewModel.attachmentPopup.IsOpen)
            //{
            //    this.ViewModel.attachmentPopup.Dismiss();
            //}

            //this.ViewModel.PulseCommand.Execute(sender);

        }

    }
}