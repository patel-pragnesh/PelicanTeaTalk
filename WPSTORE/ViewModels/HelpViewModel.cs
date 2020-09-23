using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPSTORE.ViewModels
{
    public class HelpViewModel : BaseViewModel
    {
        public HelpViewModel(INavigationService navigationService) : base(navigationService)
        {
            HelpPageUrl = GlobalSettings.HelpPageUrl;
        }

        private string _helpUrl;
        public string HelpPageUrl
        {
            get { return _helpUrl; }
            set { SetProperty(ref _helpUrl, value, "HelpPageUrl"); }
        }
    }
}
