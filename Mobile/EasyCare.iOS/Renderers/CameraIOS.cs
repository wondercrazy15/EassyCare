using System;
using System.IO;
using AVFoundation;
using CoreGraphics;
using EasyCare.Interface;
using EasyCare.iOS.Renderers;
using EasyCare.Models.Monitoring;
using Foundation;
using MobileCoreServices;
using QuickLook;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(CameraIOS))]
namespace EasyCare.iOS.Renderers
{
    public class CameraIOS : CameraInterface
    {
        private string[] allowedUTIs =  {
                   
                    //UTType.UTF8PlainText,
                    UTType.PlainText,
                    UTType.RTF,
                    UTType.PNG,
                    UTType.Text,
                    UTType.Image,
                    UTType.PDF,

                };

        public async void LaunchCamera(FileFormatEnum imageType, string imageId = null, ISelectedImagepath selectedImagepath = null)
        {
            //Check if we have permission to use the camera
            var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
            if (authorizationStatus != AVAuthorizationStatus.Authorized)
            {
                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }

                var okAlertController = UIAlertController.Create("Give Permission", "For camera you have to give manually access", UIAlertControllerStyle.Alert);

                //Add Action
                okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, alert => UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString))));

                vc.PresentViewController(okAlertController, true, null);
            }
            //If we don't have access, and have never asked before, prompt them
            if (authorizationStatus != AVAuthorizationStatus.Authorized)
            {
                var access = await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
                //UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
                //If access was granted we can proceed, if not, you can add an else statement and implement an error message or something more helpful
                if (access)
                {
                    if (imageId == null)
                        GotAccessToCamera(imageType, null, selectedImagepath);
                    else
                    {
                        var fileName = imageId + "." + imageType.ToString();
                        GotAccessToCamera(imageType, fileName, selectedImagepath);
                    }
                }
            }
            else
            {
                //We've already been given access
                if (imageId == null)
                    GotAccessToCamera(imageType, null, selectedImagepath);
                else
                {
                    var fileName = imageId + "." + imageType.ToString();
                    GotAccessToCamera(imageType, fileName, selectedImagepath);
                }
            }
        }

        public void LaunchGallery(FileFormatEnum imageType, string imageId = null, ISelectedImagepath selectedImagepath = null)
        {
            try
            {
                var imagePicker = new UIImagePickerController { SourceType = UIImagePickerControllerSourceType.PhotoLibrary, MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary) };
                imagePicker.AllowsEditing = true;

                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }

                vc.PresentViewController(imagePicker, true, null);

                imagePicker.FinishedPickingMedia += (sender, e) =>
                {
                    var type = e.MediaType;
                    UIImage originalImage = e.Info[UIImagePickerController.EditedImage] as UIImage;
                    if (originalImage != null)
                    {
                        //NSData pngImage = null;

                        //if (imageType == FileFormatEnum.JPEG)
                        //    pngImage = originalImage.AsJPEG();
                        //else
                        //    pngImage = originalImage.AsPNG();

                        //byte[] myByteArray = new byte[pngImage.Length];
                        //System.Runtime.InteropServices.Marshal.Copy(pngImage.Bytes, myByteArray, 0, Convert.ToInt32(pngImage.Length));
                        //if (imageId != null)
                        //{

                        //    var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        //    string filePath = System.IO.Path.Combine(documentsDirectory, imageId);
                        //    //NSError err = null;
                        //    //if (imgData.Save(filePath, false, out err))
                        //    //{
                        //    //    Console.WriteLine("Saved image to " + filePath);
                        //    //}
                        //    //else
                        //    //{
                        //    //    //Handle the Error!
                        //    //    Console.WriteLine("Could NOT save to " + filePath + " because" + err.LocalizedDescription);
                        //    //}
                        //    MessagingCenter.Send<string>(filePath, "ImageSelectedPath");
                        //}



                        UIGraphics.BeginImageContext(originalImage.Size);

                        if (originalImage.Orientation == UIImageOrientation.Right)
                        {
                            CGAffineTransform.MakeRotation((nfloat)(90 * Math.PI / 180));
                        }
                        else if (originalImage.Orientation == UIImageOrientation.Left)
                        {
                            CGAffineTransform.MakeRotation((nfloat)(90 * Math.PI / 180));
                        }
                        else if (originalImage.Orientation == UIImageOrientation.Down)
                        {
                            // NOTHING
                        }
                        else if (originalImage.Orientation == UIImageOrientation.Up)
                        {
                            CGAffineTransform.MakeRotation((nfloat)(90 * Math.PI / 180));
                        }

                        originalImage.Draw(new CGPoint(0, 0));
                        UIImage rotateImage = UIGraphics.GetImageFromCurrentImageContext();
                        UIGraphics.EndImageContext();

                        rotateImage = rotateImage.Scale(new CGSize(rotateImage.Size.Width, rotateImage.Size.Height), 0.5f);

                        NSData imgData = null;

                        if (imageType == FileFormatEnum.PNG)
                            imgData = rotateImage.AsPNG();
                        else
                            imgData = rotateImage.AsJPEG();

                        byte[] myByteArray = new byte[imgData.Length];
                        System.Runtime.InteropServices.Marshal.Copy(imgData.Bytes, myByteArray, 0, Convert.ToInt32(imgData.Length));

                        if (imageId != null)
                        {

                            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            var directoryname = System.IO.Path.Combine(documents, "EasyCare");
                            if (!System.IO.Directory.Exists(directoryname))
                            {
                                System.IO.Directory.CreateDirectory(directoryname);
                            }
                            string filePath = System.IO.Path.Combine(directoryname, imageId);

                            if (imageType == FileFormatEnum.PNG)
                                imgData = rotateImage.AsPNG();
                            else
                                imgData = rotateImage.AsJPEG();

                            NSError err = null;
                            if (imgData.Save(filePath, false, out err))
                            {
                                Console.WriteLine("Saved image to " + filePath);
                            }
                            else
                            {
                                //Handle the Error!
                                Console.WriteLine("Could NOT save to " + filePath + " because" + err.LocalizedDescription);
                            }
                            selectedImagepath.SelectedImagepath(filePath);
                        }


                        MessagingCenter.Send<byte[]>(myByteArray, "ImageSelected");
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        vc.DismissViewController(true, null);
                    });
                };

                imagePicker.Canceled += (sender, e) => vc.DismissViewController(true, null);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
        NSData imgData;
        private void GotAccessToCamera(FileFormatEnum imageType, string imageId = null, ISelectedImagepath selectedImagepath = null)
        {
            var imagePicker = new UIImagePickerController { SourceType = UIImagePickerControllerSourceType.Camera };
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;

            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            vc.PresentViewController(imagePicker, true, null);

            imagePicker.FinishedPickingMedia += (sender, e) =>
            {
                UIImage image = (UIImage)e.Info.ObjectForKey(new NSString("UIImagePickerControllerOriginalImage"));
                //UIImage rotateImage = RotateImage(image, image.Orientation);

                UIGraphics.BeginImageContext(image.Size);

                if (image.Orientation == UIImageOrientation.Right)
                {
                    CGAffineTransform.MakeRotation((nfloat)(90 * Math.PI / 180));
                }
                else if (image.Orientation == UIImageOrientation.Left)
                {
                    CGAffineTransform.MakeRotation((nfloat)(90 * Math.PI / 180));
                }
                else if (image.Orientation == UIImageOrientation.Down)
                {
                    // NOTHING
                }
                else if (image.Orientation == UIImageOrientation.Up)
                {
                    CGAffineTransform.MakeRotation((nfloat)(90 * Math.PI / 180));
                }

                image.Draw(new CGPoint(0, 0));
                UIImage rotateImage = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();

                rotateImage = rotateImage.Scale(new CGSize(rotateImage.Size.Width, rotateImage.Size.Height), 0.5f);

                NSData imgData = null;

                if (imageType == FileFormatEnum.PNG)
                    imgData = rotateImage.AsPNG();
                else
                    imgData = rotateImage.AsJPEG();

                byte[] myByteArray = new byte[imgData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imgData.Bytes, myByteArray, 0, Convert.ToInt32(imgData.Length));

                if (imageId != null)
                {

                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var directoryname = System.IO.Path.Combine(documents, "EasyCare");
                    if (!System.IO.Directory.Exists(directoryname))
                    {
                        System.IO.Directory.CreateDirectory(directoryname);
                    }
                    string filePath = System.IO.Path.Combine(directoryname, imageId);

                    if (imageType == FileFormatEnum.PNG)
                        imgData = rotateImage.AsPNG();
                    else
                        imgData = rotateImage.AsJPEG();

                    NSError err = null;
                    if (imgData.Save(filePath, false, out err))
                    {
                        Console.WriteLine("Saved image to " + filePath);
                    }
                    else
                    {
                        //Handle the Error!
                        Console.WriteLine("Could NOT save to " + filePath + " because" + err.LocalizedDescription);
                    }
                    selectedImagepath.SelectedImagepath(filePath);
                }
                MessagingCenter.Send<byte[]>(myByteArray, "ImageSelected");


                Device.BeginInvokeOnMainThread(() =>
                {
                    vc.DismissViewController(true, null);
                });
            };

            imagePicker.Canceled += (sender, e) => vc.DismissViewController(true, null);
        }

        public double radians(double degrees) { return degrees * Math.PI / 180; }

        private UIImage RotateImage(UIImage src, UIImageOrientation orientation)
        {
            UIGraphics.BeginImageContext(src.Size);

            if (orientation == UIImageOrientation.Right)
            {
                CGAffineTransform.MakeRotation((nfloat)radians(90));
            }
            else if (orientation == UIImageOrientation.Left)
            {
                CGAffineTransform.MakeRotation((nfloat)radians(-90));
            }
            else if (orientation == UIImageOrientation.Down)
            {
                // NOTHING
            }
            else if (orientation == UIImageOrientation.Up)
            {
                CGAffineTransform.MakeRotation((nfloat)radians(90));
            }

            src.Draw(new CGPoint(0, 0));
            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return image;
        }

        private void Picker_WasCancelled(object sender, EventArgs e)
        {

        }


        public void openDocument(string path)
        {
            try
            {
                //Link=https://forums.xamarin.com/discussion/52724/handling-a-file-open-with-or-share-options
                var fileinfo = new FileInfo(path);
                var previewController = new QLPreviewController();
                previewController.DataSource = new PreviewControllerDataSource(fileinfo.FullName, fileinfo.Name);

                UINavigationController controller = FindNavigationController();

                if (controller != null)
                {
                    controller.PresentViewController((UIViewController)previewController, true, (Action)null);
                }
                //var PreviewController = UIDocumentInteractionController.FromUrl(path);
                //var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;

                //while (vc.PresentedViewController != null)
                //{
                //    vc = vc.PresentedViewController;
                //}

                //PreviewController.Delegate = new UIDocumentInteractionControllerDelegateClass(vc);
                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    PreviewController.PresentPreview(true);
                //});

            }
            catch (Exception ex)
            {

            }
        }

        public void LaunchDocument(ISelectedImagepath selectedImagepath = null)
        {
            try
            {
                var picker = new UIDocumentPickerViewController(allowedUTIs, UIDocumentPickerMode.Open);
                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;

                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }

                vc.PresentViewController(picker, true, null);

                picker.WasCancelled += Picker_WasCancelled;
                picker.DidPickDocumentAtUrls += (object s, UIDocumentPickedAtUrlsEventArgs e) =>
                {
                    Console.WriteLine("url = {0}", e.Urls[0].AbsoluteString);
                    //bool success = await MoveFileToApp(didPickDocArgs.Url);  
                    var success = true;
                    string filename = e.Urls[0].LastPathComponent;

                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    var directoryname = System.IO.Path.Combine(documents, "EasyCare");
                    //string contentType = MimeTypes.GetMimeType(filename);
                    string msg = success ? string.Format("Successfully imported file '{0}'", filename) : string.Format("Failed to import file '{0}'", filename);
                    // Some invaild file url returns null  
                    NSData data = NSData.FromUrl(e.Urls[0]);
                    if (data != null)
                    {
                        byte[] dataBytes = new byte[data.Length];

                        //var temp = (NSString)directoryname;
                        //temp = temp.AppendPathComponent((NSString)filename);
                        //NSOutputStream nSOutputStream = NSOutputStream.CreateFile(temp, true);

                        System.Runtime.InteropServices.Marshal.Copy(data.Bytes, dataBytes, 0, Convert.ToInt32(data.Length));

                        for (int i = 0; i < dataBytes.Length; i++)
                        {
                            Console.WriteLine(dataBytes[i]);
                        }

                        string filePath = System.IO.Path.Combine(directoryname, filename);


                        NSError err = null;
                        if (data.Save(filePath, false, out err))
                        {
                            Console.WriteLine("Saved image to " + filePath);
                        }
                        else
                        {
                            //Handle the Error!
                            Console.WriteLine("Could NOT save to " + filePath + " because" + err.LocalizedDescription);
                        }
                        selectedImagepath.SelectedImagepath(filePath);
                    }

                    //var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    //var directoryname = System.IO.Path.Combine(documents, "EasyCare");
                    //if (!System.IO.Directory.Exists(directoryname))
                    //{
                    //    System.IO.Directory.CreateDirectory(directoryname);
                    //}
                    //string filePath = System.IO.Path.Combine(directoryname, filename);
                    //NSError err = null;
                    //if (data.Save(filePath, false, out err))
                    //{
                    //    Console.WriteLine("Saved image to " + filePath);
                    //}
                    //else
                    //{
                    //    //Handle the Error!
                    //    Console.WriteLine("Could NOT save to " + filePath + " because" + err.LocalizedDescription);
                    //}

                    Console.WriteLine(data + "Completed");
                    var p = e.Urls[0].AbsoluteString;
                    //var myByteArray = System.IO.File.ReadAllBytes(e.Urls[0].ToString());
                    //selectedImagepath.SelectedImagepath(e.Urls[0].AbsoluteString);
                    //var alertController = UIAlertController.Create("import", msg, UIAlertControllerStyle.Alert);
                    //var okButton = UIAlertAction.Create("OK", UIAlertActionStyle.Default, (obj) =>
                    //{
                    //    alertController.DismissViewController(true, null);
                    //});
                    //alertController.AddAction(okButton);
                    //vc.PresentViewController(alertController, true, null);
                    // PresentViewController(alertController, true, null);
                };
                //PresentViewController(picker, true, null);
            }
            catch (Exception ex)
            {

            }
        }


        private UINavigationController FindNavigationController()
        {
            foreach (var window in UIApplication.SharedApplication.Windows)
            {
                if (window.RootViewController.NavigationController != null)
                {
                    return window.RootViewController.NavigationController;
                }
                else
                {
                    UINavigationController value = CheckSubs(window.RootViewController.ChildViewControllers);
                    if (value != null)
                    {
                        return value;
                    }
                }
            }
            return null;
        }

        private UINavigationController CheckSubs(UIViewController[] controllers)
        {
            foreach (var controller in controllers)
            {
                if (controller.NavigationController != null)
                {
                    return controller.NavigationController;
                }
                else
                {
                    UINavigationController value = CheckSubs(controller.ChildViewControllers);
                    if (value != null)
                    {
                        return value;
                    }
                }

                return null;
            }
            return null;
        }
    }

    public class DocumentItem : QLPreviewItem
    {
        private string _title;
        private string _uri;

        public DocumentItem(string title, string uri)
        {
            _title = title;
            _uri = uri;
        }

        public override string ItemTitle
        { get { return _title; } }

        public override NSUrl ItemUrl
        { get { return NSUrl.FromFilename(_uri); } }
    }

    public class PreviewControllerDataSource : QLPreviewControllerDataSource
    {
        private string _url;
        private string _filename;

        public PreviewControllerDataSource(string url, string filename)
        {
            _url = url;
            _filename = filename;
        }

        public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
        {
            return (IQLPreviewItem)new DocumentItem(_filename, _url);
        }

        public override nint PreviewItemCount(QLPreviewController controller)
        { return (nint)1; }
    }

}
