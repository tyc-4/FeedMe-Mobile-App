using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using FeedMe.Models;
using Newtonsoft.Json;

namespace FeedMe
{
    [Activity(Label = "ResSpecificActivity")]
    public class ResSpecificActivity : Activity
    {
        private ImageView mainImgView;
        private TextView storeNameTextView;
        private ListView productListView;
        private Button viewCartButton;
        private int resId;
        private int userId;
        private HttpClient httpClient = new HttpClient();
        public static List<ProductModel> productResponse;
        public static Dictionary<int, int> orderCart = new Dictionary<int, int>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_resSpecific);

            InitializeViews();
        }

        private async void InitializeViews()
        {
            mainImgView = FindViewById<ImageView>(Resource.Id.mainImgView);
            storeNameTextView = FindViewById<TextView>(Resource.Id.storeNameTextView);
            productListView = FindViewById<ListView>(Resource.Id.productList);
            viewCartButton = FindViewById<Button>(Resource.Id.viewCartButton);

            resId = Intent.GetIntExtra("resId", -1);
            userId = Intent.GetIntExtra("userId", -1);
            var resReq = await httpClient.GetAsync($"http://10.0.2.2:59671/Restaurants/GetSpecResinfo?resId=1");

            var resResponse = JsonConvert.DeserializeObject<ResInfoModel>(await resReq.Content.ReadAsStringAsync());
            var imageBitmap = GetImageBitmapFromUrl(resResponse.Image);

            mainImgView.SetImageBitmap(imageBitmap);
            storeNameTextView.Text = resResponse.StoreName;

            var productReq = await httpClient.GetAsync($"http://10.0.2.2:59671/Restaurants/AllProductByRes?resId={resId}");
            productResponse = JsonConvert.DeserializeObject<List<ProductModel>>(await productReq.Content.ReadAsStringAsync());

            productListView.Adapter = new ProductAdapter(this, productResponse);
            productListView.ItemClick += ProductListView_ItemClick;

            viewCartButton.Click += ViewCartButton_Click;
        }

        private void ViewCartButton_Click(object sender, EventArgs e)
        {
            var viewCartIntent = new Intent(this, typeof(CartActivity));
            viewCartIntent.PutExtra("userId", userId);
            viewCartIntent.PutExtra("resId", resId);
            StartActivity(viewCartIntent);
        }

        private void ProductListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            viewCartButton.Visibility = ViewStates.Visible;
            if (orderCart.ContainsKey(productResponse[e.Position].ID))
            {
                orderCart[productResponse[e.Position].ID] += 1;
            }
            else
            {
                orderCart[productResponse[e.Position].ID] = 1;
            }

            Toast.MakeText(this, $"{productResponse[e.Position].Name} x {orderCart[productResponse[e.Position].ID]} in cart", ToastLength.Short).Show();
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}