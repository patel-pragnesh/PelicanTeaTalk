using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace WPSTORE.Helpers
{
    public static class SecureStorageHelpers
    {
        public static async Task SetValue(string key, string value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }
        public static async Task<string> GetValue(string key)
        {
            try
            {
                return await SecureStorage.GetAsync(key);
            }
            catch (Exception ex)
            {
                return null;
                // Possible that device doesn't support secure storage on device.
            }
        }
        public static bool RemoveKey(string key)
        {
            try
            {
                return SecureStorage.Remove(key);
            }
            catch (Exception ex)
            {
                return false;
                // Possible that device doesn't support secure storage on device.
            }
        }
    }
}
