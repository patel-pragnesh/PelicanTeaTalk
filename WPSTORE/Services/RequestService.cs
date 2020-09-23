using MonkeyCache.FileStore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Prism.Navigation;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WPSTORE.Exceptions;
using WPSTORE.Extensions;
using WPSTORE.Helpers;
using WPSTORE.Languages.Texts;
using WPSTORE.Services.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WPSTORE.Services
{
    public class RequestService : IRequestService
    {
        private readonly JsonSerializerSettings serializerSettings;
        private readonly JsonSerializerSettings requestSerializerSettings;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public RequestService(INavigationService navigationService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            requestSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new LowercaseContractResolver(),
                Formatting = Formatting.Indented
            };
            serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<TResult> GetAsync<TResult>(string uri, bool forcedRefresh = false)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return Barrel.Current.Get<TResult>(key: uri);
            }
            try
            {
                if (!Barrel.Current.IsExpired(key: uri) && !forcedRefresh)
                {
                    return Barrel.Current.Get<TResult>(key: uri);
                }
                var httpClient = await HttpClientCreator(uri);
                var response = await httpClient.GetAsync(uri);
                await HandleResponse(response);
                var serialized = await response.Content.ReadAsStringAsync();
                var result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, serializerSettings));

                //Saves the cache and pass it a timespan for expiration
                //Barrel.Current.Add(key: uri, data: result, expireIn: TimeSpan.FromMinutes(5));
                Barrel.Current.Add(key: uri, data: result, expireIn: TimeSpan.FromSeconds(1));
                return result;
            }
            catch (Exception ex)
            {
                return default(TResult);
            }

        }
        public async Task<string> PostAsync<TRequest>(string uri, TRequest data)
        {
            var httpClient = await HttpClientCreator(uri);
            var serialized = await Task.Run(() => JsonConvert.SerializeObject(data, requestSerializerSettings));

            try
            {
                var content = new StringContent(serialized, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, content);

                await HandleResponse(response);
                var responseData = await response.Content.ReadAsStringAsync();

                var responseWrapper = await Task.Run(() => JsonConvert.DeserializeObject(responseData, serializerSettings));
                if (responseWrapper != null)
                    return responseData;
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data)
        {
            var httpClient = await HttpClientCreator(uri);
            var serialized = await Task.Run(() => JsonConvert.SerializeObject(data, requestSerializerSettings));

            try
            {
                var content = new StringContent(serialized, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, content);

                await HandleResponse(response);
                var responseData = await response.Content.ReadAsStringAsync();
                return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, serializerSettings));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<TResult> AuthPostAsync<TRequest, TResult>(string uri, string request)
        {
            var httpClient = await HttpClientCreator(uri);
            var response = await httpClient.PostAsync(uri, new StringContent(request, Encoding.UTF8, "application/x-www-form-urlencoded"));
            await HandleResponse(response);
            var responseData = await response.Content.ReadAsStringAsync();
            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, serializerSettings));
        }

        public Task<TResult> PutAsync<TResult>(string uri, TResult data) => PutAsync<TResult, TResult>(uri, data);

        public async Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data)
        {
            var httpClient = await HttpClientCreator(uri);
            var serialized = await Task.Run(() => JsonConvert.SerializeObject(data, serializerSettings));
            var response = await httpClient.PutAsync(uri, new StringContent(serialized, Encoding.UTF8, "application/json"));

            await HandleResponse(response);

            var responseData = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, serializerSettings));
        }

        async Task<HttpClient> CreateHttpClient()
        {
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    throw new ConnectivityException();
                }
                //switch (Device.RuntimePlatform)
                //{
                //    case Device.Android:
                //        this.httpClient = new HttpClient(DependencyService.Get<Services.IHTTPClientHandlerCreationService>().GetInsecureHandler());
                //        break;
                //    default:
                //        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //        var httpClient = new HttpClient(new HttpClientHandler());
                //        break;
                //}
                //var httpClient = new HttpClient();
                var httpClient = new HttpClient(DependencyService.Get<IHTTPClientHandlerCreationService>().GetInsecureHandler());
                httpClient.Timeout = TimeSpan.FromSeconds(45);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true
                };
                var token = await SecureStorageHelpers.GetValue(GlobalSettings.SecureKeys.Token);

                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                return httpClient;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private HttpClient CreateWooCommerceHttpClient()
        {
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    throw new ConnectivityException();
                }

                //var httpClient = new HttpClient();
                var httpClient = new HttpClient(DependencyService.Get<IHTTPClientHandlerCreationService>().GetInsecureHandler());
                httpClient.Timeout = TimeSpan.FromSeconds(45);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true
                };
                return httpClient;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<HttpClient> HttpClientCreator(string url)
        {
            if (url.Contains(GlobalSettings.WooCommerncePrefix))
                return CreateWooCommerceHttpClient();

            return await CreateHttpClient();
        }

        async Task HandleResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception(content);
                }

                throw new HttpRequestException(content);
            }
            else
            {
                //convert response to base response object
                var responseObj = await Task.Run(() => JsonConvert.DeserializeObject(content, serializerSettings));
                if (responseObj != null )
                    await Device.InvokeOnMainThreadAsync(async () =>
                    {
                        //await _dialogService.ShowAlertAsync(TextsTranslateManager.Translate("SessionExpired"), TextsTranslateManager.Translate("SystemWarning"), "OK");
                        //await _navigationService.NavigateAsync(nameof(LoginPage));
                    });
            }
        }
    }
}
