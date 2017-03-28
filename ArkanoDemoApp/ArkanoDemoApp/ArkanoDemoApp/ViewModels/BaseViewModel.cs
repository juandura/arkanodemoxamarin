using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArkanoDemoApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        public delegate Task DisplayAlertDelegate(string title, string message, string cancel);

        public DisplayAlertDelegate DisplayAlert { get; set; }

        public delegate Task<bool> DisplayConfirmDelegate(string title, string message, string accept, string cancel);

        public DisplayConfirmDelegate DisplayConfirm { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public delegate void DisplayModalDelegate();

        public DisplayModalDelegate DisplayModal { get; set; }

        #region Property changed/set methods

        protected void RaiseAllPropertiesChanged()
        {
            // By convention, an empty string indicates all properties are invalid.
            this.PropertyChanged(this, new PropertyChangedEventArgs(string.Empty));
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propExpr)
        {
            var prop = (PropertyInfo)((MemberExpression)propExpr.Body).Member;
            this.RaisePropertyChanged(prop.Name);
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetPropertyValue<T>(ref T storageField, T newValue, Expression<Func<T>> propExpr)
        {
            if (Equals(storageField, newValue))
            {
                return false;
            }

            storageField = newValue;
            var prop = (PropertyInfo)((MemberExpression)propExpr.Body).Member;
            this.RaisePropertyChanged(prop.Name);

            return true;
        }

        protected bool SetPropertyValue<T>(ref T storageField, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (Equals(storageField, newValue))
            {
                return false;
            }

            storageField = newValue;
            this.RaisePropertyChanged(propertyName);

            return true;
        }

        #endregion Property changed/set methods

        #region Loader

        protected void SendWaitingMessage(bool waiting)
        {
            MessagingCenter.Send(this, "IsWaiting", waiting);
        }

        #endregion Loader
    }
}
