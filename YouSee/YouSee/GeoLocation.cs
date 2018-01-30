using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Text;

namespace YouSee
{
    class GeoLocation
    {
        //private varible for latitude
        public double Latitude {
            get
            {
                return Latitude;
            }
            set
            {
                Latitude = value;
            }
        }

        //Private varible for longitude
        public double Longitude {
            get
            {
                return Longitude;
            }
            set
            {
                Longitude = value;
            }
        }


        //method to retrive Latitude from Current location
        private async void RetrieveLatitude()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;
            TimeSpan span = new TimeSpan(0, 0, 0, 0, 10000);
            var position = await locator.GetPositionAsync(timeout: span);

            Latitude = position.Latitude;
        }

        //method to retrive Longitude from Current location
        private async void RetrieveLongitude()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;
            TimeSpan span = new TimeSpan(0, 0, 0, 0, 10000);
            var position = await locator.GetPositionAsync(timeout: span);

            Longitude = position.Longitude;
        }

    }//end class

} //end namespace
