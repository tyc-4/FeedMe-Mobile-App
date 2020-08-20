﻿using System;
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
    public class ResModel
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string StoreName { get; set; }
        public int Status { get; set; }
        public string Image { get; set; }

        public string Descriptiom { get; set; }
    }
}