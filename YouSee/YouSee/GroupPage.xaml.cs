using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using System.Data.SqlClient;
namespace YouSee
{
	public partial class GroupPage : ContentPage
	{
        CustomMap customMap;
        double lat;
        double lng;
        public static String groupName;
		public GroupPage ()
		{
			InitializeComponent ();
            btnInvite.Clicked += BtnInvite_Clicked;
            AddPinsToMapStart();
            
            InitTimer();
            groupName = Application.Current.Properties["currentGroup"].ToString();

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
            grdMapGrid.Children.Add(customMap, 0, 1);
            Grid.SetColumnSpan(customMap, 2);
            grdMapGrid.Children.Add(mapTypeGrid, 0, 0);
            Grid.SetColumnSpan(mapTypeGrid, 2);

            //Create a label for the current users username
            Label defaultUser = new Label();
            defaultUser.Text = Application.Current.Properties["savedUserName"].ToString();
            defaultUser.HorizontalOptions = LayoutOptions.Center;
            defaultUser.VerticalOptions = LayoutOptions.Center;
            defaultUser.TextColor = Color.White;
            defaultUser.BackgroundColor = Color.Red;

            //Create the horizontal bar to seperate default user from additional members
            BoxView boxView = new BoxView();
            BoxView boxSpace = new BoxView();
            boxSpace.HeightRequest = 1;
            RowDefinition rowSpace = new RowDefinition();
            rowSpace.Height = 1;
            boxView.BackgroundColor = Color.Red;

            //Put the page together
            grdMembersGrid.RowDefinitions.Add(rowSpace);
            grdMapGrid.Children.Add(customMap);
            grdMembersGrid.Children.Add(boxView, 0, 1);
            grdMembersGrid.Children.Add(defaultUser, 0, 1);
            grdMembersGrid.Children.Add(boxSpace, 0, 2);

            //Add usernames to page dynamically -- Change i < " " to count of users
            //Can move this another method or class later
            for (int i = 0; i < 10; i++)
            {
                //User to demonstrate adding multiple users to grid -- foreach user in group do something like this
                Label testUser = new Label();

                //Change text to username
                testUser.Text = "user" + i.ToString();
                testUser.HorizontalOptions = LayoutOptions.Center;
                testUser.VerticalOptions = LayoutOptions.Center;
                testUser.TextColor = Color.Black;

                //Box view to set background color
                BoxView boxView2 = new BoxView();
                boxView2.BackgroundColor = Color.White;

                //Box view to make a horizontal line between users
                BoxView spacer = new BoxView();
                spacer.BackgroundColor = Color.Black;
                spacer.HeightRequest = 5;

                //Row to add spacer to
                RowDefinition spacerRow = new RowDefinition();
                spacerRow.Height = 1;

                //Create a new row definition for new users
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = 55;

                //Increment i by 1 with temp var to add spacer/spacerRow
                   int spacerRowIndex = i + 4;
                   int userRowIndex = i + 3;

                //After first iteration, row index MUST add i to correctly add rows, users, and spacers
                if(i > 0)
                {
                    spacerRowIndex += i;
                    userRowIndex += i;
                }
                //Foreach user -> Add username, add boxview/user to row i -> make another row to add the spacer before adding the next user
                grdMembersGrid.RowDefinitions.Add(rowDefinition);
                grdMembersGrid.RowDefinitions.Add(spacerRow);
                //index starts at 3 because row 0 is invite button, row 1 is default user
                grdMembersGrid.Children.Add(boxView2, 0, userRowIndex);
                grdMembersGrid.Children.Add(testUser, 0, userRowIndex);
                grdMembersGrid.Children.Add(spacer, 0, spacerRowIndex);
            }
        }

        //Change the map type
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

        private void BtnInvite_Clicked(object sender, EventArgs e)
        {
            App.navigationPage.Navigation.PushAsync(new InvitePage());
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

        //Add pins to map
        private async void AddPinsToMap()
        {

            //arrays that we will use to store all the users in the groups and their information


            List<String> userName = new List<String>();
            List<double> userLat = new List<double>();
            List<double> userLng = new List<double>();
            List<string> userData = new List<string>(NetworkUtils.groupLocations());

            foreach (string data in userData)
            {

                try
                {
                    string[] temp = data.Split(',');
                    userName.Add(temp[0].Trim());
                    userLat.Add(Convert.ToDouble(temp[1]));
                    userLng.Add(Convert.ToDouble(temp[2]));
                }
                catch
                {
                    continue;
                }
            }



            await MapUtils.RetrieveLocation();
            lat = MapUtils.getLat();
            lng = MapUtils.getLng();
            int userid = AppProperties.getSaveduserID();
            //string groupname = AppProperties.getSavedGroupName();
            //string name = Application.Current.Properties["saveUserName"].ToString();


            List<string> dataList = new List<string>();
            string query = "update tUsers set userLat = @lat, userLng = @lng where userID = @username";
            string connectionString = @"Server=youseedatabase.cxj5odskcws0.us-east-2.rds.amazonaws.com,1433;DataBase=yousee;User ID=youseeDatabase; Password=yousee18";
            string conString = connectionString;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();
                // SqlCommand should be created inside using.
                // ... It receives the SQL statement.
                // ... It receives the connection object.
                // ... The SQL text works with a specific database.
                //
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@lat", Convert.ToDouble(lat));
                    command.Parameters.AddWithValue("@lng", Convert.ToDouble(lng));
                    command.Parameters.AddWithValue("@username", Convert.ToInt32(userid));
                    command.ExecuteNonQuery();
                }
            }
            customMap.Pins.Clear();

            //creates a list of pins that will store the pins for all the users grabbed from the database
            List<CustomPin> friendPins = new List<CustomPin>();
            //for each pin in the pin list we add it to the map
            for (int i = 0; i < userName.Count; i++)
            {
                var pin2 = new CustomPin()
                {
                    Type = PinType.Place,
                    Position = new Position(userLat[i], userLng[i]),
                    Label = userName[i],
                    Id = userName[i],
                    Url = "www.google.com",
                };
                //adds to the friendpins list that will be called later
                friendPins.Add(pin2);
            }
            //adds the users current location to the map                
            foreach (CustomPin p in friendPins)
            {
                customMap.Pins.Add(p);
            }

            var customPin = new CustomPin
            {
                Type = PinType.Place,
                Position = new Position(lat, lng),
                Label = "My Postition!",
                Id = "myPin",
                Url = "homepages.uc.edu/~ringjy",


            };

            customMap.Pins.Add(customPin);
        }

        //On startup the map will drop a pin on your current location and zoom in to that pin.
        private async void AddPinsToMapStart()
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

    }
}