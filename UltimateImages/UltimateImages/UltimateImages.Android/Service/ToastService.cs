using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateImages.Droid.Service;
using UltimateImages.Service;


[assembly: Xamarin.Forms.Dependency(typeof(ToastService))]
namespace UltimateImages.Droid.Service
{
    public class ToastService : IToastService
    {
        public void ShowLongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long);
        }
        
        public void ShowShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short);
        }
    }
}