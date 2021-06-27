using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateImages.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UltimateImages.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagesDisplay : ContentPage
    {
        private ImagesDisplayViewModel imagesDisplayViewModel;

        public ImagesDisplay(ImagesDisplayViewModel imagesDisplayViewModel)
        {
            InitializeComponent();
            BindingContext = this.imagesDisplayViewModel = imagesDisplayViewModel;
        }
    }
}