//YT GeoLocation tutorial: https://www.youtube.com/watch?v=pH1WaO-5LDk
//Drawing lines between two points: https://stackoverflow.com/questions/13433648/draw-a-line-between-two-point-on-a-google-map-using-jquery
//MS SQL nuget https://www.nuget.org/packages/System.Data.SqlClient/
//CustomMap/Map Pin https://developer.xamarin.com/guides/xamarin-forms/application-fundamentals/custom-renderer/map/customized-pin/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using System.Data;
using Java.Util;
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;

namespace YouSee
{
    public partial class MainPage : ContentPage
    {
        //Map map;
        CustomMap customMap;
        double lat;
        double lng;

        public MainPage()
        {
            InitializeComponent();
            btnCreate.TextColor = Color.White;
            btnJoin.TextColor = Color.White;
            btnCreate.Clicked += BtnCreate_Clicked;
            GetLocationOnLoad();

            //Retrieves the value of the saved username.. This is for demonstration purposes
            //if (Application.Current.Properties.ContainsKey("savedPropA"))
            //{
            //    String s = Convert.ToString(Application.Current.Properties["savedUserName"]);
            //}

            //Create map object
            //map = new Map
            //{
            //    HeightRequest = 100,
            //    WidthRequest = 960,
            //    VerticalOptions = LayoutOptions.FillAndExpand
            //};

            customMap = new CustomMap
            {
                MapType = MapType.Street,
                WidthRequest = App.ScreenWidth,
                HeightRequest = App.ScreenHeight
            };

            // put the page together
            //grdButtonGrid.Children.Add(map, 0, 2);
            //Grid.SetColumnSpan(map, 2);
            grdButtonGrid.Children.Add(customMap, 0, 2);
            Grid.SetColumnSpan(customMap, 2);
        }

        //Testing purposes only... Change this to actual button click event
        private async void BtnCreate_Clicked(object sender, EventArgs e)
        {
            await RetrieveLocation();
        }

        //Await method to get location when page loads
        private async void GetLocationOnLoad()
        {
            await RetrieveLocation();
        }

        //Get the users location
        private async Task RetrieveLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 20;
            TimeSpan span = new TimeSpan(0, 0, 0, 0, 60000);
            var position = await locator.GetPositionAsync(timeout: span);

            lat = position.Latitude;
            lng = position.Longitude;

            //Create map pin
            var pin = new Pin()
            {
                Position = new Position(lat, lng),
                Label = "My Position!"
            };

            var customPin = new CustomPin
            {
                Type = PinType.Place,
                Position = new Position(lat, lng),
                Label = "My Postition!",               
                Id = "Xamarin",
                Url = "homepages.uc.edu/~ringjy"
            };

            //map.Pins.Add(pin);
            customMap.Pins.Add(customPin);


            // You can use MapSpan.FromCenterAndRadius 
            //map.MoveToRegion (MapSpan.FromCenterAndRadius (new Position (37, -122), Distance.FromMiles (0.3)));
            // or create a new MapSpan object directly

            //map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, lng), Distance.FromMiles(0.1)));
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, lng), Distance.FromMiles(0.1)));

        }//Retrieve Location


        //Create new Random String
        //https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings-in-c
        private static System.Random random = new System.Random();
        public static string RandomString()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[8];
            var random = new System.Random();

            //Create the initial group code
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];               
            }

            var finalString = new String(stringChars);

            //if the group code exists, create a new group code
            while(NetworkUtils.searchDBForRandom(finalString) > 0)
            {
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];

                }
                finalString = new string(stringChars);
            }
            return finalString;
        }//RandomString


        //TODO Implement multithreaded client/server
        //https://www.youtube.com/watch?v=BvRJIYDu7wo -> creates chat

        //xTODO Implement hamburger menu on mainPage
        //https://wolfprogrammer.com/2016/09/02/creating-a-hamburger-menu-in-xamarin-forms/

        //xTODO: Get the userID when the user inserts their username
        //https://stackoverflow.com/questions/5228780/how-to-get-last-inserted-id
    }
}