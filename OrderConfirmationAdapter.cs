using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FeedMe.Models;

namespace FeedMe
{
    public class OrderConfirmationAdapter : BaseAdapter<OrderConfirmationModel>
    {
        private List<OrderConfirmationModel> mItems;
        private Context mContext;

        public OrderConfirmationAdapter(List<OrderConfirmationModel> mItems, Context mContext)
        {
            this.mItems = mItems;
            this.mContext = mContext;
        }

        public override OrderConfirmationModel this[int position]
        {
            get { return mItems[position]; }
        }

        public override int Count => mItems.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.From(mContext).Inflate(Resource.Layout.order_confirmation_grid, null, false);
            }

            var foodName = convertView.FindViewById<TextView>(Resource.Id.foodNameQtyGrid);
            var price = convertView.FindViewById<TextView>(Resource.Id.priceGrid);

            foodName.Text = $"{mItems[position].Quantity}x  { mItems[position].FoodName}";
            price.Text = "$" + mItems[position].Total.ToString("0.00");

            return convertView;
        }
    }
}