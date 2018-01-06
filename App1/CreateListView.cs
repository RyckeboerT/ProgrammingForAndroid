using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using WeatherApp.BDD;
using Newtonsoft.Json;
using Plugin.CurrentActivity;

namespace WeatherApp
{
    [Activity]
    public class CreateListView : ListActivity
    {
        BDD.Table[] ListOfWeather;
        private Initialisation BDD ;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            BDD = new Initialisation();
            BDD.DBConnection();
            base.OnCreate(savedInstanceState);
            CrossCurrentActivity.Current.Activity = this;
            BDD.Table[] ListOfWeather = BDD.GetTable();
            List<Table> CleanList = new List<Table>();

            foreach(Table weather in ListOfWeather)
            {
                if (weather != null)
                {
                    CleanList.Add(weather);
                }
            }
            
            ListView.Adapter = new DisplayAdapter(this, CleanList);
            ListView.Clickable = true;
            ListView.DescendantFocusability = DescendantFocusability.BlockDescendants;
            ListView.Focusable = false;
            ListView.ItemClick += View_Click;
            ListView.ItemSelected += ListView_ItemSelected;
            ListView.ItemLongClick += ListView_ItemLongClick;
        }

        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(CreateView));

            ListOfWeather = BDD.GetTable();

            int pos = e.Position;

            string weather = JsonConvert.SerializeObject(ListOfWeather[pos]);

            intent.PutExtra("weather", weather);

            this.StartActivity(intent);
        }

        private void ListView_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Intent intent = new Intent(this, typeof(CreateView));

            ListOfWeather = BDD.GetTable();

            int pos = e.Position;

            string weather = JsonConvert.SerializeObject(ListOfWeather[pos]);

            intent.PutExtra("weather", weather);

            this.StartActivity(intent);
        }

        private void View_Click(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(CreateView));

            ListOfWeather = BDD.GetTable();

            int pos = e.Position;

            string weather = JsonConvert.SerializeObject(ListOfWeather[pos]);

            intent.PutExtra("weather", weather);

            this.StartActivity(intent);
        }

        public override void OnBackPressed()
        {
        }
    }

    internal class DisplayAdapter : BaseAdapter<BDD.Table>
    {
        List<Table> items;
        Activity context;
        public DisplayAdapter(Activity context, List<Table> weath) : base()
        {
            this.context = context;
            this.items = weath;

            if (items.Count() == 0)
            {
                Intent intent = new Intent(context, typeof(NoData));
                context.StartActivity(intent);
            }
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override int Count
        {
            get { return items.Count; }
        }

        public override Table this[int position]
        {
            get { return items[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available

            if (items[position] != null)
            {
                if (view == null) // otherwise create a new one
                    view = context.LayoutInflater.Inflate(Resource.Layout.CustomView, null);
            
                view.FindViewById<TextView>(Resource.Id.Text2).Text = items[position].Description;
                view.FindViewById<TextView>(Resource.Id.Text1).Text = items[position].Main;
                view.FindViewById<TextView>(Resource.Id.Text3).Text = items[position].Date.ToString();
                string Test = items[position].Icon;
                switch (Test)
                {
                    case "01d":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Cleard);
                        break;
                    case "01n":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Clearn);
                        break;
                    case "02d":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Fewd);
                        break;
                    case "02n":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Fewn);
                        break;
                    case "03d":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Scatteredd);
                        break;
                    case "03n":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Scatteredn);
                        break;
                    case "04d":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Brokend);
                        break;
                    case "04n":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Brokenn);
                        break;
                    case "09d":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Showerd);
                        break;
                    case "09n":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Showern);
                        break;
                    case "10d":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Raind);
                        break;
                    case "10n":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Rainn);
                        break;
                    case "11d":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Thunderstormd);
                        break;
                    case "11n":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Thunderstormn);
                        break;
                    case "13d":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Snowd);
                        break;
                    case "13n":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Snown);
                        break;
                    case "50d":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Mistd);
                        break;
                    case "50n":
                        view.FindViewById<ImageView>(Resource.Id.Icon).SetImageResource(Resource.Drawable.Mistn);
                        break;
                }
            }
            else
            {
                if ( position == 0)
                {
                    Intent intent = new Intent(context, typeof(NoData));
                    context.StartActivity(intent);

                }
                return null;
            }
            return view;
        }
    }
}