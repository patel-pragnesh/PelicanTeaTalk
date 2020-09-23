using Plugin.Messaging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WPSTORE.AppSettings;
using WPSTORE.Helpers;
using WPSTORE.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private bool _isSocialOpened;
        private CancellationTokenSource _tokenSource;
        private HomeViewModel _context => (HomeViewModel)BindingContext;
        private CancellationTokenSource animateTimerCancellationTokenSource;
        public HomePage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<string, bool>(MessagingCenterKeys.App, MessagingCenterKeys.Play, OnPlay);
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _context?.LoadNotification();
            await _context?.LoadAsync();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            await _context?.UnloadAsync();
        }

        private void OnPlay(string app, bool isPlaying)
        {
            if (app.Equals(MessagingCenterKeys.App))
            {
                RotateCover(isPlaying);
            }
        }

        private void RotateCover(bool isPlaying)
        {
            if (isPlaying)
            {
                StartCoverAnimation(new CancellationTokenSource());
            }
            else
            {
                ViewExtensions.CancelAnimations(CoverImage);

                if (animateTimerCancellationTokenSource != null)
                {
                    animateTimerCancellationTokenSource.Cancel();
                }
            }
        }

        void StartCoverAnimation(CancellationTokenSource tokenSource)
        {
            try
            {
                animateTimerCancellationTokenSource = tokenSource;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!animateTimerCancellationTokenSource.IsCancellationRequested)
                    {
                        await CoverImage.RelRotateTo(360, Settings.CoverAnimationDuration, Easing.Linear);

                        StartCoverAnimation(animateTimerCancellationTokenSource);
                    }
                });
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine(ex);
            }
        }
        //private async void OnFeaturedItemTapped(object sender, System.EventArgs e)
        //{
        //    await _context.GotoDetailPage();
        //}

        //private async void OnSliderItemTapped(object sender, System.EventArgs e)
        //{
        //    await _context.GotoDetailPage();
        //}

        private async void ShowAllWhatsNews_Tapped(object sender, System.EventArgs e)
        {
            await _context.GotoPostListPage(AppHelpers.GetCategoryId(GlobalSettings.Slug.WhatsNews));
        }
        private async void ShowAllNews_Tapped(object sender, System.EventArgs e)
        {
            await _context.GotoPostListPage(AppHelpers.GetCategoryId(GlobalSettings.Slug.News));
        }
        private async void ShowAllCoffeeLover_Tapped(object sender, System.EventArgs e)
        {
            await _context.GotoPostListPage(AppHelpers.GetCategoryId(GlobalSettings.Slug.CoffeeLover));
        }
        //private async void ShowAllFeatures_Tapped(object sender, System.EventArgs e)
        //{
        //    await _context.GotoPostListPage(AppHelpers.GetCategoryId(GlobalSettings.Slug.Featured));
        //}
        //public async void ShowAllCategories_Tapped(object sender, System.EventArgs e)
        //{
        //    await _context.GotoCategoriesPage();
        //}
        //private async void ShowAllEvents_Tapped(object sender, System.EventArgs e)
        //{
        //    await _context.GotoPostListPage(AppHelpers.GetCategoryId(GlobalSettings.Slug.Events));
        //}
        //private async void OnShowAllPostcategories_Clicked(object sender, System.EventArgs e)
        //{
        //    await _context.GotoPostListPage(AppHelpers.GetCategoryId(GlobalSettings.Slug.Postcategory));
        //}
        //private async void ShowAllPromotions_Tapped(object sender, EventArgs e)
        //{
        //    await _context.GotoPostListPage(AppHelpers.GetCategoryId(GlobalSettings.Slug.Promotion));
        //}
        //private async void OnShowAllUncategorizes_Clicked(object sender, System.EventArgs e)
        //{
        //    await _context.GotoPostListPage(AppHelpers.GetCategoryId(GlobalSettings.Slug.Uncategorized));
        //}

        private void OnSocialClicked(VisualElement sender, TouchEffect.EventArgs.TouchCompletedEventArgs args)
        {
            _tokenSource?.Cancel();
            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;
            _isSocialOpened = !_isSocialOpened;

            Device.BeginInvokeOnMainThread(async () =>
            {
                await FirstSocial.ScaleTo(_isSocialOpened ? 1 : 0);
                if (token.IsCancellationRequested) return;
                await SecondSocial.ScaleTo(_isSocialOpened ? 1 : 0);
                if (token.IsCancellationRequested) return;
                await ThirdSocial.ScaleTo(_isSocialOpened ? 1 : 0);
            });
        }
        private void OnQuestionPicked(VisualElement sender, TouchEffect.EventArgs.TouchCompletedEventArgs args)
        {
            var mailBox = GlobalSettings.MailBox;
            if (_isSocialOpened)
            {
                _context.SendEmail(mailBox);
            }
            OnSocialClicked(null, null);
        }
        private void OnCallPicked(VisualElement sender, TouchEffect.EventArgs.TouchCompletedEventArgs args)
        {
            var contact = GlobalSettings.Call;
            if (_isSocialOpened)
            {
                _context.CallHelp(contact);
            }
            OnSocialClicked(null, null);
        }
        private void OnFacebookPicked(VisualElement sender, TouchEffect.EventArgs.TouchCompletedEventArgs args)
        {
            if (_isSocialOpened)
            {
                AsyncRunner.Run(_context.OpenFacebookChat());
            }
            OnSocialClicked(null, null);
        }

        private async void OnSliderItemTapped(object sender, EventArgs e)
        {
           
            await _context.GotoSlideDetailPage();
        }
    }
}