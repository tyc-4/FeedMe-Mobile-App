using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FeedMe.Models
{
    public class OrderConfirmationModel
    {
        public OrderConfirmationModel(string foodName, decimal total, int quantity)
        {
            FoodName = foodName;
            Total = total;
            Quantity = quantity;
        }

        public string FoodName { get; set; }
        public decimal Total { get; set; }
        public int Quantity { get; set; }
    }
}