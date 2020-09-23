using Newtonsoft.Json;
using WPSTORE.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WPSTORE.Services
{
    public class GoogleMapsApiService
    {
        private const string ApiBaseAddress = "https://maps.googleapis.com/maps/";
        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiBaseAddress)
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }
        public async Task<GoogleMapsDistanceResult> CalculateDistance(MapPoint myPoint, IEnumerable<StoreInfo> stores)
        {
            GoogleMapsDistanceResult results = null;
            using (var httpClient = CreateClient())
            {
                string[] destPoints = stores.Select(x => $"{x.Latitude.Value},{x.Longitude.Value}").ToArray();
                string destination = string.Join("|", destPoints);
                var response = await httpClient.GetAsync($"api/distancematrix/json?origins={myPoint.Latitude},{myPoint.Longitude}&destinations={destination}&key={GlobalSettings.GoogleMapsApiKey}").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrWhiteSpace(json) && json != "ERROR")
                    {
                        results = await Task.Run(() =>
                           JsonConvert.DeserializeObject<GoogleMapsDistanceResult>(json)
                        ).ConfigureAwait(false);
                    }
                }
            }
            return results;
        }
    }
}
