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
    public partial class RedemptionHistoryPage : ContentPage
    {
        private RedemptionHistoryViewModel _context => (RedemptionHistoryViewModel)BindingContext;
        public RedemptionHistoryPage()
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                await _context.LoadTransactions();
                _context.LoadUsedVouchers();
            });
        }
    }
}