using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Plugin.Permissions;
using Acr.UserDialogs;
using WPSTORE.Models;
using Plugin.Geolocator.Abstractions;
using Plugin.Geolocator;
using System.Diagnostics;

namespace WPSTORE.Helpers
{
    public static class AppHelpers
    {
        private const int R = 6378137;
        public static int? GetCategoryId(this string slugName)
        {
            var category = GlobalSettings.WpCategories?.FirstOrDefault(x => x.slug == slugName);
            return category?.id;
        }
        public static string GetCategoryName(this int? categoryId)
        {
            var category = GlobalSettings.WpCategories?.FirstOrDefault(x => x.id == categoryId);
            return category?.name;
        }
        public static async Task<bool> RequestPermission(Permission permission)
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);

            if (permissionStatus == PermissionStatus.Granted) return true;

            if (permissionStatus == PermissionStatus.Denied)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(permission))
                    {
                        await UserDialogs.Instance.AlertAsync("We neeed permission", "We neeed permission", "OK");
                    }
                    try
                    {
                        var result = await CrossPermissions.Current.RequestPermissionsAsync(new[] { permission });
                        permissionStatus = result[permission];
                    }
                    catch(Exception ex)
                    {

                    }
                    
                }
                else // On IOS
                {
                    var isGoingToSettings = await UserDialogs.Instance.ConfirmAsync("Go to Settings !", "Permission request", "OK", "Cancel");
                    if (isGoingToSettings)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }
                }
            }
            // On IOS Only
            else if (permissionStatus == PermissionStatus.Unknown)
            {
                var result = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                permissionStatus = result[permission];
            }

            return permissionStatus == PermissionStatus.Granted;
        }

        private static double Rad(double x)
        {
            return x * Math.PI / 180;
        }
        public static double Distance(MapPoint point1, MapPoint point2)
        {
            var dLat = Rad(point2.Latitude - point1.Latitude);
            var dLong = Rad(point2.Longitude - point1.Longitude);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(Rad(point1.Latitude)) * Math.Cos(Rad(point2.Latitude)) *
                    Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;
            return d; // returns the distance in meter
        }

        public static async Task<Position> GetCurrentPosition()
        {
            Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //got a cahched position, so let's use it.
                    return position;
                }
                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    //not available or enabled
                    return null;
                }
                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location: " + ex);
            }

            if (position == null)
                return null;

            var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            Debug.WriteLine(output);

            return position;
        }
    }
}
