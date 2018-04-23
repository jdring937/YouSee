using Android;
using Android.Locations;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace YouSee
{
	public partial class GroupPage : ContentPage
	{
        CustomMap customMap;
        double lat;
        double lng;
        public static String groupName;
        bool timerOn = false;
        List<String> usersInGroup = NetworkUtils.getUsers();
        List<String> usersInGrid = new List<string>();
        //List<String> differenceInUsers = new List<String>();
        int countOfUsers;
        int countOfUsersInGrid;
        User myUsers;
        List<int> userIDs = new List<int>();
        List<double> userLats = new List<double>();
        List<double> userLngs = new List<double>();
        public GroupPage ()
		{
			InitializeComponent ();
            groupName = Application.Current.Properties["currentGroup"].ToString();
            MenuPage.prevPage = groupName;
            setupPage();
            countOfUsers = usersInGroup.Count;

            //for(int i = 0; i < usersInGroup.Count; i++)
            //{
            //    //Every user in the list now has associated properties
            //    userIDs.Add(i);
            //    //Index out of range exception when joining a group
            //    userLats.Add(NetworkUtils.userLats[i]);
            //    userLngs.Add(NetworkUtils.userLngs[i]);
            //}
            //myUsers= new User { userNames = new ObservableCollection<string>(usersInGroup), userID = userIDs, userLats = userLats, userLngs = userLngs };

        }

        //Add the group member pins to map
        private void updateGroupPins()
        {
            List<String> userNames = NetworkUtils.getUsers();
            for (int i = 0; i < NetworkUtils.userLats.Count; i++)
            {
                //Create map pin
                var customPin = new CustomPin
                {
                    Type = PinType.Place,
                    Position = new Position(NetworkUtils.userLats[i], NetworkUtils.userLngs[i]),
                    Label = userNames[i],
                };
                customMap.Pins.Add(customPin);
            }
        }

        //Set up the page
        private void setupPage()
        {
            timerOn = true;

            customMap = new CustomMap
            {
                MapType = MapType.Street,
                WidthRequest = App.ScreenWidth,
                HeightRequest = App.ScreenHeight
            };

            // create map type buttons
            var street = new Button { Text = "Street", BackgroundColor = Color.Black, TextColor = Color.White };
            var hybrid = new Button { Text = "Hybrid", BackgroundColor = Color.Black, TextColor = Color.White };
            var satellite = new Button { Text = "Satellite", BackgroundColor = Color.Black, TextColor = Color.White };
            street.Clicked += HandleClicked;
            hybrid.Clicked += HandleClicked;
            satellite.Clicked += HandleClicked;

            //Put map type buttons in a grid
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

            // put the map grid together
            grdMapGrid.Children.Add(mapTypeGrid, 0, 0);
            Grid.SetColumnSpan(mapTypeGrid, 2);
            grdMapGrid.Children.Add(customMap, 0, 1);
            Grid.SetColumnSpan(customMap, 2);

            //Create the horizontal bar to seperate default user from additional members
            //RowDefinition rowSpace = new RowDefinition();
            //rowSpace.Height = 1;

            //Put the members grid together
            //grdMembersGrid.RowDefinitions.Add(rowSpace);
            grdMapGrid.Children.Add(customMap);

            showGroupMembersInGrid();
            AddPinOnLoad();
            InitTimer();
            CheckForGroupUpdates();
        }

        private void showGroupMembersInGrid()
        {

            grdMembersGrid.Children.Clear();
            grdMembersGrid.RowDefinitions.Clear();
            Button btnInvite = new Button();
            btnInvite.Text = "Invite to Group";
            btnInvite.BackgroundColor = Color.White;
            btnInvite.Clicked += BtnInvite_Clicked;
            RowDefinition btnInviteRow = new RowDefinition();
            grdMembersGrid.RowDefinitions.Add(btnInviteRow);
            grdMembersGrid.Children.Add(btnInvite, 0, 0);

            //Create a label for the current users username
            MyButton defaultUser = new MyButton();
            defaultUser.Text = Application.Current.Properties["savedUserName"].ToString();
            defaultUser.HorizontalOptions = LayoutOptions.FillAndExpand;
            defaultUser.VerticalOptions = LayoutOptions.FillAndExpand;
            defaultUser.TextColor = Color.White;
            defaultUser.BackgroundColor = Color.Red;
            defaultUser.Image = "pin.png";
            defaultUser.Clicked += DefaultUser_Clicked;
            RowDefinition defaultUserRow = new RowDefinition();
            RowDefinition rowSpacer = new RowDefinition();
            rowSpacer.Height = 1;
            BoxView boxView = new BoxView();
            BoxView boxSpace = new BoxView();
            boxView.BackgroundColor = Color.Red;
            boxSpace.HeightRequest = 1;
            boxSpace.Color = Color.Black;
            grdMembersGrid.RowDefinitions.Add(defaultUserRow);
            grdMembersGrid.RowDefinitions.Add(rowSpacer);
            grdMembersGrid.Children.Add(boxView, 0, 1);
            grdMembersGrid.Children.Add(defaultUser, 0, 1);
            grdMembersGrid.Children.Add(boxSpace, 0, 2);
            int caseSwitch = 0;

            for (int i = 0; i < usersInGroup.Count; i++)
            {
                usersInGrid.Add(usersInGroup[i]);
                //User to demonstrate adding multiple users to grid -- foreach user in group do something like this
                MyButton groupMembers = new MyButton();

                //Change text to username
                groupMembers.Text = usersInGroup[i];
                groupMembers.HorizontalOptions = LayoutOptions.FillAndExpand;
                groupMembers.VerticalOptions = LayoutOptions.Center;
                groupMembers.TextColor = Color.Black;
                groupMembers.BackgroundColor = Color.White;
                groupMembers.Clicked += GroupMembers_Clicked;

                //Box view to set background color
                BoxView membersBox = new BoxView();
                //Switch background color for each user
                switch (caseSwitch)
                {
                    case 0:
                        membersBox.BackgroundColor = Color.FromHex("e67e22");
                        groupMembers.Image = "pin" + 0;
                        caseSwitch++;
                        break;
                    case 1:
                        membersBox.BackgroundColor = Color.FromHex("2ecc71");
                        groupMembers.Image = "pin" + 1;
                        caseSwitch++;
                        break;
                    case 2:
                        membersBox.BackgroundColor = Color.FromHex("f1c40f");
                        groupMembers.Image = "pin" + 2;
                        caseSwitch++;
                        break;
                    case 3:
                        membersBox.BackgroundColor = Color.FromHex("3498db");
                        groupMembers.Image = "pin" + 3;
                        caseSwitch++;
                        break;
                    case 4:
                        membersBox.BackgroundColor = Color.FromHex("9b59b6");
                        groupMembers.Image = "pin" + 4;
                        caseSwitch++;
                        break;
                    case 5:
                        membersBox.BackgroundColor = Color.FromHex("79F648");
                        groupMembers.Image = "pin" + 5;
                        caseSwitch++;
                        break;
                    case 6:
                        membersBox.BackgroundColor = Color.FromHex("36F3FF");
                        groupMembers.Image = "pin" + 6;
                        caseSwitch = 0;
                        break;
                }

                //Box view to make a horizontal line between users
                BoxView spacer = new BoxView();
                spacer.BackgroundColor = Color.Black;
                spacer.HeightRequest = 5;

                //Row to add spacer to
                RowDefinition spacerRow = new RowDefinition();
                spacerRow.Height = 1;

                //Create a new row definition for new users
                RowDefinition userNameRow = new RowDefinition();
                userNameRow.Height = 55;

                //Increment i by 1 with temp var to add spacer/spacerRow
                int spacerRowIndex = i + 4;
                int userRowIndex = i + 3;

                //After first iteration, row index MUST add i to correctly add rows, users, and spacers
                if (i > 0)
                {
                    spacerRowIndex += i;
                    userRowIndex += i;
                }
                //Foreach user -> Add username, add boxview/user to row i -> make another row to add the spacer before adding the next user
                grdMembersGrid.RowDefinitions.Add(userNameRow);
                grdMembersGrid.RowDefinitions.Add(spacerRow);
                //index starts at 3 because row 0 is invite button, row 1 is default user
                grdMembersGrid.Children.Add(membersBox, 0, userRowIndex);
                grdMembersGrid.Children.Add(groupMembers, 0, userRowIndex);
                grdMembersGrid.Children.Add(spacer, 0, spacerRowIndex);

                countOfUsersInGrid = 0;
                countOfUsersInGrid = grdMembersGrid.RowDefinitions.Count / 2 - 1;
            }
        }
        
        //Pan to users current location on click
        private void DefaultUser_Clicked(object sender, EventArgs e)
        {
            try
            {
                customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, lng), customMap.VisibleRegion.Radius));
            }
            catch
            {
                Console.WriteLine("Broke for no reason");
            }
        }

        //Pan to the location of the clicked group member
        private void GroupMembers_Clicked(object sender, EventArgs e)
        {
            var row = (int)((BindableObject)sender).GetValue(Grid.RowProperty);
            int startingRow = 3;
            double memberLat;
            double memberLng;
            if (row == startingRow)
            {
                memberLat = NetworkUtils.userLats[0];
                memberLng = NetworkUtils.userLngs[0];
                try
                {
                    customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(memberLat, memberLng), customMap.VisibleRegion.Radius));
                }
                catch
                {
                    Console.WriteLine("Broke for no reason");
                }
            }
            else
            {               
                for(int i = 1; i < usersInGroup.Count; i++)
                {
                    startingRow++;
                    if(row == i + startingRow)
                    {
                        int index = row - startingRow;
                        memberLat = NetworkUtils.userLats[index];
                        memberLng = NetworkUtils.userLngs[index];
                        try
                        {
                            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(memberLat, memberLng), customMap.VisibleRegion.Radius));
                        }
                        catch
                        {
                            Console.WriteLine("Why does Xamarin hate me");
                        }
                    }
                }
            }
        }

        //What to do when the page re-appears
        protected override void OnAppearing()
        {
            timerOn = true;
            AddPinOnLoad();
            InitTimer();
            CheckForGroupUpdates();
        }

        //What to do when the page dissapears
        protected override void OnDisappearing()
        {
            timerOn = false;
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
                double secondsInterval = 5;
                Device.StartTimer(TimeSpan.FromSeconds(secondsInterval), () =>
                {
                    if (timerOn == true)
                    {
                        Device.BeginInvokeOnMainThread(() => AddPinsToMap());                   
                    }
                    return timerOn;
                });
        }

        private void CheckForGroupUpdates()
        {
            int secondsInterval = 5;
            Device.StartTimer(TimeSpan.FromSeconds(secondsInterval), () =>
            {
                Device.BeginInvokeOnMainThread(() => usersInGroup = NetworkUtils.getUsers());
                countOfUsers = usersInGroup.Count;
                if (countOfUsers != countOfUsersInGrid)
                {
                    if (countOfUsers > countOfUsersInGrid)
                    {
                        //When a user joins
                        IEnumerable<string> differenceQuery = usersInGroup.Except(usersInGrid);
                        foreach (string s in differenceQuery)
                        DisplayAlert("", s + " has joined the group!", "Ok");
                    }
                    if (countOfUsersInGrid > countOfUsers)
                    {
                        //When a user leaves
                        IEnumerable<string> differenceQuery = usersInGrid.Except(usersInGroup);
                        foreach (string s in differenceQuery)
                            DisplayAlert("", s + " has left the group", "Ok");
                    }
                    showGroupMembersInGrid();
                }

                return timerOn;
            });
        }

        //Add pins to map
        private async void AddPinsToMap()
        {
            await MapUtils.RetrieveLocation();
            lat = MapUtils.getLat();
            lng = MapUtils.getLng();

            //Create map pin
            var customPin = new CustomPin
            {
                Type = PinType.Place,
                Position = new Position(lat, lng),
                Label = "My Position!",
                Id = "Xamarin",
                Url = "homepages.uc.edu/~ringjy"
            };

            //Add pin to map
            customMap.Pins.Clear();
            customMap.Pins.Add(customPin);
            updateGroupPins();

            NetworkUtils.updateCoords(lat, lng);
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
            updateGroupPins();
            NetworkUtils.updateCoords(lat, lng);

            //Center map on user/pin location
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, lng), Distance.FromMiles(0.1)));

        }

    }
}