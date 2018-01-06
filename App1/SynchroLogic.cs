using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Net;
using Android.Preferences;
using Plugin.CurrentActivity;

namespace WeatherApp
{
    [Activity]
    public class SynchroLogic : Activity
    {

        private ServiceConnection ServiceConnection;
        internal bool isBound;
        internal RequestBinder binder;

        public BDD.Initialisation BDD = new WeatherApp.BDD.Initialisation();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            
            CrossCurrentActivity.Current.Activity = this;

            var ServiceIntent = new Intent(this, typeof(APICall));
            ServiceConnection = new ServiceConnection(this);
            BindService(ServiceIntent, ServiceConnection, Bind.AutoCreate);
        }



        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (isBound)
            {
                UnbindService(ServiceConnection);
                isBound = false;
            }
        }

        public bool CheckInternet()
        {
            ConnectivityManager conMgr = (ConnectivityManager)GetSystemService(Context.ConnectivityService);
            NetworkInfo netInfo = conMgr.ActiveNetworkInfo;
            if (netInfo == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    class ServiceConnection : Java.Lang.Object, IServiceConnection
    {
        SynchroLogic activity;

        public ServiceConnection(SynchroLogic activity)
        {
            this.activity = activity;
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            var ServiceBinder = service as RequestBinder;
            if (ServiceBinder != null)
            {
                activity.binder = ServiceBinder;
                activity.isBound = true;
            }

            activity.BDD.DBConnection();

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(activity);
            if (!prefs.GetBoolean("Install", false))
            {
                activity.BDD.CreateTable();

                // mark first time has runned.
                ISharedPreferencesEditor editor = prefs.Edit();
                editor.PutBoolean("Install", true);
                editor.Commit();
            }

            if (activity.CheckInternet())
            {
                DateTime OldDate = activity.BDD.GetOldest();
                TimeSpan difference = (DateTime.Now - OldDate);

                if ((DateTime.Now - OldDate) == (DateTime.Now - new DateTime()) || (difference.Days > 0 || (difference.Hours >= 3 && difference.Minutes > 0)) )
                {
                    APICall Rservice = activity.binder.GetService();

                    
                    string json = Rservice.GetJson();

                    if (json == null)
                    {
                        activity.StartActivity(new Intent(activity, typeof(NoData)));
                    }
                    else
                    {
                        List<WeatherApp.JSON.Weather> temp = JSON.Parsing.JsonToWeather(json);

                        activity.BDD.ClearTable();

                        activity.BDD.InsertList(temp);

                        activity.StartActivity(new Intent(activity, typeof(CreateListView)));
                    }
                    

                }
                else
                {
                    activity.StartActivity(new Intent(activity, typeof(CreateListView)));
                }

            }
            else
            {
                activity.StartActivity(new Intent(activity, typeof(CreateListView)));
            }

            activity.BDD.BDDConnection.Close();
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            activity.isBound = false;
        }
    }
}