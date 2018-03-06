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
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;

namespace YouSee
{
    public partial class MainPage : ContentPage
    {
        CustomMap customMap;
        static double lat;
        static double lng;

        public MainPage()
        {
            InitializeComponent();
            btnCreate.Clicked += BtnCreate_Clicked;
            btnJoin.Clicked += BtnJoin_Clicked;
            AddPinOnLoad();
            AddPinsToMap();
            InitTimer();

            customMap = new CustomMap
            {
                MapType = MapType.Street,
                WidthRequest = App.ScreenWidth,
                HeightRequest = App.ScreenHeight
            };

            // create map style buttons
            var street = new Button { Text = "Street" };
            var hybrid = new Button { Text = "Hybrid" };
            var satellite = new Button { Text = "Satellite" };
            street.Clicked += HandleClicked;
            hybrid.Clicked += HandleClicked;
            satellite.Clicked += HandleClicked;
            street.BackgroundColor = Color.Black;
            hybrid.BackgroundColor = Color.Black;
            satellite.BackgroundColor = Color.Black;
            street.TextColor = Color.White;
            hybrid.TextColor = Color.White;
            satellite.TextColor = Color.White;

            //Put maptype buttons in grid
            var mapTypeGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(3.333, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(3.333, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(3.333, GridUnitType.Star) }
                },
                ColumnSpacing = -5,
                RowSpacing = -5

            };
            mapTypeGrid.Children.Add(street, 0, 0);
            mapTypeGrid.Children.Add(hybrid, 1, 0);
            mapTypeGrid.Children.Add(satellite, 2, 0);

            // put the page together
            grdButtonGrid.Children.Add(customMap, 0, 2);
            Grid.SetColumnSpan(customMap, 2);
            grdButtonGrid.Children.Add(mapTypeGrid, 0, 1);
            Grid.SetColumnSpan(mapTypeGrid, 2);

        }

        private void HandleClicked(object sender, EventArgs e)
        {
            var b = sender as Button;
            switch (b.Text)
            {
                case "Street":
                    customMap.MapType = MapType.Street;
                    break;
                case "Hybrid":
                    customMap.MapType = MapType.Hybrid;
                    break;
                case "Satellite":
                    customMap.MapType = MapType.Satellite;
                    break;
            }
        }

        //Every 5 seconds, retrieve users location
        public void InitTimer()
        {
            int secondsInterval = 3;
            Device.StartTimer(TimeSpan.FromSeconds(secondsInterval), () =>
            {
                Device.BeginInvokeOnMainThread(() => AddPinsToMap());
                return true;
            });
        }

        //Go to createGroupPage
        private void BtnCreate_Clicked(object sender, EventArgs e)
        {
            //App.Current.MainPage = new CreatePage();
            App.navigationPage.Navigation.PushAsync(new CreatePage());
        }

        private void BtnJoin_Clicked(object sender, EventArgs e)
        {
            App.navigationPage.Navigation.PushAsync(new JoinPage());
            //App.navigationPage.Navigation.PushAsync(new ListViewPageJoin());
        }

        //Method that runs every 3 seconds and updates the users pin location - prevents panning to that location every 3s
        private async void AddPinsToMap()
        {
            await MapUtils.RetrieveLocation();
            lat = MapUtils.getLat();
            lng = MapUtils.getLng();

            var customPin = new CustomPin
            {
                Type = PinType.Place,
                Position = new Position(lat, lng),
                Label = "My Postition!",
                Id = "myPin",
                Url = "homepages.uc.edu/~ringjy"
            };

            //Add pin to map
            customMap.Pins.Clear();
            customMap.Pins.Add(customPin);
        }

        //Adds the first pin to the map and pans to that pins location
        private async void AddPinOnLoad()
        {
            await MapUtils.RetrieveLocation();
            lat = MapUtils.getLat();
            lng = MapUtils.getLng();

            var customPin = new CustomPin
            {
                Type = PinType.Place,
                Position = new Position(lat, lng),
                Label = "My Postition!",
                Id = "myPin",
                Url = "homepages.uc.edu/~ringjy"
            };

            //Add pin to map
            customMap.Pins.Clear();
            customMap.Pins.Add(customPin);

            //Center map on user/pin location
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(MapUtils.getLat(), MapUtils.getLng()), Distance.FromMiles(0.1)));

        }

        //Add a page to top of stack with hamburger menu
        //var groupPage = new GroupPage { Title = Application.Current.Properties["savedGroupName"].ToString() };
        //var homePage = App.navigationPage.Navigation.NavigationStack.First();
        //App.navigationPage.Navigation.InsertPageBefore(groupPage, homePage);
        //App.navigationPage.PopToRootAsync(true);

        //xTODO Implement multithreaded client/server
        //https://www.youtube.com/watch?v=BvRJIYDu7wo -> creates chat

        //xTODO Implement hamburger menu on mainPage
        //https://wolfprogrammer.com/2016/09/02/creating-a-hamburger-menu-in-xamarin-forms/

        //xTODO: Get the userID when the user inserts their username
        //https://stackoverflow.com/questions/5228780/how-to-get-last-inserted-id

        //TODO: Make default page the last page the user was on

        //TODO: Retrieve additional group members locations and place their pins on the map

        //TODO: Place additional users usernames in the group page scrollview

        //TODO: Add pins with additional colors, set additional users to pins to different colors

        //TODO: Add click events to change the active group in ham menu7

        //TODO: Fix the delete SP/update method if necessary -- Currently deleting on groupName... Change to delete on GroupID or GroupUSerID

    }
}