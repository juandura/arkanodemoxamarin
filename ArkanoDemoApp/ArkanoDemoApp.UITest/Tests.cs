using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace ArkanoDemoApp.UITest
{
    [TestFixture(Platform.Android)]
    //[TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }

        [Test]
        public void LoginTest()
        {
            app.Tap(x => x.Class("EntryEditText"));
            app.EnterText(x => x.Class("EntryEditText"), "info@arkanosoft.com");
            app.Tap(x => x.Class("EntryEditText").Index(1));
            app.EnterText(x => x.Class("EntryEditText").Index(1), "12345678");
            app.DismissKeyboard();
            app.Tap(x => x.Text("Login"));
            app.Screenshot("UserLogged");
        }

        [Test]
        public void LoginTestExit()
        {
            app.Tap(x => x.Class("EntryEditText"));
            app.EnterText(x => x.Class("EntryEditText"), "info@arkanosoft.com");
            app.DismissKeyboard();
            app.Screenshot("NoKeyBoard");
            app.Back();
        }
    }
}

