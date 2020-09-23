using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WPSTORE.Services.Interfaces
{
    public interface IDialogService
    {
        Task<bool> ShowConfirmedAsync(string message, string title, string okText, string cancelText);
        Task ShowAlertAsync(string message, string title, string buttonLabel);
        void ShowToast(string message, int duration = 5000);
    }
}
