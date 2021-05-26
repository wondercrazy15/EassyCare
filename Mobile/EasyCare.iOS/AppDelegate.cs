using System;
using System.Diagnostics;
using Foundation;
using UIKit;
using UserNotifications;
using Syncfusion.XForms.iOS.Cards;
using Syncfusion.XForms.iOS.BadgeView;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.XForms.iOS.Border;
using Syncfusion.XForms.iOS.Buttons;
using System.Linq;
using WindowsAzure.Messaging;
using EasyCare.Core.Constants;
using EasyCare.DI;
using EasyCare.Services;
using Syncfusion.SfCalendar.XForms.iOS;
using Syncfusion.XForms.iOS.TabView;
using Syncfusion.XForms.iOS.TextInputLayout;
using Microsoft.Identity.Client;
using Syncfusion.SfNumericUpDown.XForms.iOS;
using Twilio.Chat.iOS;
using System.Runtime.InteropServices;

namespace EasyCare.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private SBNotificationHub Hub { get; set; }
        
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            FormsInit();
            SyncfusionInit();
            LoadApplication(new App());
           
            base.FinishedLaunching(app, options);
            RegisterForRemoteNotifications();

            return true;
        }

        void FormsInit()
        {
            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            Rg.Plugins.Popup.Popup.Init();
        }
        
        void SyncfusionInit()
        {
            Syncfusion.SfDataGrid.XForms.iOS.SfDataGridRenderer.Init();
            Syncfusion.XForms.iOS.PopupLayout.SfPopupLayoutRenderer.Init();
            SfCardViewRenderer.Init();
            SfBadgeViewRenderer.Init();
            SfCalendarRenderer.Init();
            SfListViewRenderer.Init();
            SfTextInputLayoutRenderer.Init();
            SfTabViewRenderer.Init();
            SfBorderRenderer.Init();
            SfButtonRenderer.Init();
            SfCheckBoxRenderer.Init();
            SfNumericUpDownRenderer.Init();
            Syncfusion.XForms.iOS.Chat.SfChatRenderer.Init();
        }

        void RegisterForRemoteNotifications()
        {
            // register for remote notifications based on system version
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert |
                    UNAuthorizationOptions.Sound |
                    UNAuthorizationOptions.Sound,
                    (granted, error) =>
                    {
                        if (granted)
                            InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                    });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                 UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
        }

 
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
         
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                byte[] result = new byte[deviceToken.Length];
                Marshal.Copy(deviceToken.Bytes, result, 0, (int)deviceToken.Length);
                GlobalConstant.deviceID = BitConverter.ToString(result).Replace("-", "");

                //deviceRegistration.Handle = GlobalConstants.DeviceToken.Replace("-", string.Empty);
            }
            else
            {
                GlobalConstant.deviceID = deviceToken.ToString().TrimStart('<').TrimEnd('>').Replace(" ", string.Empty);
            }


            // GlobalConstants.DeviceToken = deviceToken.ToString();
            //MessagingManager messagingManager = new MessagingManager();
            //messagingManager.SetDeviceToken(deviceToken);

            //Hub = new SBNotificationHub(
            //    NotificationConstants.FullListenSharedAccessString, NotificationConstants.NotificationHubName);

            //// update registration with Azure Notification Hub
            //Hub.UnregisterAll(deviceToken, (error) =>
            //{
            //    if (error != null)
            //    {
            //        Debug.WriteLine($"Unable to call unregister {error}");
            //        return;
            //    }

            //   string[] SubscriptionTags = { GlobalConstant.deviceID };

            //    var tags = new NSSet(SubscriptionTags.ToArray());
            //    Hub.RegisterNative(deviceToken, tags, (errorCallback) =>
            //    {
            //        if (errorCallback != null)
            //        {
            //            Debug.WriteLine($"RegisterNativeAsync error: {errorCallback}");
            //        }
            //    });

            //    var templateExpiration = DateTime.Now.AddDays(120).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            //    Hub.RegisterTemplate(deviceToken, "defaultTemplate", NotificationConstants.APNTemplateBody, templateExpiration, tags, (errorCallback) =>
            //    {
            //        if (errorCallback != null)
            //        {
            //            if (errorCallback != null)
            //            {
            //                Debug.WriteLine($"RegisterTemplateAsync error: {errorCallback}");
            //            }
            //        }
            //    });
            //       });

       }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
            return true;
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }

        void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            if (options != null && options.ContainsKey(new NSString("aps")))
            {
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;
                string payload = string.Empty;
                NSString payloadKey = new NSString("alert");
                if (aps.ContainsKey(payloadKey))
                {
                    payload = aps[payloadKey].ToString();
                }

                if (!string.IsNullOrWhiteSpace(payload))
                {
                    AppContainer.Resolve<INotificationService>().Add(payload);
                }
            }
            else
            {
                Debug.WriteLine($"Received request to process notification but there was no payload.");
            }
        }
    }
}
