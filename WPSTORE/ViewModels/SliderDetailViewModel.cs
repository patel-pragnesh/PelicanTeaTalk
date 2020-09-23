using Prism.Navigation;
using WPSTORE.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WPSTORE.ViewModels
{
    [QueryProperty("ArticleUrl", "articleUrl")]
    public class SliderDetailViewModel : BaseViewModel
    {
        public SliderDetailViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        //Comment OnNavigatedTo function when use App Shell 
        //public override void OnNavigatedTo(INavigationParameters parameters)
        //{
        //    base.OnNavigatedTo(parameters);
        //    if (parameters["ArticleUrl"] != null)
        //        ArticleUrl = parameters["ArticleUrl"].ToString();
        //}

        private string _articleUrl;
        public string ArticleUrl
        {
            get { return _articleUrl; }
            set { SetProperty(ref _articleUrl, value, "ArticleUrl"); }
        }
    }
}
