//Page created when user clicks hamburger menu
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YouSee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        //Lists for all the controls being dynamically added to the page
        List<Button> deleteBtns = new List<Button>();
        List<String> myGroups = new List<String>();
        Dictionary<int, String> userGroups = new Dictionary<int, String>();
        List<MyButton> groupName = new List<MyButton>();
        List<BoxView> groupBoxView = new List<BoxView>();
        List<BoxView> groupSpacer = new List<BoxView>();
        List<BoxView> delRowSpacer = new List<BoxView>();
        List<BoxView> deleteBoxView = new List<BoxView>();
        List<RowDefinition> groupSpacerRow = new List<RowDefinition>();
        List<RowDefinition> groupRow = new List<RowDefinition>();       
        //Want a grid? set this to true... Want a list? Set this to false -- List doesn't line up well and difficult to change colors
        bool gridIsTrueListIsFalse = true;
        public static String prevPage;
        public static int pageCount;


        //public ObservableCollection<string> Items = new ObservableCollection<string>(NetworkUtils.getUserGroups());

        public MenuPage()
        {
            Title = "Menu";
            InitializeComponent();
            btnCreateSm.Clicked += BtnCreateSm_Clicked;
            btnJoinSm.Clicked += btnJoinSm_Clicked;
            getUserGroups();
            setupPage();
            //If using a list view
            MyListView.ItemsSource = new ObservableCollection<string>(myGroups);
            Console.WriteLine(prevPage);
        }

        //I can populate the grid with dictionary values, but I don't know how to get the dictionary to persist while updating values
        
        //Populates the page with the groups the user belongs to
        private void getUserGroups()
        {
            userGroups = NetworkUtils.getUserGroups();
            myGroups = userGroups.Values.ToList();
        }

        private void setupPage()
        {
            if (gridIsTrueListIsFalse) { 
            int groupCol = 2;
            int delBtnCol = 0;

                //Add the groups to the page
                for (int i = 0; i < myGroups.Count; i++)
                {
                    //Define the groupName label properties
                    groupName.Add(new MyButton());
                    groupName[i].Text = myGroups[i];
                    groupName[i].HorizontalOptions = LayoutOptions.FillAndExpand;
                    groupName[i].VerticalOptions = LayoutOptions.Center;
                    groupName[i].BackgroundColor = Color.White;
                    groupName[i].TextColor = Color.Black;
                    groupName[i].Clicked += groupName_Clicked;

                    //Add spacers between groups 
                    groupSpacer.Add(new BoxView());
                    groupSpacer[i].BackgroundColor = Color.SlateGray;

                    //Row to add spacer between group names
                    groupSpacerRow.Add(new RowDefinition());
                    groupSpacerRow[i].Height = 1;

                    //Create a new row definition for new users
                    groupRow.Add(new RowDefinition());
                    groupRow[i].Height = 55;

                    //Define delete button properties
                    deleteBtns.Add(new Button());
                    deleteBtns[i].BackgroundColor = Color.Black;
                    deleteBtns[i].Image = "trashcan.png";
                    deleteBtns[i].Clicked += BtnDelete_Clicked;

                    //Add spacers between delete buttons
                    delRowSpacer.Add(new BoxView());
                    delRowSpacer[i].BackgroundColor = Color.SlateGray;

                    //Box view to set background color for delete icons
                    deleteBoxView.Add(new BoxView());
                    deleteBoxView[i].BackgroundColor = Color.Black;

                    //Increment i by 1 with temp var to add spacer/spacerRow
                    int spacerRowIndex = i + 1;
                    int groupRowIndex = i;

                    //After first iteration, row index MUST add i to correctly add rows, users, and spacers
                    if (i > 0)
                    {
                        spacerRowIndex += i;
                        groupRowIndex += i;
                    }

                    //Add the row definitions for the group names and the spacer bar between groups
                    grdGroups.RowDefinitions.Add(groupRow[i]);
                    grdGroups.RowDefinitions.Add(groupSpacerRow[i]);

                    ////Add everything to the page
                    //grdGroups.Children.Add(groupBoxView[i], groupCol, groupRowIndex);
                    grdGroups.Children.Add(groupName[i], groupCol, groupRowIndex);
                    grdGroups.Children.Add(groupSpacer[i], groupCol, spacerRowIndex);
                    grdGroups.Children.Add(delRowSpacer[i], delBtnCol, spacerRowIndex);
                    grdGroups.Children.Add(deleteBoxView[i], delBtnCol, groupRowIndex);
                    grdGroups.Children.Add(deleteBtns[i], delBtnCol, groupRowIndex);
                }
                //Set the selected group name button color
                if (Application.Current.Properties.ContainsKey("currentGroupID"))
                {
                    try
                    {
                        NetworkUtils.getUserGroups();
                        List<int> groupIds = userGroups.Keys.ToList();
                        int selectedGroup = groupIds.IndexOf((int)Application.Current.Properties["currentGroupID"]);
                        groupName[selectedGroup].BackgroundColor = Color.Red;
                    }
                    catch
                    {
                        Application.Current.Properties.Remove("currentGroupID");
                        Application.Current.Properties.Remove("currentGroup");
                        CreatePage.createHamburgerIcon(new MainPage(), Application.Current.Properties["savedUserName"].ToString());
                    }
                }
            }
            else if (!gridIsTrueListIsFalse)
            {
                //Warning... ListView selectedItem is ugly as shit
                int delBtnCol = 0;
                MyListView.IsVisible = true;

                //Add the groups to the page
                for (int i = 0; i < myGroups.Count; i++)
                {
                    //Create a new row definition for new users
                    groupRow.Add(new RowDefinition());
                    groupRow[i].Height = 55;

                    //Box view to set background color for delete icons
                    deleteBoxView.Add(new BoxView());
                    deleteBoxView[i].BackgroundColor = Color.Black;

                    //Define delete button properties
                    deleteBtns.Add(new Button());
                    deleteBtns[i].BackgroundColor = Color.Black;
                    deleteBtns[i].Image = "trashcan.png";
                    deleteBtns[i].Clicked += BtnDelete_Clicked;

                    //Row to add spacer between group names
                    groupSpacerRow.Add(new RowDefinition());
                    groupSpacerRow[i].Height = 1;

                    //Add everything to the page
                    grdGroups.Children.Add(grdDeleteBtns, 0, 0);
                    grdDeleteBtns.RowDefinitions.Add(groupRow[i]);
                    grdDeleteBtns.Children.Add(deleteBoxView[i], delBtnCol, i);
                    grdDeleteBtns.Children.Add(deleteBtns[i], delBtnCol, i);
                    grdGroups.Children.Add(MyListView, 2, 0);
                }
            }
        }

        //Set the group page to the group selected by the user
        private void groupName_Clicked(object sender, EventArgs e)
        {
            int groupSelectedID = 0;
            var row = (int)((BindableObject)sender).GetValue(Grid.RowProperty);

            for(int i = 0; i < groupName.Count; i++)
            {
                if(groupName[i].BackgroundColor == Color.Red)
                {
                    groupName[i].BackgroundColor = Color.White;
                }
            }
            if(row == 0)
            {
                groupName[row].BackgroundColor = Color.Red;
                groupSelectedID = userGroups.ElementAt(row).Key;
                //Set the current group name in the app properties
                AppProperties.setCurrentGroup(groupName[row].Text);
                AppProperties.setCurrentGroupId(groupSelectedID);
                CreatePage.createHamburgerIcon(new GroupPage(), groupName[row].Text);
                //TODO: Open that group page with the groupName in title bar
            }
            else
            {
                groupName[row / 2].BackgroundColor = Color.Red;
                groupSelectedID = userGroups.ElementAt(row / 2).Key;
                AppProperties.setCurrentGroup(groupName[row / 2].Text);
                AppProperties.setCurrentGroupId(groupSelectedID);
                CreatePage.createHamburgerIcon(new GroupPage(), groupName[row / 2].Text);
                //TODO: Open that group page with the groupName in title bar
            }
        }

        //Remove the user from the DB Group and hide the row for the group that was deleted
        private void BtnDelete_Clicked(object sender, EventArgs e)
        {
            String groupToDelete = null;
            int GroupIdToDelete = 0;

            //Get the selected grid row number
            //https://forums.xamarin.com/discussion/19915/how-to-isentify-the-selected-row-in-a-grid
            var row = (int)((BindableObject)sender).GetValue(Grid.RowProperty);
            if (gridIsTrueListIsFalse)
            {
                //Group name starts at row 0 and then appears on every other row
                if (row == 0)
                {
                    groupToDelete = myGroups[0];
                    GroupIdToDelete = userGroups.ElementAt(0).Key;
                    if((int)Application.Current.Properties["currentGroupID"] == GroupIdToDelete)
                    {
                        if (userGroups.Count > 1)
                        {
                            AppProperties.setCurrentGroupId(userGroups.ElementAt(row + 1).Key);
                            AppProperties.setCurrentGroup(myGroups[row + 1]);
                            grdGroups.RowDefinitions.ElementAt<RowDefinition>(row).Height = 0;
                            NetworkUtils.DeleteUserFromGroup(GroupIdToDelete);
                            NetworkUtils.getUserGroups();
                            NetworkUtils.getUserGroups();
                            CreatePage.createHamburgerIcon(new GroupPage(), Application.Current.Properties["currentGroup"].ToString());
                        }
                    }
                    grdGroups.RowDefinitions.ElementAt<RowDefinition>(row).Height = 0;
                    NetworkUtils.DeleteUserFromGroup(GroupIdToDelete);
                    NetworkUtils.getUserGroups();
                    if(NetworkUtils.groupsDictionary.Count == 0)
                    {
                        //CreatePage.createHamburgerIcon(new MainPage(), Application.Current.Properties["savedUserName"].ToString());
                        var otherPage = new MainPage { Title = Application.Current.Properties["savedUserName"].ToString() };
                        var homePage = App.navigationPage.Navigation.NavigationStack.First();
                        App.navigationPage.Navigation.InsertPageBefore(otherPage, homePage);
                        App.navigationPage.PopToRootAsync(false);
                    }
                }
                else
                {
                    groupToDelete = myGroups[row / 2];
                    GroupIdToDelete = userGroups.ElementAt(row / 2).Key;
                    if ((int)Application.Current.Properties["currentGroupID"] == GroupIdToDelete)
                    {
                        AppProperties.setCurrentGroupId(userGroups.ElementAt(row / 2 - 1).Key);
                        AppProperties.setCurrentGroup(myGroups[row / 2 - 1]);
                        grdGroups.RowDefinitions.ElementAt<RowDefinition>(row).Height = 0;
                        NetworkUtils.DeleteUserFromGroup(GroupIdToDelete);
                        NetworkUtils.getUserGroups();
                        NetworkUtils.getUserGroups();
                        CreatePage.createHamburgerIcon(new GroupPage(), Application.Current.Properties["currentGroup"].ToString());
                    }
                    grdGroups.RowDefinitions.ElementAt<RowDefinition>(row).Height = 0;
                    NetworkUtils.DeleteUserFromGroup(GroupIdToDelete);
                    NetworkUtils.getUserGroups();
                    NetworkUtils.getUserGroups();
                    if (NetworkUtils.groupsDictionary.Count == 0)
                    {
                        var otherPage = new MainPage { Title = Application.Current.Properties["savedUserName"].ToString() };
                        var homePage = App.navigationPage.Navigation.NavigationStack.First();
                        App.navigationPage.Navigation.InsertPageBefore(otherPage, homePage);
                        App.navigationPage.PopToRootAsync(false);
                    }
                }
            }
            else
            {
                groupToDelete = myGroups[row];
                UpdateRow(GroupIdToDelete, row);
            }
        }

        //Used to update the row when looking at a listView
        private void UpdateRow(int GroupIdToDelete, int row)
        {
            //NetworkUtils.DeleteUserFromGroup(groupToDelete);
            grdDeleteBtns.RowDefinitions.ElementAt<RowDefinition>(row).Height = 0;
            MyListView.ItemsSource = null;
            MyListView.ItemsSource = NetworkUtils.getUserGroups();
        }

        //Open page to create new group on click
        private void BtnCreateSm_Clicked(object sender, EventArgs e)
        {
            if(prevPage == null || prevPage == "")
            {
                App.Current.MainPage = new CreatePage();
            }
            else
            {
                App.navigationPage.Navigation.PushAsync(new CreatePage());
            }
        }

        //Open page to join group on click
        private void btnJoinSm_Clicked(object sender, EventArgs e)
        {
            if (prevPage == null || prevPage == "")
            {
                App.Current.MainPage = new JoinPage();
            }
            else
            {
                App.navigationPage.Navigation.PushAsync(new JoinPage());
            }
        }
    }
}
