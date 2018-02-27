using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		public GroupPage ()
		{
			InitializeComponent ();
            btnInvite.Clicked += BtnInvite_Clicked;
            AddPinsToMap();
            InitTimer();

            customMap = new CustomMap
            {
                MapType = MapType.Street,
                WidthRequest = App.ScreenWidth,
                HeightRequest = App.ScreenHeight
            };

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

        private void BtnInvite_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new InvitePage();
        }

        //Every 5 seconds, retrieve users location
        public void InitTimer()
        {
            int secondsInterval = 5;
            Device.StartTimer(TimeSpan.FromSeconds(secondsInterval), () =>
            {
                Device.BeginInvokeOnMainThread(() => AddPinsToMap());
                return true;
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
                Label = "My Postition!",
                Id = "Xamarin",
                Url = "homepages.uc.edu/~ringjy"
            };

            //Add pin to map
            customMap.Pins.Clear();
            customMap.Pins.Add(customPin);

            //Move map to user/pin location
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, lng), Distance.FromMiles(0.1)));
        }

    }
}