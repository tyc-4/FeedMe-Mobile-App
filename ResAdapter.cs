using System;
using System.Collections.Generic;

using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FeedMe.Models;

namespace FeedMe
{
    public class ResAdapter : BaseAdapter<ResModel>
    {
        private List<ResModel> mResItems;
        private Context mContext;

        public ResAdapter(List<ResModel> resItems, Context mContext)
        {
            this.mResItems = resItems;
            this.mContext = mContext;
        }

        public override ResModel this[int position]
        {
            get { return mResItems[position]; }
        }

        public override int Count
        {
            get { return mResItems.Count; }
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
            var resName = convertView.FindViewById<TextView>(Resource.Id.resNameGrid);
            var resImage = convertView.FindViewById<ImageView>(Resource.Id.restImageGrid);
            var resDesc = convertView.FindViewById<TextView>(Resource.Id.resDesc);

            var imageBitmap = GetImageBitmapFromUrl(mResItems[position].Image);
            resName.Text = mResItems[position].StoreName;
            resImage.SetImageBitmap(imageBitmap);
            resDesc.Text = mResItems[position].Descriptiom;

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