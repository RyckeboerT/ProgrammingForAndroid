using Android.App;
using Android.Widget;
using Android.OS;
using Android.Runtime;
using Plugin.Permissions;
using System;
using Plugin.CurrentActivity;
using Android.Support.V4.App;
using Android;
using Android.Content;
using Android.Views;
using Android.Preferences;

namespace WeatherApp
{
    [Activity(MainLauncher = true)]
    public class StartActivity : Activity, ActivityCompat.IOnRequestPermissionsResultCallback 
    {
        /**
     	* Id to identify application request.
     	*/
        static readonly int REQUEST_LOCATION = 0;
        private BDD.Initialisation BDD = new WeatherApp.BDD.Initialisation();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            CrossCurrentActivity.Current.Activity = this;

            StartService(new Android.Content.Intent(this,typeof(ServiceGeolocalisation)));
                        
            View view = this.LayoutInflater.Inflate(Resource.Layout.Main, null);
            view.FindViewById<ImageView>(Resource.Id.Loading).SetImageResource(Resource.Drawable.Loading);
            SetContentView(view);

            ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.AccessFineLocation, Manifest.Permission.WriteExternalStorage }, REQUEST_LOCATION);

        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            if (!prefs.GetBoolean("Install", false))
            {
                BDD.CreateBDD();
            }

            Intent intent = new Intent(this, typeof(SynchroLogic));
            StartActivity(intent);
        }        
    }

    
}

