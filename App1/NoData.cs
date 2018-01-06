
using Android.App;
using Android.OS;
using Android.Views;

namespace WeatherApp
{
    [Activity]
    public class NoData : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Nodata);
        }

        public override void OnBackPressed()
        {
        }
    }
}