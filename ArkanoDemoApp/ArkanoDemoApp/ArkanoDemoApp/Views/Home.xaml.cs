using ArkanoDemoApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ArkanoDemoApp.Views
{
    public partial class Home : WaitingPage
    {
        private HomeViewModel viewModel;

        public Home(HomeViewModel viewModel)
        {
            this.viewModel = viewModel;
            NavigationPage.SetHasNavigationBar(this, true);
            this.ConfigureWaiting();
            MessagingCenter.Subscribe<BaseViewModel, bool>(this, "IsWaiting", (sender, value) => { IsWaiting = value; });
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            this.viewModel.Navigation = this.Navigation;
            this.viewModel.DisplayConfirm = this.DisplayAlert;
            this.viewModel.DisplayAlert = this.DisplayAlert;
            this.BindingContext = this.viewModel;

            var viewModel = BindingContext as HomeViewModel;
            if (viewModel == null)
            {
                return;
            }
            await viewModel.RefreshViewModel();
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();

            var viewModel = BindingContext as HomeViewModel;
            if (viewModel == null)
            {
                return;
            }
            await viewModel.DisposeViewModel();
        }
    }
}
