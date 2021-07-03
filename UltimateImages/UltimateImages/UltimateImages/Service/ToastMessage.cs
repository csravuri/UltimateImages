using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace UltimateImages.Service
{
    public static class ToastMessage 
    {
        public static void ShowLongAlert(string message)
        {
            DependencyService.Get<IMessage>().LongAlert(message);
        }

        public static void ShowShortAlert(string message)
        {
            DependencyService.Get<IMessage>().ShortAlert(message);
        }
    }
}
