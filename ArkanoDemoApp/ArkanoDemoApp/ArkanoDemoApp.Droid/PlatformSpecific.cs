using ArkanoDemoApp.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using ArkanoDemoApp.Droid;
using Android.Provider;
using Java.IO;
using Android.Net;
using static ArkanoDemoApp.ViewModels.LoginViewModel;

[assembly: Dependency(typeof(PlatformSpecific))]

namespace ArkanoDemoApp.Droid
{
    public class PlatformSpecific : IPlatformSpecific
    {
    }
}