using System.Collections.Generic;
using System.Net;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using FeedMe.Models;

namespace FeedMe
{
    public class ProductAdapter : BaseAdapter<ProductModel>
    {
        private Context mContext;
        private List<ProductModel> mProductList;

        public ProductAdapter(Context mContext, List<ProductModel> mProductList)
        {
            this.mContext = mContext;
            this.mProductList = mProductList;
        }

        public override ProductModel this[int position]
        {
            get { return mProductList[position]; }
        }

        public override int Count
        {
            get { return mProductList.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.rest_grid, null, false);
            }
            var prodName = convertView.FindViewById<TextView>(Resource.Id.resNameGrid);
            var prodPhoto = convertView.FindViewById<ImageView>(Resource.Id.restImageGrid);
            var prodPrice = convertView.FindViewById<TextView>(Resource.Id.resDesc);

            var imageBitmap = GetImageBitmapFromUrl(mProductList[position].Image);
            prodName.Text = mProductList[position].Name;
            prodPrice.Text = "$" + mProductList[position].Price.ToString("0.00");
            prodPhoto.SetImageBitmap(imageBitmap);

            return convertView;
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