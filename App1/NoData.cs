
using Android.App;
using Android.Locations;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace WeatherApp
{
    [Activity]
    public class NoData : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            LocationManager tmp = (LocationManager)GetSystemService(LocationService);
            var GPSEnabled = tmp.IsProviderEnabled(Android.Locations.LocationManager.GpsProvider);
            if (!GPSEnabled)
            {
                Toast.MakeText(this, "GPS is disabled. It is required for the first start and to update position.", ToastLength.Long).Show();
            }

                SetContentView(Resource.Layout.Nodata);
        }

        public override void OnBackPressed()
        {
        }
    }
}