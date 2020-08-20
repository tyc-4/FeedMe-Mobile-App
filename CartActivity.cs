using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;

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
    [Activity(Label = "CartActivity")]
    public class CartActivity : Activity
    {
        private ListView orderConfirmationListView;
        private int userId;
        private int resId;
        private TextView resNameTextView;
        private TextView userNameTextView;
        private TextView totalTextView;
        private Button orderConfirmButton;
        private HttpClient httpClient = new HttpClient();
        private List<OrderItem> orderItemList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.cart_layout);

            // Create your application here
            userId = Intent.GetIntExtra("userId", -1);
            resId = Intent.GetIntExtra("resId", -1);
            InitializeViews();
        }

        private async void InitializeViews()
        {
            resNameTextView = FindViewById<TextView>(Resource.Id.resNameTextView);
            userNameTextView = FindViewById<TextView>(Resource.Id.userNameTextView);
            totalTextView = FindViewById<TextView>(Resource.Id.totalTextView);
            orderConfirmButton = FindViewById<Button>(Resource.Id.orderConfirmButton);

            var resName = await httpClient.GetAsync($"http://10.0.2.2:59671/Restaurants/GetResNameById?resId={resId}");
            var userName = await httpClient.GetAsync($"http://10.0.2.2:59671/Users/GetName?userId={userId}");

            resNameTextView.Text = await resName.Content.ReadAsStringAsync();
            userNameTextView.Text = await userName.Content.ReadAsStringAsync();

            orderItemList = new List<OrderItem>();
            var orderConfirmList = new List<OrderConfirmationModel>();
            var orderItems = ResSpecificActivity.orderCart;
            var products = ResSpecificActivity.productResponse;
            foreach (var item in orderItems)
            {
                var order = products.Where(x => x.ID == item.Key).First();
                orderItemList.Add(new OrderItem { ItemFK = order.ID, Notes = string.Empty, Quantity = item.Value });
                orderConfirmList.Add(new OrderConfirmationModel(order.Name, (item.Value) * (decimal)order.Price, item.Value));
            }

            orderConfirmationListView = FindViewById<ListView>(Resource.Id.orderConfirmListView);
            orderConfirmationListView.Adapter = new OrderConfirmationAdapter(orderConfirmList, this);

            decimal totalprice = 0;
            foreach (var item in orderConfirmList)
            {
                totalprice += item.Total;
            }
            totalTextView.Text = $"Total: ${totalprice.ToString("0.00")}";

            orderConfirmButton.Click += OrderConfirmButton_Click;
        }

        private async void OrderConfirmButton_Click(object sender, EventArgs e)
        {
            var postOrder = new OrderPostModel
            {
                ResId = 1,
                UserId = 1,
                OrderItems = orderItemList
            };

            var postOrderContent = new StringContent(JsonConvert.SerializeObject(postOrder), Encoding.UTF8, "application/json");

            var postOrderRequest = await httpClient.PostAsync("http://10.0.2.2:59671/Orders/PlaceOrder", postOrderContent);
            var orderno = postOrderRequest.Content;
        }
    }
}