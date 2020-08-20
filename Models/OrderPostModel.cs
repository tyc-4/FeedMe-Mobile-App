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
    internal class OrderPostModel
    {
        public int ResId { get; set; }
        public int UserId { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}