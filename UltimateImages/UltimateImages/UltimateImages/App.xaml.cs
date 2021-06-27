using System;
using UltimateImages.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UltimateImages
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Database.DBConnect.GetDBConnect();

            MainPage = new NavigationPage(new ImagesHome()) 
            {
                BarBackgroundColor = Color.FromHex("#570099")
            };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
