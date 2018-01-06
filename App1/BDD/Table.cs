using SQLite.Net.Attributes;
using System;

namespace WeatherApp.BDD
{
    public class Table
    {

        [PrimaryKey]
        public DateTime Date { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public float Temp_Max { get; set; }
        public float Temp_Min { get; set; }
        public int Humidity { get; set; }
        public float Speed { get; set; }
        public int Cloudiness { get; set; }
        public string Localisation { get; set; }

        public static implicit operator Table (JSON.Weather p_Weath)
        {
            Table table = new Table
            {
                Main = p_Weath.mainWeather.Main,
                Description = p_Weath.mainWeather.Description,
                Icon = p_Weath.mainWeather.Icon,
                Temp_Max = p_Weath.temperature.Temp_Max,
                Temp_Min = p_Weath.temperature.Temp_Min,
                Humidity = p_Weath.temperature.Humidity,
                Speed = p_Weath.wind.Speed,
                Cloudiness = p_Weath.cloud.Cloudiness,
                Localisation = p_Weath.location.City,
                Date = DateTime.Parse(p_Weath.date.Heure)
            };
            return table;
        }
    }

    
}