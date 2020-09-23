using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WPSTORE.Controls
{
    public class WpSearchHandler : SearchHandler
    {
        public event EventHandler<string> OnSearchTextChange;
        public event EventHandler<object> OnSearchItemSelected;

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);
            OnSearchTextChange?.Invoke(this, newValue);
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            await Task.Delay(1000);
            OnSearchItemSelected?.Invoke(this, item);
        }
    }
}
