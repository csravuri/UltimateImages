using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UltimateImages.Ads;
using UltimateImages.Droid.Ads;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AdsHolderView), typeof(AdsHolderViewRenderer))]
namespace UltimateImages.Droid.Ads
{
    public class AdsHolderViewRenderer : ViewRenderer<AdsHolderView, AdView>
    {
        private AdView adView;

        public AdsHolderViewRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<AdsHolderView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && Control == null)
                SetNativeControl(CreateAdView());
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(AdView.AdUnitId))
                Control.AdUnitId = Element.UnitID;
        }

        private AdView CreateAdView()
        {
            if (adView != null)
                return adView;

            adView = new AdView(Context)
            {
                AdSize = AdSize.Banner,
                AdUnitId = Element.UnitID
            };

            adView.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

            adView.LoadAd(new AdRequest.Builder().Build());

            return adView;
        }
    }
}
