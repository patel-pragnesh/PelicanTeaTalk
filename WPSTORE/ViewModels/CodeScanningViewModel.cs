using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using WPSTORE.Services.Interfaces;

namespace WPSTORE.ViewModels
{
    public class CodeScanningViewModel : BaseViewModel 
    {
        private IAppService _appService;
        public CodeScanningViewModel(INavigationService navigationService, IAppService appService) : base(navigationService)
        {
            _appService = appService;
        }
    }
}
