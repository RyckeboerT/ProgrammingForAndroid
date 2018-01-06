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
using SQLite.Net.Attributes;

namespace WeatherApp.BDD
{
    public class Localisation
    {
        [PrimaryKey]
        public float Latitude { get; set; }
        public float Longitude { get; set; }

    }
}