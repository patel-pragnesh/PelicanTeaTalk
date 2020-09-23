using WPSTORE.Exceptions;
using WPSTORE.Helpers;
using WPSTORE.Services;
using WPSTORE.Services.Interfaces;
using WPSTORE.Views;
using Prism.Services;
using Prism.Unity;
using Rg.Plugins.Popup.Services;
using System;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using WPSTORE.Languages.Texts;

namespace WPSTORE.Threading
{
    public static class WebRequestExecuter
    {
        public static async Task Execute<TResult>(
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<System.Exception, Task> failCallback = null,
            Action finallyCallback = null, bool showConnectionError = true)
        {
            if (successCallback == null)
            {
                successCallback = _ => Task.CompletedTask;
            }

            if (failCallback == null)
            {
                failCallback = _ => Task.CompletedTask;
            }

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet && showConnectionError)
                {
                    await PopupNavigation.Instance.PushAsync(new ConnectionErrorPage());


                    //await Device.InvokeOnMainThreadAsync(async () =>
                    //{
                    //    var container = ((WPSTORE.App)Application.Current).Container;
                    //    var dialogService = (IDialogService)container.Resolve(typeof(IDialogService));
                    //    dialogService.ShowToast(TextsTranslateManager.Translate("ConnectionErrorToast"));
                    //});
                    var accepted = false; //await UserDialogs.Instance.ConfirmAsync("No Internet", "No internet connection", "Ok", "Cancel");
                    if (accepted)
                    {
                        await Execute(func, successCallback, failCallback);
                    }
                    else
                    {
                        await successCallback(await func());
                        //await failCallback(new System.Exception("NoInternet"));
                    }
                }
                else
                {
                    await successCallback(await func());
                }
            }
            catch (System.Exception exception)
            {
                await HandleException(exception, func, successCallback, failCallback);
            }
            finally
            {
                finallyCallback?.Invoke();
            }
        }

        public static async Task Execute(
            Func<Task> func,
            Func<Task> successCallback = null,
            Func<System.Exception, Task> failCallback = null,
            Action finallyCallback = null)
        {
            if (successCallback == null)
            {
                successCallback = () => Task.CompletedTask;
            }

            if (failCallback == null)
            {
                failCallback = _ => Task.CompletedTask;
            }

            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await PopupNavigation.Instance.PushAsync(new ConnectionErrorPage());
                    //await Device.InvokeOnMainThreadAsync(async () =>
                    //{
                    //    var container = ((WPSTORE.App)Application.Current).Container;
                    //    var dialogService = (IDialogService)container.Resolve(typeof(IDialogService));
                    //    dialogService.ShowToast(TextsTranslateManager.Translate("ConnectionErrorToast"));
                    //});
                    var accepted = false;

                    if (accepted)
                    {
                        await Execute(func, successCallback, failCallback);
                    }
                    else
                    {
                        await func();
                        await successCallback();
                        //await failCallback(new System.Exception("No Internet"));
                    }
                }
                else
                {
                    await func();
                    await successCallback();
                }
            }
            catch (System.Exception ex)
            {
                await HandleException(ex, func, successCallback, failCallback);
            }
            finally
            {
                finallyCallback?.Invoke();
            }
        }
        private static async Task HandleException<TResult>(System.Exception exception,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            switch (exception)
            {
                case WebException httpTimeoutException:
                    await HandleHttpTimeoutException(httpTimeoutException, func, successCallback, failCallback);
                    break;
                case UnAuthorizedException httpException:
                    await HandleUnAuthorizedHttpException(httpException, func, successCallback, failCallback);
                    break;
                default:
                    await HandleDefaultException(exception, func, successCallback, failCallback);
                    break;
            }
        }

        private static async Task HandleException(System.Exception exception,
            Func<Task> func,
            Func<Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            switch (exception)
            {
                case WebException httpTimeoutException:
                    await HandleHttpTimeoutException(httpTimeoutException, func, successCallback, failCallback);
                    break;
                case UnAuthorizedException httpException:
                    await HandleUnAuthorizedHttpException(httpException, func, successCallback, failCallback);
                    break;
                default:
                    await HandleDefaultException(exception, func, successCallback, failCallback);
                    break;
            }
        }


        private static async Task HandleHttpTimeoutException<TResult>(
           WebException httpTimeoutException,
           Func<Task<TResult>> func,
           Func<TResult, Task> successCallback,
           Func<System.Exception, Task> failCallback)
        {
            var accepted = false;

            if (accepted)
            {
                await Execute(func, successCallback, failCallback);
            }
            else
            {
                await failCallback(httpTimeoutException);
            }
        }

        private static async Task HandleHttpTimeoutException(WebException httpTimeoutException,
            Func<Task> func,
            Func<Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            var accepted = false;

            if (accepted)
            {
                await Execute(func, successCallback, failCallback);
            }
            else
            {
                await failCallback(httpTimeoutException);
            }
        }

        private static async Task HandleUnAuthorizedHttpException(UnAuthorizedException httpException,
            Func<Task> func,
            Func<Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            var accepted = false;

            if (accepted)
            {
                await Execute(func, successCallback, failCallback);
            }
            else
            {
                await failCallback(httpException);
            }
        }

        private static async Task HandleUnAuthorizedHttpException<TResult>(UnAuthorizedException httpException,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            //var employeeId = GlobalSettings.UserName;
            //var password = await SecureStorageHelpers.GetValue(GlobalSettings.PasswordKey);

            //if (GlobalSettings.UserInfo != null && GlobalSettings.RememberMe && !string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(password))
            //{
            //    try
            //    {
            //        Device.BeginInvokeOnMainThread(async() =>
            //        {
            //            var container = ((WPSTORE.App)Application.Current).Container;
            //            var authService = (IAuthenticationService)container.Resolve(typeof(IAuthenticationService));

            //            var result = await authService.LoginAsync(employeeId, password);
            //            if(result != null && result.Token != null)
            //            {
            //                await Execute(func, successCallback, failCallback);
            //            }
            //            else
            //            {
            //                Application.Current.MainPage = new LoginPage();
            //            }
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}
            //else
            //{
            //    Application.Current.MainPage = new LoginPage();
            //}
            Application.Current.MainPage = new LoginPage();
        }

        private static async Task HandleDefaultException<TResult>(System.Exception exception,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            var accepted = false;

            if (accepted)
            {
                await Execute(func, successCallback, failCallback);
            }
            else
            {
                await failCallback(exception);
            }
        }

        private static async Task HandleDefaultException(System.Exception exception,
            Func<Task> func,
            Func<Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            var accepted = false;
            if (accepted)
            {
                await Execute(func, successCallback, failCallback);
            }
            else
            {
                await failCallback(exception);
            }
        }
    }
}
