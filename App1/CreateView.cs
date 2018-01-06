using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WeatherApp.BDD;
using Newtonsoft.Json;

namespace WeatherApp
{
    [Activity]
    public class CreateView : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            Intent intent = Intent;

            string weather = intent.GetStringExtra("weather");

            Table table = JsonConvert.DeserializeObject<Table>(weather);

            View view;
            view = this.LayoutInflater.Inflate(Resource.Layout.Detail, null);
            if (table != null)
            {
                string Test = table.Icon;
                switch (Test)
                {
                    case "01d":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Cleard);
                        break;
                    case "01n":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Clearn);
                        break;
                    case "02d":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Fewd);
                        break;
                    case "02n":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Fewn);
                        break;
                    case "03d":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Scatteredd);
                        break;
                    case "03n":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Scatteredn);
                        break;
                    case "04d":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Brokend);
                        break;
                    case "04n":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Brokenn);
                        break;
                    case "09d":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Showerd);
                        break;
                    case "09n":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Showern);
                        break;
                    case "10d":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Raind);
                        break;
                    case "10n":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Rainn);
                        break;
                    case "11d":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Thunderstormd);
                        break;
                    case "11n":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Thunderstormn);
                        break;
                    case "13d":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Snowd);
                        break;
                    case "13n":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Snown);
                        break;
                    case "50d":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Mistd);
                        break;
                    case "50n":
                        view.FindViewById<ImageView>(Resource.Id.IconDetail).SetImageResource(Resource.Drawable.Mistn);
                        break;
                }
                view.FindViewById<TextView>(Resource.Id.DateTXT).Text = table.Date.ToString();
                view.FindViewById<TextView>(Resource.Id.LocationTXT).Text = table.Localisation;
                view.FindViewById<TextView>(Resource.Id.WeatherTXT).Text = table.Main;
                view.FindViewById<TextView>(Resource.Id.DescriptionTXT).Text = table.Description;
                view.FindViewById<TextView>(Resource.Id.TempMaxTXT).Text = ((table.Temp_Max + table.Temp_Min) / 2).ToString() + " °C";
                view.FindViewById<TextView>(Resource.Id.HumidityTXT).Text = table.Humidity.ToString() + " %";
                view.FindViewById<TextView>(Resource.Id.CloudTXT).Text = table.Cloudiness.ToString() + " %";

                view.FindViewById<TextView>(Resource.Id.SpeedTXT).Text = table.Speed.ToString() + " m/s";

                SetContentView(view);

            }
        }
    }
}