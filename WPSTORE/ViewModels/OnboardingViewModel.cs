using PanCardView.Enums;
using PanCardView.EventArgs;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WPSTORE.Languages.Texts;
using WPSTORE.Models;
using WPSTORE.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace WPSTORE.ViewModels
{
    [Preserve(AllMembers = true)]
    public class OnboardingViewModel : BaseViewModel
    {
        private ObservableCollection<Boarding> boardings;

        private string nextButtonText = TextsTranslateManager.Translate("Next");

        private bool isSkipButtonVisible = true;

        private int selectedIndex;

        public OnboardingViewModel(INavigationService navigationService) : base(navigationService)
        {
            Boardings = new ObservableCollection<Boarding>
            {
                new Boarding
                {
                    ImagePath = "ChooseGradient.svg",
                    Header = TextsTranslateManager.Translate("Choose"),
                    Content = TextsTranslateManager.Translate("PickItem")
                },
                new Boarding
                {
                    ImagePath = "ConfirmGradient.svg",
                    Header = TextsTranslateManager.Translate("OrderConfirmed"),
                    Content = TextsTranslateManager.Translate("OrderConfirmedText")
                },
                new Boarding
                {
                    ImagePath = "DeliverGradient.svg",
                    Header = TextsTranslateManager.Translate("Delivered"),
                    Content = TextsTranslateManager.Translate("DeliveredText")
                }

            };

            SkipCommand = new Command(this.Skip);
            NextCommand = new Command(this.Next);
        }

        public ICommand SkipCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand BoardingItemSwipeCommand => new Command<ItemSwipedEventArgs>((obj) =>
        {
            var index = FindNextIndex(obj.Index, obj.Direction);
            ValidateSelection(index);
        });
        private int FindNextIndex(int currentIndex, ItemSwipeDirection swipeDirection)
        {
            if (swipeDirection == ItemSwipeDirection.Left)
            {
                if(currentIndex < Boardings.Count - 1)
                    return currentIndex + 1;
            }
            else if (swipeDirection == ItemSwipeDirection.Right)
            {
                if(currentIndex > 0)
                    return currentIndex - 1;
            }
            return currentIndex;
        }

        public ObservableCollection<Boarding> Boardings
        {
            get { return boardings; }
            set { SetProperty(ref boardings, value); }
        }

        public string NextButtonText
        {
            get { return nextButtonText; }
            set { SetProperty(ref nextButtonText, value); }
        }

        public bool IsSkipButtonVisible
        {
            get { return isSkipButtonVisible; }
            set { SetProperty(ref isSkipButtonVisible, value); }
        }

        public int SelectedItemIndex
        {
            get { return selectedIndex; }
            set { SetProperty(ref selectedIndex, value); }
        }

        private bool ValidateAndUpdateSelectedIndex()
        {
            ValidateSelection(SelectedItemIndex + 1);

            if (SelectedItemIndex >= Boardings.Count - 1) 
                return true;

            SelectedItemIndex++;
            return false;
        }

        private void ValidateSelection(int index)
        { 
            if (index <= Boardings.Count - 2)
            {
                IsSkipButtonVisible = true;
                NextButtonText = TextsTranslateManager.Translate("Next");
            }
            else
            {
                NextButtonText = TextsTranslateManager.Translate("Done");
                IsSkipButtonVisible = false;
            }
        }

        private void Skip(object obj)
        {
            MoveToNextPage();
        }

        private void Next(object obj)
        {
            if (ValidateAndUpdateSelectedIndex())
            {
                MoveToNextPage();
            }
        }

        private void MoveToNextPage()
        {
            Application.Current.MainPage = new LoginPage();
        }
    }
}
