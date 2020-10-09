using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView.Droid;
using ButtonCircle.FormsPlugin.Droid;
using CarouselView.FormsPlugin.Android;
using Android;
using Rg.Plugins.Popup.Services;
using Acr.UserDialogs;
using Xamarin.Auth;
using Plugin.CurrentActivity;
using LabelHtml.Forms.Plugin.Droid;
using Plugin.FirebasePushNotification;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.GoogleMaps.Android;
using TouchEffect.Android;
using Plugin.PayCards;
using Android.Content;

namespace WPSTORE.Droid
{
    [Activity(Label = "TeaTalk",
        Icon = "@drawable/icon",
        //Theme = "@style/Theme.Splash", // <-- Updated
        Theme = "@style/MainTheme",
        MainLauncher = false, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const int RequestLocationId = 0;

        readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //SetTheme(Resource.Style.MainTheme); // <-- Added
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            // Initializing FFImageLoading
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            Forms.SetFlags("FastRenderers_Experimental", "CollectionView_Experimental", "Expander_Experimental");
            HtmlLabelRenderer.Initialize();
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);

            TouchEffectPreserver.Preserve();
            // Initializing Popups
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            //CardsView
            PanCardView.Droid.CardsViewRenderer.Preserve();
            CarouselViewRenderer.Init();

            PayCardsRecognizerService.Initialize(this);

            ButtonCircleRenderer.Init();

            //ACR UserDialogs initializing
            UserDialogs.Init(this);

            Lottie.Forms.Droid.AnimationViewRenderer.Init();
            Stormlion.PhotoBrowser.Droid.Platform.Init(this);
            PancakeViewRenderer.Init();
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            var platformConfig = new PlatformConfig
            {
                BitmapDescriptorFactory = new CachingNativeBitmapDescriptorFactory()
            };
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState, platformConfig);

            CustomTabsConfiguration.CustomTabsClosingMessage = null;
           
            LoadApplication(new App());

            FirebasePushNotificationManager.ProcessIntent(this, Intent);
            Window.SetSoftInputMode(Android.Views.SoftInput.AdjustResize);
        }
        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        //{
        //    Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}

        protected override void OnStart()
        {
            base.OnStart();

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                }
                else
                {
                    Console.WriteLine("Location permissions already granted.");
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == RequestLocationId)
            {
                if ((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted))
                {
                    Console.WriteLine("Location permissions granted.");
                }
                else
                {
                    Console.WriteLine("Location permissions denied.");
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }

        //PayCard
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            PayCardsRecognizerService.OnActivityResult(requestCode, resultCode, data);
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                PopupNavigation.Instance.PopAsync();
            }
        }
    }
}