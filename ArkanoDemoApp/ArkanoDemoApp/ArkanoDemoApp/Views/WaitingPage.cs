using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArkanoDemoApp.Views
{
    /// <summary>
    /// Loading icon
    /// </summary>
    /// <seealso cref="Xamarin.Forms.ContentPage" />
    public class WaitingPage : ContentPage
    {
        /// <summary>
        /// The is waiting property
        /// </summary>
        public static readonly BindableProperty IsWaitingProperty = BindableProperty.Create("IsWaiting", typeof(bool), typeof(WaitingPage), false);

        /// <summary>
        /// The show loading frame property
        /// </summary>
        public static readonly BindableProperty ShowLoadingFrameProperty = BindableProperty.Create("ShowLoadingFrame", typeof(bool), typeof(WaitingPage), false);

        /// <summary>
        /// The show loading message property
        /// </summary>
        public static readonly BindableProperty ShowLoadingMessageProperty = BindableProperty.Create("ShowLoadingMessage", typeof(bool), typeof(WaitingPage), false);

        /// <summary>
        /// The shade background property
        /// </summary>
        public static readonly BindableProperty ShadeBackgroundProperty = BindableProperty.Create("ShadeBackground", typeof(bool), typeof(WaitingPage), false);

        /// <summary>
        /// The loading message property
        /// </summary>
        public static readonly BindableProperty LoadingMessageProperty = BindableProperty.Create("LoadingMessage", typeof(string), typeof(WaitingPage), "Loading...");

        /// <summary>
        /// The waiting orientation property
        /// </summary>
        public static readonly BindableProperty WaitingOrientationProperty = BindableProperty.Create("WaitingOrientation", typeof(StackOrientation), typeof(WaitingPage), StackOrientation.Vertical);

        /// <summary>
        /// Gets or sets a value indicating whether this instance is waiting.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is waiting; otherwise, <c>false</c>.
        /// </value>
        public bool IsWaiting
        {
            get
            {
                return (bool)GetValue(IsWaitingProperty);
            }

            set
            {
                if (value)
                {
                    this.ShowIndicator();
                }
                else
                {
                    this.HideIndicator();
                }
            }
        }

        /// <summary>
        /// Gets or sets the loading message.
        /// </summary>
        /// <value>
        /// The loading message.
        /// </value>
        public string LoadingMessage
        {
            get
            {
                return (string)GetValue(LoadingMessageProperty);
            }

            set
            {
                this.SetValue(LoadingMessageProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show loading message].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show loading message]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowLoadingMessage
        {
            get
            {
                return (bool)GetValue(ShowLoadingMessageProperty);
            }

            set
            {
                this.SetValue(ShowLoadingMessageProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show loading frame].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show loading frame]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowLoadingFrame
        {
            get
            {
                return (bool)GetValue(ShowLoadingFrameProperty);
            }

            set
            {
                this.SetValue(ShowLoadingFrameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [shade background].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [shade background]; otherwise, <c>false</c>.
        /// </value>
        public bool ShadeBackground
        {
            get
            {
                return (bool)GetValue(ShadeBackgroundProperty);
            }

            set
            {
                this.SetValue(ShadeBackgroundProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the waiting orientation.
        /// </summary>
        /// <value>
        /// The waiting orientation.
        /// </value>
        public StackOrientation WaitingOrientation
        {
            get
            {
                return (StackOrientation)GetValue(WaitingOrientationProperty);
            }

            set
            {
                this.SetValue(WaitingOrientationProperty, value);
            }
        }

        /// <summary>
        /// Sets the View element representing the content of the Page.
        /// </summary>
        /// <value>
        /// A <see cref="T:Xamarin.Forms.View" /> subclass, or <see langword="null" />.
        /// </value>
        public new View Content
        {
            set
            {
                this.waitingPageContent.Content = value;
            }
        }

        /// <summary>
        /// Gets or sets the indicator.
        /// </summary>
        /// <value>
        /// The indicator.
        /// </value>
        public ActivityIndicator Indicator { get; set; }

        /// <summary>
        /// The waiting page content
        /// </summary>
        private ContentView waitingPageContent;

        /// <summary>
        /// The content layout
        /// </summary>
        private Grid contentLayout;

        /// <summary>
        /// The frame layout
        /// </summary>
        private Frame frameLayout;

        /// <summary>
        /// The shaded background color
        /// </summary>
        private Color shadedBackgroundColor = Color.Black.MultiplyAlpha(0.3); //Color.Black.MultiplyAlpha(0.2);

        /// <summary>
        /// The transparent background color
        /// </summary>
        private Color transparentBackgroundColor = Color.Transparent;

        /// <summary>
        /// The white background color
        /// </summary>
        private Color whiteBackgroundColor = Color.FromRgba(255, 255, 255, 1.0);    // Background color of White, Opaque is required for Android

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitingPage"/> class.
        /// </summary>
        public WaitingPage()
        {
            this.waitingPageContent = new ContentView
            {
                Content = null,
            };

            this.contentLayout = new Grid
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                Padding = new Thickness(0, 0, 0, 0),
                BackgroundColor = this.ShadeBackground ? this.shadedBackgroundColor : this.transparentBackgroundColor,
            };
            this.contentLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            this.contentLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            base.Content = this.contentLayout;
        }

        /// <summary>
        /// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (this.Indicator == null)
            {
                this.Indicator = new ActivityIndicator
                {
                    Color = Color.Black,
                    Scale = 1.5,
                    IsEnabled = true,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                };
            }

            if (this.IsWaiting)
            {
                this.ShowIndicator();
            }

            this.BuildIndicatorFrame();

            this.contentLayout.Children.Add(this.waitingPageContent, 0, 0);
            this.contentLayout.Children.Add(this.frameLayout, 0, 0);
        }

        /// <summary>
        /// Builds the indicator frame.
        /// </summary>
        private void BuildIndicatorFrame()
        {
            var loadingLabel = new Label
            {
                TextColor = Color.Black,
                Text = this.LoadingMessage,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
            };

            var stack = new StackLayout
            {
                Spacing = 15,
                Orientation = this.WaitingOrientation,
                Children =
                {
                    this.Indicator
                }
            };

            switch (this.WaitingOrientation)
            {
                case StackOrientation.Vertical:
                    if (this.ShowLoadingMessage)
                    {
                        stack.Children.Insert(0, loadingLabel);
                    }

                    break;

                case StackOrientation.Horizontal:
                    if (this.ShowLoadingMessage)
                    {
                        stack.Children.Add(loadingLabel);
                    }

                    break;
            }

            this.frameLayout = new Frame
            {
                //BackgroundColor = this.ShowLoadingFrame ? this.whiteBackgroundColor : this.transparentBackgroundColor,
                //HorizontalOptions = LayoutOptions.Center,
                //VerticalOptions = LayoutOptions.Center,
                BackgroundColor = this.ShowLoadingFrame && this.ShadeBackground ? this.shadedBackgroundColor : this.transparentBackgroundColor,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,

                OutlineColor = this.ShowLoadingFrame ? Color.Black : this.transparentBackgroundColor,
                IsVisible = this.IsWaiting,
                HasShadow = false,
                Content = stack,
            };
        }

        /// <summary>
        /// Shows the indicator.
        /// </summary>
        private void ShowIndicator()
        {
            if (this.Indicator != null)
            {
                this.Indicator.IsRunning = true;
            }

            if (this.frameLayout != null)
            {
                this.frameLayout.IsVisible = true;
            }

            this.contentLayout.BackgroundColor = this.ShadeBackground ? this.shadedBackgroundColor : this.transparentBackgroundColor;
        }

        /// <summary>
        /// Hides the indicator.
        /// </summary>
        private void HideIndicator()
        {
            if (this.Indicator != null)
            {
                this.Indicator.IsRunning = false;
            }

            if (this.frameLayout != null)
            {
                this.frameLayout.IsVisible = false;
            }

            this.contentLayout.BackgroundColor = this.transparentBackgroundColor;
        }

        //TODO: Quitar y utilizar la extension.
        public void ConfigureWaiting()
        {
            //// waitinPage.ShowLoadingMessage = true;
            this.ShadeBackground = false;
            this.ShowLoadingFrame = true;
            //// waitinPage.LoadingMessage = "Cargando";
        }
    }
}
