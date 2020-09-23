using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPSTORE.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RewardsPage : ContentPage
    {
        private RewardsViewModel _context => (RewardsViewModel)BindingContext;
        public RewardsPage()
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                await _context.LoadExchangeVouchers();
            });
        }
    }
}