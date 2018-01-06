
using System.IO;
using System.Collections.Generic;
using Android.Util;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;
using WeatherApp.JSON;
using Android.OS;
using System;

namespace WeatherApp.BDD
{
    public class Initialisation
    {
        public SQLiteConnection BDDConnection;
        private ISQLitePlatform sqlitePlatform = new SQLitePlatformAndroidN();

        public void CreateBDD()
        {
            string dbPath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(
            Android.OS.Environment.DirectoryDocuments.ToString()).ToString(), "WeatherApp.sqlite");

            Java.IO.File DB = new Java.IO.File(dbPath);

            DB.Delete();

            DB.CreateNewFile();
        }

        public void DBConnection()
        {
            string dbPath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(
            Android.OS.Environment.DirectoryDocuments.ToString()).ToString(), "WeatherApp.sqlite");

            BDDConnection = new SQLiteConnection(sqlitePlatform, dbPath);
        }

        public void CreateTable()
        {
            BDDConnection.CreateTable<Table>();
            BDDConnection.CreateTable<Localisation>();
        }

        public void Insert(Table p_Table)
        {
            BDDConnection.Insert(p_Table);
        }

        public void InsertList(List<JSON.Weather> p_List)
        {
            try
            {

                List<Table> temp = new List<Table>();
                Table tmp;
                foreach (Weather wth in p_List)
                {
                    tmp = wth;
                    temp.Add(tmp);
                }

                BDDConnection.InsertOrIgnoreAll(temp);
            }
            catch(Exception e)
            {
                Log.Info("BDD Weather APP", "Error when trying to insert the list of weather in the BDD  " + e.ToString());
            }
        }

        public void InsertNewElement( List<JSON.Weather> p_List)
        {
            TableQuery<Table> DateOrder = BDDConnection.Table<Table>().OrderByDescending(t => t.Date);
            DateTime RecentDate = new DateTime();
            if (DateOrder.Count() != 0)
            {
                RecentDate = DateOrder.ElementAt(0).Date;
            }
            int indiceInsert = 0;

            if (DateTime.Compare(RecentDate, DateTime.Parse(p_List[indiceInsert].date.Heure)) < 0)
            {
                ClearTable();

                InsertList(p_List);
            }
            else
            {
                while (DateTime.Compare(RecentDate, DateTime.Parse(p_List[indiceInsert].date.Heure)) > 0)
                {
                    indiceInsert++;
                }

                List<Table> temp = new List<Table>();
                Table tmp;
                foreach (Weather wth in p_List)
                {
                    tmp = wth;
                    temp.Add(tmp);
                }

                for (int i = indiceInsert; i < temp.Count-1; i++)
                {
                try
                {
                    Insert(temp[i]);
                     }
                     catch (Exception e)
                     {
                    Log.Info("BDD Weather APP", "Error when trying to insert the new elements of weather in the BDD  " + e.ToString());
                    }
                }
            }

        }

        public void ClearTable()
        {
            try
            {
                BDDConnection.DeleteAll<Table>();
            }
            catch (Exception e)
            {
                Log.Info("BDD Weather APP", "Error when trying to delete values from the table" + e.ToString());
            }
        }

        public DateTime GetOldest()
        {
            TableQuery<Table> DateOrder = BDDConnection.Table<Table>().OrderBy(t => t.Date);
            int test = DateOrder.Count();
            if (DateOrder.Count() != 0 )
            {
                DateTime OldtDate = DateOrder.ElementAt(0).Date;
                return OldtDate;
            }

            return new DateTime();
        }

        public  Table[] GetTable()
        {
            try
            {
                TableQuery<Table> Weather = BDDConnection.Table<Table>().Where( t =>  t.Date != null) ;

                IEnumerator<Table> enumerateur = Weather.GetEnumerator();
                Table[] ArrayWeather = new Table[Weather.Count()];
                int i = 0; 
                while (enumerateur.MoveNext())
                {
                    if (DateTime.Compare(enumerateur.Current.Date,DateTime.Now) >= 0 )
                    {
                    ArrayWeather[i] = enumerateur.Current;
                    i++;
                    }
                    else{
                    }      
                    
                }

                return ArrayWeather;
            }
            catch
            {
                Log.Info("GET weather" ,"Error when trying to get values from the table");
                throw new Exception();
            }
        }
    }
}