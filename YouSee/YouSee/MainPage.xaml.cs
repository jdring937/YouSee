//YT GeoLocation tutorial: https://www.youtube.com/watch?v=pH1WaO-5LDk
//Drawing lines between two points: https://stackoverflow.com/questions/13433648/draw-a-line-between-two-point-on-a-google-map-using-jquery

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;
using System.Diagnostics;

namespace YouSee
{
    public partial class MainPage : ContentPage
    {
        Map map;
        double lat;
        double lng;

        public MainPage()
        {
            InitializeComponent();
            btnCreate.Clicked += BtnGetLocation_Clicked;
            //RetrieveLocation();
            GetLocationOnLoad();

            map = new Map
            {
                //IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            // You can use MapSpan.FromCenterAndRadius 
            //map.MoveToRegion (MapSpan.FromCenterAndRadius (new Position (37, -122), Distance.FromMiles (0.3)));
            // or create a new MapSpan object directly
            map.MoveToRegion(new MapSpan(new Position(0, 0), 360, 360));


            // put the page together
            grdButtonGrid.Children.Add(map, 0, 2);
            Grid.SetColumnSpan(map, 2);

            // for debugging output only
            map.PropertyChanged += (sender, e) => {
                Debug.WriteLine(e.PropertyName + " just changed!");
                if (e.PropertyName == "VisibleRegion" && map.VisibleRegion != null)
                    CalculateBoundingCoordinates(map.VisibleRegion);
            };

            //Create pin for current location
            var pin = new Pin()
            {
                Position = new Position(lat, lng),
                Label = "My Position!"
            };

            //map.Pins.Add(pin);

            //Center map on current location
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, lng), Distance.FromMiles(0.0)));

        }

        /// <summary>
        /// In response to this forum question http://forums.xamarin.com/discussion/22493/maps-visibleregion-bounds
        /// Useful if you need to send the bounds to a web service or otherwise calculate what
        /// pins might need to be drawn inside the currently visible viewport.
        /// </summary>
        static void CalculateBoundingCoordinates(MapSpan region)
        {
            // WARNING: I haven't tested the correctness of this exhaustively!
            var center = region.Center;
            var halfheightDegrees = region.LatitudeDegrees / 2;
            var halfwidthDegrees = region.LongitudeDegrees / 2;

            var left = center.Longitude - halfwidthDegrees;
            var right = center.Longitude + halfwidthDegrees;
            var top = center.Latitude + halfheightDegrees;
            var bottom = center.Latitude - halfheightDegrees;

            // Adjust for Internation Date Line (+/- 180 degrees longitude)
            if (left < -180) left = 180 + (180 + left);
            if (right > 180) right = (right - 180) - 180;
            // I don't wrap around north or south; I don't think the map control allows this anyway

            Debug.WriteLine("Bounding box:");
            Debug.WriteLine("                    " + top);
            Debug.WriteLine("  " + left + "                " + right);
            Debug.WriteLine("                    " + bottom);
        }


        private async void BtnGetLocation_Clicked(object sender, EventArgs e)
        {
        }

<<<<<<< HEAD
        
=======
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

            map.Pins.Add(pin);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, lng), Distance.FromMiles(0.3)));
        }        
>>>>>>> master
    }
}



//buttonBasicMap.Clicked += (_, e) => Navigation.PushAsync(new BasicMapPage());
//            buttonCamera.Clicked += (_, e) => Navigation.PushAsync(new CameraPage());
//            buttonPins.Clicked += (_, e) => Navigation.PushAsync(new PinsPage());
//            buttonShapes.Clicked += (_, e) => Navigation.PushAsync(new ShapesPage());
//            buttonShapes2.Clicked += (_, e) => Navigation.PushAsync(new Shapes2Page());
//            buttonTiles.Clicked += (_, e) => Navigation.PushAsync(new TilesPage());
//            buttonCustomPins.Clicked += (_, e) => Navigation.PushAsync(new CustomPinsPage());
//            buttonShapesWithInitialize.Clicked += (_, e) => Navigation.PushAsync(new ShapesWithInitializePage());
//            buttonBindingPin.Clicked += (_, e) => Navigation.PushAsync(new BindingPinViewPage());
//            buttonGroundOverlays.Clicked += (_, e) => Navigation.PushAsync(new GroundOverlaysPage());
//            buttonMapStyles.Clicked += (_, e) => Navigation.PushAsync(new MapStylePage());