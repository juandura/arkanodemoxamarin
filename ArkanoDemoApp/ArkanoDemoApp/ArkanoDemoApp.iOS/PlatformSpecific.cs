using ArkanoDemoApp.Interfaces;
using ArkanoDemoApp.iOS;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformSpecific))]

namespace ArkanoDemoApp.iOS
{
    public class PlatformSpecific : IPlatformSpecific
    {
    }
}
