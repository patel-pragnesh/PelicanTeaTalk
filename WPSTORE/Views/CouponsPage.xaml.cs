using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPSTORE.Languages.Texts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CouponsPage : ContentPage
    {
        public CouponsPage()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        public async void ScanBarcode(object sender, EventArgs e)
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            if (cameraStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                if (results.ContainsKey(Permission.Camera))
                    cameraStatus = results[Permission.Camera];
            }
            if (cameraStatus == PermissionStatus.Granted)
                ScanCustomOverlayAsync();
        }

        public async void ScanCustomOverlayAsync()
        {
            var options = new MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                DisableAutofocus = false,
            };

            options.PossibleFormats.Add(BarcodeFormat.QR_CODE);
            options.PossibleFormats.Add(BarcodeFormat.CODE_128);
            options.PossibleFormats.Add(BarcodeFormat.CODE_93);
            options.PossibleFormats.Add(BarcodeFormat.CODE_39);
            options.PossibleFormats.Add(BarcodeFormat.EAN_13);
            options.PossibleFormats.Add(BarcodeFormat.EAN_8);

            var overlay = new ZXingDefaultOverlay
            {
                ShowFlashButton = true,
                BottomText = TextsTranslateManager.Translate("ScanTopText"),
                TopText = TextsTranslateManager.Translate("TurnOnOffFlash")
            };

            overlay.BindingContext = overlay;

            var scanPage = new ZXingScannerPage(options, overlay)
            {
                Title = TextsTranslateManager.Translate("ScannerTitle"),
            };

            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    await DisplayAlert("Scanned result", result.Text, "OK");
                });
            };
            await Navigation.PushAsync(scanPage);

            overlay.FlashButtonClicked += (s, ed) =>
            {
                scanPage.ToggleTorch();
            };
        }
    }
}