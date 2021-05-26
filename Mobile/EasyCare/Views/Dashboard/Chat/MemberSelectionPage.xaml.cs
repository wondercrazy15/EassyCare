using System;
using System.Collections.Generic;
using System.IO;
using EasyCare.Interface;
using EasyCare.Models.Monitoring;
using EasyCare.ViewModels.Dashboard.Chat;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Chat
{
    public partial class MemberSelectionPage : ContentPage
    {
        private MemberSelectionPageViewModel vm;

        public MemberSelectionPage()
        {
            try
            {
                vm = new MemberSelectionPageViewModel(Navigation);
                InitializeComponent();
                BindingContext = vm;
                MessagingCenter.Subscribe<byte[]>(this, "ImageSelected", (sender) =>
                {
                    vm.GroupImage = sender;
                    imageSelect.Source = ImageSource.FromStream(() => new MemoryStream(sender));
                });
            }
            catch (Exception ex)
            {

            }

        }


        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                var action = await DisplayActionSheet("Foto hinzufügen", "Abbrechen", null, "Aus der Galerie wählen", "Foto aufnehmen");

                if (action == "Aus der Galerie wählen")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var fileName = SetImageFileName();
                        DependencyService.Get<CameraInterface>().LaunchGallery(FileFormatEnum.JPEG, null);
                    });
                }
                else if (action == "Foto aufnehmen")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var fileName = SetImageFileName();
                        DependencyService.Get<CameraInterface>().LaunchCamera(FileFormatEnum.JPEG, fileName);
                    });
                }
            }
            catch (Exception ex)
            {

            }
        }

        private string SetImageFileName(string fileName = null)
        {
            try
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    if (fileName != null)
                        App.ImageIdToSave = fileName;
                    else
                        App.ImageIdToSave = App.DefaultImageId;

                    return App.ImageIdToSave;
                }
                else
                {
                    //To iterate, on iOS, if you want to save images to the devie, set 
                    if (fileName != null)
                    {
                        App.ImageIdToSave = fileName;
                        return fileName;
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
