using ArkanoDemoApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ArkanoDemoApp
{
    public class App : Application
    {
        public App()
        {
            MainPage = new Login();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static void NavigateTo(WaitingPage page)
        {
            App.Current.MainPage = page;
        }
    }
}
