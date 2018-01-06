using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android.Locations;
using Android;
using Android.Content.PM;
using Plugin.CurrentActivity;
using Android.Util;
using WeatherApp.BDD;
using SQLite.Net;

namespace WeatherApp
{
    [Service(Label = "Geolocalisation")]
    public class ServiceGeolocalisation : Service
    {
        protected LocationManager locationManager;
        LocalisationListener locationListener;
        protected string Provider = LocationManager.GpsProvider;
        private static long MIN_DISTANCE_FOR_UPDATE = 1;
        private static long MIN_TIME_FOR_UPDATE = 1000 * 60 * 2;
        
        public static Localisation GetLocation()
        {
            Initialisation BDD = new Initialisation();

            BDD.DBConnection();

            TableQuery<Localisation> LocationList  = BDD.BDDConnection.Table<Localisation>();

            Localisation Location = null;
            if (LocationList.Count() != 0)
            {
                Location = LocationList.First();
            }
            

            BDD.BDDConnection.Close();

            return Location;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            StopSelf();
        }

        public LocationManager GetLocationManager()
        {
            return locationManager;
        }

        private void SetLocationManager(LocationManager p_locationManager)
        {
            locationManager = p_locationManager;
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            if (CrossCurrentActivity.Current.Activity != null)
            {
                locationManager = (LocationManager)CrossCurrentActivity.Current.Activity.GetSystemService(Context.LocationService);

                locationListener = new LocalisationListener(this);

                TryGetLocationAsync();

            }
            return base.OnStartCommand(intent, flags, startId);
        }

        public void TryGetLocationAsync()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                GetLocationAsync(Provider);
                return;
            }
            GetLocationPermissionAsync(Provider);
        }

        private void GetLocationPermissionAsync(string p_Provider)
        {
            const string permission = Manifest.Permission.AccessFineLocation;
            if (CrossCurrentActivity.Current.Activity.CheckSelfPermission(permission) == (int)Permission.Granted)
            {
                GetLocationAsync(p_Provider);
            }

        }

        /* Peut etre execute en synchrone car recupere uste la position*/
        public void GetLocationAsync(String p_Provider)
        {
            LocationManager l_LocationManager = GetLocationManager();


            Initialisation BDD = new Initialisation();
            BDD.DBConnection();

            var locationCriteria = new Criteria
            {
                Accuracy = Accuracy.Fine,
                PowerRequirement = Power.Medium
            };
                var locationProvider = l_LocationManager.GetBestProvider(locationCriteria, true);
                l_LocationManager.RequestLocationUpdates(p_Provider, MIN_TIME_FOR_UPDATE, MIN_DISTANCE_FOR_UPDATE, locationListener);
                var GPSEnabled = l_LocationManager.IsProviderEnabled(p_Provider);
                if (GPSEnabled)
                {
                    if (l_LocationManager != null)
                    {
                        Localisation location = new Localisation();
                        
                        Location p_location = getLastKnownLocation();
                        if (p_location != null)
                        {
                            BDD.BDDConnection.DeleteAll<Localisation>();
                            location.Latitude = (float)p_location.Latitude;
                            location.Longitude = (float)p_location.Longitude;
                            BDD.BDDConnection.InsertOrIgnore(location);
                        }

                    }
                }
            else
            {
                Toast.MakeText(this, "GPS is disabled. It is required for the first start and to update position.", ToastLength.Long).Show();
            }
            
            BDD.BDDConnection.Close();
            return;
        }

        private Location getLastKnownLocation()
        {
            LocationManager l_LocationManager = GetLocationManager();
            List<string> providers = new List<string>(l_LocationManager.GetProviders(true));
            Location bestLocation = null;
            foreach (String provider in providers)
            {
                Location l = l_LocationManager.GetLastKnownLocation(provider);
                if (l == null)
                {
                    continue;
                }
                if (bestLocation == null || l.Accuracy < bestLocation.Accuracy)
                {
                    bestLocation = l;
                }
            }
            return bestLocation;
        }

        private class LocalisationListener : Java.Lang.Object, ILocationListener
        {
            private ServiceGeolocalisation serviceGeolocalisation;
            const int RequestLocationId = 0;
            

            public LocalisationListener(ServiceGeolocalisation p_ServiceLocation)
            {
                this.serviceGeolocalisation = p_ServiceLocation;
            } 

            public void OnLocationChanged(Location p_location)
            {
                if (p_location != null)
                {
                    Localisation location = new Localisation();
                    location.Latitude = (float )p_location.Latitude;
                    location.Longitude = (float)p_location.Longitude;
                    Initialisation BDD = new Initialisation();
                    BDD.DBConnection();
                    BDD.BDDConnection.InsertOrIgnore(location);
                    BDD.BDDConnection.Close();
                }
            }

            public void OnProviderDisabled(string provider)
            {
                //GPS eteint
            }

            public void OnProviderEnabled(string provider)
            {
                //GPS allumé
            }

            public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
            {
                Log.Info("WEATHER APP", ServiceGeolocalisation.GetLocation().ToString());
            }
        }
    }
}