using System;
using Foundation;
using WPSTORE.iOS;
using WPSTORE.Services;
using WPSTORE.Services.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSAppVersion))]
namespace WPSTORE.iOS
{
    public class iOSAppVersion: IAppVersion
    {
        public iOSAppVersion()
        {
        }

        public string GetVersionNumber()
        {
            return NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString();
        }
    }
}
