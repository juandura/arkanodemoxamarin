using ArkanoDemoApp.Utils;
using ArkanoDemoApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ArkanoDemoApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Attributes And Properties

        public ICommand LogoutCommand { get; private set; }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                this.RaisePropertyChanged();
            }
        }

        private bool isIndicatorActive = false;
        public bool IsIndicatorActive
        {
            get
            {
                return this.isIndicatorActive;
            }

            set
            {
                this.isIndicatorActive = value;

                this.RaisePropertyChanged();
            }
        }

        #endregion Attributes And Properties

        #region Constructors

        public HomeViewModel()
        {
            this.LogoutCommand = new Command(async () => await this.OnLogoutCommand());
        }

        #endregion Constructors

        #region Operations

        public async Task RefreshViewModel()
        {
            this.SendWaitingMessage(true);
            IsIndicatorActive = true;
            try
            {
                //Load data if necessary.
                //this.UserName = string.Empty;

                await Task.Delay(4000);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Log(ex, this.DisplayAlert);
            }
            this.SendWaitingMessage(false);
            IsIndicatorActive = false;
        }

        public async Task DisposeViewModel()
        {
            try
            {
                this.UserName = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Log(ex, this.DisplayAlert);
            }
        }

        public async Task OnLogoutCommand()
        {
            try
            {
                this.SendWaitingMessage(true);
                //TODO: hacer logout y volver a la pagina de login.
                App.NavigateTo(new Login());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Arkano Demo App", "Problems with internet access.", "OK");
            }
        }

        #endregion Operations
    }
}
