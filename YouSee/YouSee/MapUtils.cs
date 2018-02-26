using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace YouSee
{
    public class MapUtils
    {
        private static double lat;
        private static double lng;

        //Get the users location
        public static async Task RetrieveLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 20;
            TimeSpan span = new TimeSpan(0, 0, 0, 0, 60000);
            var position = await locator.GetPositionAsync(timeout: span);

            lat = position.Latitude;
            lng = position.Longitude;

        }//Retrieve Location

        public static double getLat() { return lat; }
        public static double getLng() { return lng; }

        public static void setLat(double setLat)
        {
            lat = setLat;
        }

        public static void setLng(double setLng)
        {
            lng = setLng;
        }
    }
}
