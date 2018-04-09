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
        CustomMap customMap = new CustomMap()
        {
            MapType = MapType.Street,
                WidthRequest = App.ScreenWidth,
                HeightRequest = App.ScreenHeight
        };
        static double lat;
        static double lng;
        bool timerOn = false;

        public MainPage()
        {
            InitializeComponent();
            btnCreate.Clicked += BtnCreate_Clicked;
            btnJoin.Clicked += BtnJoin_Clicked;

            InitTimer();

            MenuPage.prevPage = this.Title;
            MenuPage.pageCount += 1;
        }


        //What happens when the page disappears
        protected override void OnDisappearing()
        {
            //grdButtonGrid.Children.Remove(customMap);
            timerOn = false;
            this.grdButtonGrid.Children.Remove(customMap);
        }

        //What happens when the page appears
        protected override void OnAppearing()
        {
            timerOn = true;
            // create map style buttons
            var street = new Button { Text = "Street", BackgroundColor = Color.Black, TextColor = Color.White };
            var hybrid = new Button { Text = "Hybrid", BackgroundColor = Color.Black, TextColor = Color.White };
            var satellite = new Button { Text = "Satellite", BackgroundColor = Color.Black, TextColor = Color.White };
            street.Clicked += HandleClicked;
            hybrid.Clicked += HandleClicked;
            satellite.Clicked += HandleClicked;

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

            AddPinOnLoad();

            AddPinsToMap();


            InitTimer();

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
            int secondsInterval = 5;
            Device.StartTimer(TimeSpan.FromSeconds(secondsInterval), () =>
            {
                Device.BeginInvokeOnMainThread(() => AddPinsToMap());
                return timerOn;
            });
        }

        //Go to createGroupPage
        private void BtnCreate_Clicked(object sender, EventArgs e)
        {

            //Does't crash when doing this, but back btn also doesn't work correctly and ugly UI
            MenuPage.prevPage = this.Title;
            if (MenuPage.pageCount <= 1)
            {
                App.Current.MainPage = new CreatePage();
            }
            else
            {
                App.navigationPage.Navigation.PushAsync(new CreatePage());
            }

            //Cleaner UI, back button works as expected, but crashes on first group entry
   
        }

        private void BtnJoin_Clicked(object sender, EventArgs e)
        {
            //App.navigationPage.Navigation.PushAsync(new JoinPage());
            MenuPage.prevPage = this.Title;
            if(MenuPage.pageCount <= 1)
            {
                App.Current.MainPage = new JoinPage();
            }
            else
            {
                App.navigationPage.Navigation.PushAsync(new JoinPage());
            }
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
                Label = "My Position!",
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
                Label = "My Position!",
                Id = "myPin",
                Url = "homepages.uc.edu/~ringjy"
            };

            //Add pin to map
            customMap.Pins.Clear();
            customMap.Pins.Add(customPin);

            //Center map on user/pin location
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, lng), Distance.FromMiles(0.1)));

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
