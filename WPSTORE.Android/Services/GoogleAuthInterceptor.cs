using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System;

namespace WPSTORE.Droid.Services
{
    [Activity(Label = "GoogleAuthInterceptor", NoHistory = false, LaunchMode = LaunchMode.SingleTask)]
    [
        IntentFilter
        (
            actions: new[] { Intent.ActionView },
            Categories = new[]
            {
                Intent.CategoryDefault,
                Intent.CategoryBrowsable
            },
            DataSchemes = new[]
            {
                // First part of the redirect url (Package name)
                "com.googleusercontent.apps.877130854950-fjai37crgsccjmbp8nbhlh49e4tuhsc5"
            },
            DataPaths = new[]
            {
                // Second part of the redirect url (Path)
                "/oauth2redirect"
            }
        )
    ]
    public class GoogleAuthInterceptor : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Android.Net.Uri uri_android = Intent.Data;

            // Convert Android Url to C#/netxf/BCL System.Uri

            var uri_netfx = new Uri(uri_android.ToString());

            // Send the URI to the Authenticator for continuation
            App.Authenticator.OnPageLoading(uri_netfx);

            var intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(intent);

            Finish();
        }
    }
}