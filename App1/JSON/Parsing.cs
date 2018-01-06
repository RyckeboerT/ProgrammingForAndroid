using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace WeatherApp.JSON
{
    public class MainWeather 
    {
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        
    }

    public class Temperature 
    {
        public float Temp_Max { get; set; }
        public float Temp_Min { get; set; }
        //Pourcentage
        public int Humidity { get; set; }

        public Temperature(JObject p_Temp)
        {
           this.Temp_Max = p_Temp.Value<float>("temp_max") ;

            this.Temp_Min = p_Temp.Value<float>("temp_min");

            this.Humidity = p_Temp.Value<int>("humidity");
        }
    }

    public class Wind 
    {
        public float Speed { get; set; }

        public Wind(JObject p_Loc)
        {
            this.Speed = p_Loc.Value<float>("speed");
        }
    }

    public class Cloud 
    {
        //Pourcentage
        public int Cloudiness { get; set; }

        public Cloud(JObject p_Loc)
        {
            this.Cloudiness = p_Loc.Value<int>("all");
        }
    }

    public class Location 
    {
        public string City { get; set; }

        public Location(JObject p_Loc)
        {
            this.City = p_Loc.Value<string>("name");
        }
    }
    public class Date
    {
        public string Heure { get; set; }

        public Date(JObject p_Loc)
        {
            this.Heure = p_Loc.Value<string>("dt_txt");
        }
    }


    public class Weather
    {
        public MainWeather mainWeather;
        public Temperature temperature;
        public Cloud cloud;
        public Wind wind;
        public Date date;
        public Location location;
    }

    public class Parsing
    {
        
        public static List<Weather> JsonToWeather(string p_Json)
        {
            JToken Json = JToken.Parse(p_Json);


            JObject RequeteParse = JObject.Parse(p_Json);

            IList<JToken> TemperatureList = RequeteParse["list"].Children().ToList();

            List<Weather> weatherList = new List<Weather>();        
            Location location ;


            JObject First;
            JObject Valeur;
            JToken Element;
            Weather weather;
            if ((First = TemperatureList.First()["main"].Value<JObject>()) != null)
            {
                Temperature TemperatureRes = new Temperature(First);
                weather = new Weather();
                weather.temperature = TemperatureRes;
                weatherList.Add(weather);

                Element = TemperatureList.First();                
                while (Element.Next != null && (Element = Element.Next) != Element.Last){
                    Valeur = Element["main"].Value<JObject>();
                    TemperatureRes = new Temperature(Valeur);
                    weather = new Weather();
                    weather.temperature = TemperatureRes;
                    weatherList.Add(weather);
                }
            }
            
            Element = TemperatureList.First();
            // get JSON result objects into a list
            IList<JToken> MainWeathList = Element["weather"].Children().ToList();

            MainWeather MainWeathRes = null;
            // serialize JSON results into .NET objects
            foreach (JToken MainWeath in MainWeathList)
            {
                // JToken.ToObject is a helper method that uses JsonSerializer internally
                MainWeathRes = new MainWeather();
                MainWeathRes = MainWeath.ToObject<MainWeather>();
            }
            weather = new Weather();
            weather = weatherList[0];
            weather.mainWeather = MainWeathRes;
            weatherList[0] = weather ;
            for (int i = 1; i < weatherList.Count; i++)
            {
                Element = Element.Next;
                // get JSON result objects into a list
                MainWeathList = Element["weather"].Children().ToList();

                MainWeathRes = null;
                // serialize JSON results into .NET objects
                foreach (JToken MainWeath in MainWeathList)
                {
                    // JToken.ToObject is a helper method that uses JsonSerializer internally
                    MainWeathRes = new MainWeather();
                    MainWeathRes = MainWeath.ToObject<MainWeather>();
                }
                weather = new Weather();
                weather = weatherList[i];
                weather.mainWeather = MainWeathRes;
                weatherList[i] = weather;
            }

            First = TemperatureList.First()["wind"].Value<JObject>();
            weatherList[0].wind = new Wind(First);
            Element = TemperatureList.First();
            for (int i = 1; i < weatherList.Count ; i++)
            {
                Element = Element.Next;
                Valeur = Element["wind"].Value<JObject>();
                weatherList[i].wind = new Wind(Valeur);
            }

            First = TemperatureList.First()["clouds"].Value<JObject>();
            weatherList[0].cloud = new Cloud(First);
            Element = TemperatureList.First();
            for (int i = 1; i < weatherList.Count; i++)
            {
                Element = Element.Next;
                Valeur = Element["clouds"].Value<JObject>();
                weatherList[i].cloud = new Cloud(Valeur);
            }

            First = TemperatureList.First().Value<JObject>();
            weatherList[0].date = new Date(First);
            Element = TemperatureList.First();
            for (int i = 1; i < weatherList.Count; i++)
            {
                Element = Element.Next;
                Valeur = Element.Value<JObject>();
                weatherList[i].date = new Date(Valeur);
            }

            location = new Location(RequeteParse["city"].Value<JObject>());

            weatherList.ForEach(t => t.location = location);

            return weatherList;
        }
    }
    
}