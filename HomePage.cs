using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FeedMe.Models;
using Newtonsoft.Json;

namespace FeedMe
{
    [Activity(Label = "HomePage")]
    public class HomePage : Activity
    {
        private TextView welcomeText;
        private ListView resList;

        private HttpClient httpClient = new HttpClient();
        private List<ResModel> resResponse;
        private int userId;

        protected async override void OnCreate(Bundle savedInstanceState)

        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.main_page_layout);
            InitializeViews();
        }

        private async void InitializeViews()
        {
            userId = Intent.GetIntExtra("userid", -1);
            var nameReq = await httpClient.GetAsync($"http://10.0.2.2:59671/Users/GetName?userId={userId}");

            welcomeText = FindViewById<TextView>(Resource.Id.welcomeText);
            welcomeText.Text = $"Welcome, {await nameReq.Content.ReadAsStringAsync()}";

            resList = FindViewById<ListView>(Resource.Id.resView);

            var resRequest = await httpClient.GetAsync("http://10.0.2.2:59671/Restaurants/AllResOpen");

            resResponse = JsonConvert.DeserializeObject<List<ResModel>>(await resRequest.Content.ReadAsStringAsync());

            resList.Adapter = new ResAdapter(resResponse, this);

            resList.ItemClick += ResList_ItemClick;
        }

        private void ResList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var goSpecRes = new Intent(this, typeof(ResSpecificActivity));
            goSpecRes.PutExtra("resId", resResponse[e.Position].ID);
            goSpecRes.PutExtra("userId", userId);
            StartActivity(goSpecRes);
        }
    }
}