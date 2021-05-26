using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Acr.UserDialogs;
using EasyCare.Interface;
using EasyCare.Models.Monitoring;
using EasyCare.ViewModels.Dashboard.Monitoring;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Monitoring
{
    public partial class AddNewMedikamentePage : ContentPage
    {
        private AddNewMedikamenteViewModel vm;
        private DrugsModel drugsModel;

        public AddNewMedikamentePage()
        {
            try
            {
                vm = new AddNewMedikamenteViewModel(Navigation);
                InitializeComponent();
                BindingContext = vm;
                MessagingCenter.Subscribe<byte[]>(this, "ImageSelected", (sender) =>
                {
                    var datas = sender;
                    vm.isImage = true;
                    vm.isImageLable = false;
                    vm.Image = sender;
                    imageSelect.Source = ImageSource.FromStream(() => new MemoryStream(sender));
                });
            }
            catch (Exception ex)
            {

            }
            
        }

        public AddNewMedikamentePage(DrugsModel drugsModel)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                vm = new AddNewMedikamenteViewModel();
                InitializeComponent();
                BindingContext = vm;
                this.drugsModel = drugsModel;
                imageSelect.Source = drugsModel.ActualImage;
                byte[] imageByte = (new WebClient()).DownloadData(drugsModel.ActualImage);
                vm.Image = imageByte;
                vm.EditDataInfo(this.drugsModel, Navigation);

                MessagingCenter.Subscribe<byte[]>(this, "ImageSelected", (sender) =>
                {
                    var datas = sender;
                    vm.isImage = true;
                    vm.isImageLable = false;
                    vm.Image = sender;
                    imageSelect.Source = ImageSource.FromStream(() => new MemoryStream(sender));
                });
                
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {

            }
        }

       

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        async void BildRight_Clicked(System.Object sender, System.EventArgs e)
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
