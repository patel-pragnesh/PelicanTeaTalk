using Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPSTORE.Models.WooCommerce;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Android.Webkit;
using Android.Views;
using Newtonsoft.Json;

namespace WPSTORE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Android.Runtime.Register("android/webkit/WebView", ApiSince = 1, DoNotGenerateAcw = true)]
    public partial class PayByCardPage : ContentPage
    {       
        public PayByCardPage(OrderModel orderModel)
        {
            InitializeComponent();           
        }

        public PayByCardPage(string URL,OrderModel orderModel)
        {
            Button button = new Button
            {
                Text = "Back TO SHOP HOME",
                VerticalOptions = LayoutOptions.StartAndExpand,
                HeightRequest = 50,
                WidthRequest = 50,
                BackgroundColor = Color.Orange
               
            };
            button.Clicked += OnButtonClicked;


            string PreparedURL = Prepare_PaymentGatewayCall(orderModel);

            var PaymentPageView = new Xamarin.Forms.WebView
            {
                Source = (GlobalSettings.WebXPayRequest + PreparedURL),
                HeightRequest = (Application.Current.MainPage.Height * 90/100),
                WidthRequest = (Application.Current.MainPage.Width),
                FlowDirection = FlowDirection.MatchParent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            PaymentPageView.BackgroundColor = Color.Yellow;
            Content = new StackLayout { Children = { PaymentPageView,button } };
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OrdersPage());
        }

        private string Prepare_PaymentGatewayCall(OrderModel orderModel)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("?");

                sb.Append("Customer_ID");
                sb.Append("=" + orderModel.CustomerId);
                sb.Append("&");

                sb.Append("first_name");
                sb.Append("=" + orderModel.Billing.FirstName);
                sb.Append("&");

                sb.Append("last_name");
                sb.Append("=" + orderModel.Billing.LastName);
                sb.Append("&");

                sb.Append("email");
                sb.Append("=" + orderModel.Billing.Email);
                sb.Append("&");

                sb.Append("contact_number");
                sb.Append("=" + orderModel.Billing.Phone);
                sb.Append("&");

                sb.Append("address_line_one");
                sb.Append("=" + orderModel.Billing.Address1);
                sb.Append("&");

                sb.Append("address_line_two");
                sb.Append("=" + orderModel.Billing.Address2);
                sb.Append("&");

                sb.Append("city");
                sb.Append("=" + orderModel.Billing.City);
                sb.Append("&");

                sb.Append("state");
                sb.Append("=" + orderModel.Billing.State);
                sb.Append("&");

                sb.Append("postal_code");
                sb.Append("=" + orderModel.Billing.Postcode);
                sb.Append("&");

                sb.Append("country");
                sb.Append("=" + orderModel.Billing.Country);
                sb.Append("&");

                sb.Append("process_currency");
                sb.Append("=" + orderModel.Currency);
                sb.Append("&");

                sb.Append("cms");
                sb.Append("=" + "PHP");
                sb.Append("&");

                sb.Append("Total");
                sb.Append("=" + orderModel.Total);
                sb.Append("&");

                sb.Append("Customer_Id");
                sb.Append("=" + orderModel.CustomerId);
                sb.Append("&");

                sb.Append("Order_Key");
                sb.Append("=" + orderModel.OrderKey);
                sb.Append("&");

                sb.Append("Order_ID");
                sb.Append("=" + orderModel.Id);

                return sb.ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}