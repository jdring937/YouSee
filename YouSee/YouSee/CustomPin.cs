using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms.Maps;

namespace YouSee
{
    public class CustomPin : Pin
    {
        public string Id { get; set; }
        public string Url { get; set; }
    }
}