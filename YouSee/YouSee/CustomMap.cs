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
using Xamarin.Forms.Maps;

namespace YouSee
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }
    }
}