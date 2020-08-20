using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using FeedMe.Models;
using Org.Apache.Http.Protocol;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using System.Reflection.Emit;

namespace FeedMe
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private EditText usernameEditText;
        private EditText passwordEditText;
        private Button loginBtn;
        private Button registerBtn;
        private Button orderBtn;
        private HttpClient httpClient = new HttpClient();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            InitializeViews();
        }

        private void InitializeViews()
        {
            usernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
            passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            loginBtn = FindViewById<Button>(Resource.Id.loginButton);
            registerBtn = FindViewById<Button>(Resource.Id.registerButton);
            orderBtn = FindViewById<Button>(Resource.Id.orderItemButton);

            loginBtn.Click += LoginBtn_Click;
            registerBtn.Click += RegisterBtn_Click;
            orderBtn.Click += OrderBtn_Click;
            usernameEditText.Text = "yichen";
            passwordEditText.Text = "password1";
        }

        private async void OrderBtn_Click(object sender, EventArgs e)
        {
            var orderItemList = new List<OrderItem>();
            orderItemList.Add(new OrderItem { ItemFK = 1, Notes = "Add Chilli", Quantity = 1 });
            orderItemList.Add(new OrderItem { ItemFK = 4, Notes = "Add Rice", Quantity = 2 });

            var postOrder = new OrderPostModel
            {
                ResId = 1,
                UserId = 1,
                OrderItems = orderItemList
            };

            var postOrderContent = new StringContent(JsonConvert.SerializeObject(postOrder), Encoding.UTF8, "application/json");

            var postOrderRequest = await httpClient.PostAsync("http://10.0.2.2:59671/Orders/PlaceOrder", postOrderContent);
        }

        private async void LoginBtn_Click(object sender, EventArgs e)
        {
            var loginRequest = await httpClient.GetAsync($"http://10.0.2.2:59671/Users/Login?username={usernameEditText.Text}&password={passwordEditText.Text}");

            if (loginRequest.IsSuccessStatusCode)
            {
                var user = JsonConvert.DeserializeObject<UserModel>(await loginRequest.Content.ReadAsStringAsync());
                Toast.MakeText(this, $"Logged in, welcome {user.FirstName}!", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(HomePage));
                intent.PutExtra("userid", user.ID);
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Username or password is wrong!", ToastLength.Short).Show();
            }
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            var registerIntent = new Intent(this, typeof(RegisterActivitycs));
            StartActivity(registerIntent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}