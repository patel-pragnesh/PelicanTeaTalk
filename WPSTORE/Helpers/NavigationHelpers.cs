using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WPSTORE.Helpers
{
    public static class NavigationHelpers
    {
        public static async Task GotoPageAsync(string pageName, Dictionary<string, object> uriParams = null, bool flyoutIsPresented = false)
        {
            string paramUrl = "";
            if (uriParams != null)
            {
                var paramLists = uriParams.Select(x => $"{x.Key}={x.Value}");
                paramUrl = String.Join("&", paramLists);
                paramUrl = $"?{paramUrl}";
            }
            await Shell.Current.GoToAsync($"{pageName}{paramUrl}");
            Shell.Current.FlyoutIsPresented = flyoutIsPresented;
        }
    }
}
