using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms.Maps;

namespace YouSee
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }
    }
}