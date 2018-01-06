using System;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using System.Net;
using System.IO;
using WeatherApp.BDD;

namespace WeatherApp
{
    [Service(Label = "Request")]
    public class APICall : Service
    {
        private RequestBinder binder;

        public override IBinder OnBind(Intent intent)
        {
            binder = new RequestBinder(this);
            return binder;
        }

        // Returns JSON string
        static string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }

        public string GetJson()
        {
            string Test = null ;
            Localisation location = ServiceGeolocalisation.GetLocation() ;
            if (location != null)
            {
                string latitude = Convert.ToInt32(location.Longitude).ToString();
                string longitude = Convert.ToInt32(location.Latitude).ToString();
                string url = "http://api.openweathermap.org/data/2.5/forecast?lat=" + longitude + "&lon=" + latitude + "&units=metric&APPID=b15c4f9c4f1e0ac3f382a9f3f31f814f";
                try
                {
                    Test = GET(url);
                }
                catch (Exception e)
                {
                    Log.Info("Request", e.ToString());
                }
            }            

            return Test;
        }

    }

    public class RequestBinder : Binder
    {
        APICall service;

        public RequestBinder(APICall service)
        {
            this.service = service;
        }

        public APICall GetService()
        {
            return service;
        }
    }
}