using ArkanoDemoApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkanoDemoApp.Utils
{
    public static class ExceptionHandler
    {
        public static void Log(Exception e, BaseViewModel.DisplayAlertDelegate del = null)
        {
            if (del != null)
            {
                del("Exception", e.Message + " | " + e.StackTrace, "OK");
            }
        }
    }
}
