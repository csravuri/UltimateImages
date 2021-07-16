using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace UltimateImages.Ads
{
    public class AdsHolderView : View
    {
        public BindableProperty UnitIDProperty = BindableProperty.Create(nameof(UnitID), typeof(string), typeof(AdsHolderView));
        public string UnitID
        {
            get => (string)GetValue(UnitIDProperty);
            set => SetValue(UnitIDProperty, value);
        }        
    }
}
