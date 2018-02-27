//Page created when user clicks hamburger menu
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YouSee
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
		public MenuPage ()
		{
            Title = "Menu";
            InitializeComponent ();
            NetworkUtils.getUserGroups();
            getUserGroups();
		}

        private void getUserGroups()
        {
            List<String> myGroups = new List<String>();
            myGroups = NetworkUtils.getUserGroups();

            //Add the groups to the page
            for(int i = 0; i < myGroups.Count; i++)
            {
                //User to demonstrate adding multiple users to grid -- foreach user in group do something like this
                Label groupName = new Label();

                //Change text to username
                groupName.Text = myGroups[i];
                groupName.HorizontalOptions = LayoutOptions.Center;
                groupName.VerticalOptions = LayoutOptions.Center;
                groupName.TextColor = Color.Black;

                //Create the delete button
                Button btnDelete = new Button();
                btnDelete.BackgroundColor = Color.Black;
                btnDelete.Image = "trashcan.png";

                //Box view to set background color
                BoxView groupBoxView = new BoxView();
                groupBoxView.BackgroundColor = Color.Red;

                //Box view to make a horizontal line between groupNames
                BoxView groupSpacer = new BoxView();
                groupSpacer.BackgroundColor = Color.White;

                //Box view to make horizontal line between delete buttons
                BoxView delRowSpacer = new BoxView();
                delRowSpacer.BackgroundColor = Color.White;

                //Box view to set background color for delete icons
                BoxView deleteBoxCol = new BoxView();
                deleteBoxCol.BackgroundColor = Color.Black;

                //Row to add spacer between group names
                RowDefinition groupSpacerRow = new RowDefinition();
                groupSpacerRow.Height = 1;

                //Create a new row definition for new users
                RowDefinition groupRow = new RowDefinition();
                groupRow.Height = 55;

                //Increment i by 1 with temp var to add spacer/spacerRow
                int spacerRowIndex = i + 1;
                int groupRowIndex = i;

                //After first iteration, row index MUST add i to correctly add rows, users, and spacers
                if (i > 0)
                {
                    spacerRowIndex += i;
                    groupRowIndex += i;
                }
                ////Foreach user -> Add username, add boxview/user to row i -> make another row to add the spacer before adding the next user
                grdGroups.RowDefinitions.Add(groupRow);
                grdGroups.RowDefinitions.Add(groupSpacerRow);


                //grdGroups.Children.Add(spacerRow, 0, 2, 0, 1);
                //index starts at 3 because row 0 is invite button, row 1 is default user
                grdGroups.Children.Add(groupBoxView, 2, groupRowIndex);
                grdGroups.Children.Add(groupName, 2, groupRowIndex);
                grdGroups.Children.Add(groupSpacer, 2, spacerRowIndex);
                grdGroups.Children.Add(delRowSpacer, 0, spacerRowIndex);
                grdGroups.Children.Add(deleteBoxCol, 0, groupRowIndex);
                grdGroups.Children.Add(btnDelete, 0, groupRowIndex);
            }
        }

	}
}