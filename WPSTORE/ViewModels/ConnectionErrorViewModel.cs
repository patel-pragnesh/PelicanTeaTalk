using Prism.Navigation;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WPSTORE.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace WPSTORE.ViewModels
{
    /// <summary>
    /// ViewModel for no internet connection page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ConnectionErrorViewModel : BaseViewModel
    {
        #region Fields

        private string imagePath;

        private string header;

        private string content;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="ConnectionErrorViewModel" /> class.
        /// </summary>
        public ConnectionErrorViewModel(INavigationService navigationService) : base(navigationService)
        {
            this.ImagePath = "NoInternet.svg";
            this.Header = "NO INTERNET";
            this.Content = "You must be connected to the internet to complete this action";
            this.TryAgainCommand = new Command(this.TryAgain);
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that is executed when the TryAgain button is clicked.
        /// </summary>
        public ICommand TryAgainCommand { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ImagePath.
        /// </summary>

        public string ImagePath
        {
            get { return imagePath; }
            set { SetProperty(ref imagePath, value); }
        }

        /// <summary>
        /// Gets or sets the Header.
        /// </summary>
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }
        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
        public string Content
        {
            get { return content; }
            set { SetProperty(ref content, value); }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the Try again button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void TryAgain(object obj)
        {
            // Do something
        }

        #endregion
    }
}
