using System;
using ButtonCircle.FormsPlugin.iOS;
using CarouselView.FormsPlugin.iOS;
using FFImageLoading.Forms.Platform;
using FFImageLoading.Svg.Forms;
using Foundation;
using LabelHtml.Forms.Plugin.iOS;
using Lottie.Forms.iOS.Renderers;
using PanCardView.iOS;
using Plugin.FirebasePushNotification;
using Syncfusion.SfRotator.XForms.iOS;
using TouchEffect.iOS;
using UIKit;
using Xamarin.Forms.GoogleMaps.iOS;

namespace WPSTORE.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override UIWindow Window { get; set; }
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            if (Window == null)
            {
                Window = new UIWindow(frame: UIScreen.MainScreen.Bounds);
                var initialViewController = new SplashViewController();
                Window.RootViewController = initialViewController;
                Window.MakeKeyAndVisible();

                return true;
            }
            else
            {
                global::Xamarin.Forms.Forms.SetFlags("Shell_Experimental", "Visual_Experimental", "CollectionView_Experimental", "FastRenderers_Experimental", "Expander_Experimental");
                HtmlLabelRenderer.Initialize();

                new Syncfusion.XForms.iOS.ComboBox.SfComboBoxRenderer();

                global::Xamarin.Forms.Forms.Init();
                global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
                //Xamarin.FormsMaps.Init();

                ZXing.Net.Mobile.Forms.iOS.Platform.Init();
                var ignore = typeof(SvgCachedImage);

                CachedImageRenderer.Init(); // Initializing FFImageLoading

                Stormlion.PhotoBrowser.iOS.Platform.Init();
                CarouselViewRenderer.Init(); // Initializing CarouselView
                new SfRotatorRenderer();
                Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
                Syncfusion.XForms.iOS.BadgeView.SfBadgeViewRenderer.Init();
                AnimationViewRenderer.Init();

                var platformConfig = new PlatformConfig
                {
                    ImageFactory = new CachingImageFactory()
                };
                Xamarin.FormsGoogleMaps.Init(GlobalSettings.GoogleMapsApiKey, platformConfig);

                //ButtonCircleRenderer.Init();

                Rg.Plugins.Popup.Popup.Init();

                CardsViewRenderer.Preserve();
                TouchEffectPreserver.Preserve();
                LoadApplication(new App());

                Plugin.Segmented.Control.iOS.SegmentedControlRenderer.Initialize();

              FirebasePushNotificationManager.Initialize(options, true);

                return base.FinishedLaunching(app, options);
            }
            
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);

        }
        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired 'till the user taps on the notification launching the application.

            // If you disable method swizzling, you'll need to call this method. 
            // This lets FCM track message delivery and analytics, which is performed
            // automatically with method swizzling enabled.
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            // Do your magic to handle the notification data
            System.Console.WriteLine(userInfo);

            completionHandler(UIBackgroundFetchResult.NewData);
        }

        public class CustomLogger : FFImageLoading.Helpers.IMiniLogger
        {
            public void Debug(string message)
            {
                Console.WriteLine(message);
            }

            public void Error(string errorMessage)
            {
                Console.WriteLine(errorMessage);
            }

            public void Error(string errorMessage, Exception ex)
            {
                Error(errorMessage + System.Environment.NewLine + ex.ToString());
            }
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            // Convert iOS NSUrl to C#/netxf/BCL System.Uri
            var uri_netfx = new Uri(url.AbsoluteString);
            App.Authenticator?.OnPageLoading(uri_netfx);

            return true;
        }
    }
}
