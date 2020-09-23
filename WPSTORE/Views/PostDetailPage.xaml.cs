using WPSTORE.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostDetailPage : ContentPage
    {
        private PostDetailViewModel _context => (PostDetailViewModel)BindingContext;

        public PostDetailPage()
        {
            InitializeComponent();
        }

        protected void OnImageViewTapped(object sender, EventArgs e)
        {
            try
            {
                string url = ((TappedEventArgs)e).Parameter.ToString();
                new PhotoBrowser
                {
                    Photos = new List<Photo>
                {
                    new Photo
                    {
                        URL = url,
                    }
                },
                    ActionButtonPressed = (index) =>
                    {
                        Debug.WriteLine($"Clicked {index}");
                        PhotoBrowser.Close();
                    },
                    EnableGrid = false,
                }.Show();
            }
            catch(Exception ex)
            {
            }
        }
    }
}