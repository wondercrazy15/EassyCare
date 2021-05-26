using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyCare.Interface;
using EasyCare.Models.Monitoring;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Chat
{
    public partial class AttachmentChatView : CustomGrid, ISelectedImagepath
    {
        private ITwilioChatHelper twilioChatHelper;

        public AttachmentChatView()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {

            }
        }

        private Task DisplayActionSheet(string v1, string v2, object p, string v3, string v4)
        {
            throw new NotImplementedException();
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

        void GallaryTapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Random random = new Random();
                    int num = random.Next(1000000);
                    // EasyCare
                    var fileName = "EasyCareChatImage" + num + ".png";
                    DependencyService.Get<CameraInterface>().LaunchGallery(FileFormatEnum.JPEG, fileName, this);
                });
                this.ViewModel.attachmentPopup.Dismiss();
          
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var fileName = SetImageFileName();
                    DependencyService.Get<CameraInterface>().LaunchCamera(FileFormatEnum.JPEG, fileName);
                });
            }
        }
        void CameraTapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Random random = new Random();
                    int num = random.Next(1000000);
                    // EasyCare
                    var fileName = "EasyCareChatCameraImage" + num;
                    DependencyService.Get<CameraInterface>().LaunchCamera(FileFormatEnum.JPEG, fileName, this);
                });
                this.ViewModel.attachmentPopup.Dismiss();

            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var fileName = SetImageFileName();
                    DependencyService.Get<CameraInterface>().LaunchCamera(FileFormatEnum.JPEG, fileName);
                });
            }
        }

        public void SelectedImagepath(string imagePath)
        {
            try
            {
                if (imagePath != null)
                {
                    twilioChatHelper = DependencyService.Get<ITwilioChatHelper>();
                    twilioChatHelper.SendMediaMessage(imagePath, true, Application.Current.Properties["supervisor_id"].ToString(),this.ViewModel.GroupKey);
                }
            }
            catch (Exception ex)
            {

            }
        }

        void DocumentTapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DependencyService.Get<CameraInterface>().LaunchDocument(this);
                });
                this.ViewModel.attachmentPopup.Dismiss();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
