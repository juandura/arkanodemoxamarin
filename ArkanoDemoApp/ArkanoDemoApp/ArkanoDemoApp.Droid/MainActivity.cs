using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using ArkanoDemoApp.ViewModels;
using static ArkanoDemoApp.ViewModels.LoginViewModel;
using Java.IO;
using Plugin.Media;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

namespace ArkanoDemoApp.Droid
{
    [Activity(Label = "ArkanoDemoApp", MainLauncher = false, Icon = "@drawable/appIcon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            MobileCenter.Start("a9b966f7-0ae2-453d-bd10-c01510d56d32",
                   typeof(Analytics), typeof(Crashes));

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

