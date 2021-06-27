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
    public partial class ImagesHome : ContentPage
    {
        private ImagesHomeViewModel viewModels;

        public ImagesHome()
        {
            InitializeComponent();
            BindingContext = viewModels = new ImagesHomeViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModels.LoadImagesCommand.Execute(null);
        }

        private void Image_Clicked(object sender, EventArgs e)
        {
            ImageButton imgBtn = sender as ImageButton;
            if (imgBtn != null)
            {
                viewModels.ImageSelectedCommand.Execute(imgBtn.CommandParameter);
            }
        }
    }
}