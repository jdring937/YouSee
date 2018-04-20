using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YouSee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePage : ContentPage
    {
        public CreatePage()
        {
            InitializeComponent();
            btnCreateGroup.Clicked += BtnCreateGroup_Clicked;
            btnBack.Clicked += BtnBack_Clicked;
            Dictionary<int, String> userGroups = NetworkUtils.getUserGroups();
            if (MenuPage.pageCount <= 1 && userGroups.Count < 1)
            {
                btnBack.IsVisible = true;
            }
            else
            {
                btnBack.IsVisible = false;
            }
        }

        //
        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            CreatePage.createHamburgerIcon(new MainPage(), Application.Current.Properties["savedUserName"].ToString());
        }

        //When user clicks create group run spInsertGroup on DB
        private void BtnCreateGroup_Clicked(object sender, EventArgs e)
        {
            String groupName = txtGroupName.Text;
            Dictionary<int, String> tempGroupDict = NetworkUtils.getUserGroups();
            //List<String> tempGroupList = NetworkUtils.getUserGroups();
            if (tempGroupDict.Values.Contains(groupName))
            {
                lblError.Text = "You are already a member of a group with that name.";
                lblError.IsVisible = true;
            }
            else if (!String.IsNullOrEmpty(txtGroupName.Text))
            {
                groupName = txtGroupName.Text;
                String groupCode = RandomString();
                String hostid = Application.Current.Properties["savedUserID"].ToString();
                AppProperties.setSavedGroupName(groupName);
                int hostID = Convert.ToInt32(hostid);
                int groupID = NetworkUtils.insertGroup(groupName, groupCode, hostID);
                AppProperties.setCurrentGroupId(groupID);
                NetworkUtils.groupsDictionary.Add(groupID, groupName);
                AppProperties.setCurrentGroup(Application.Current.Properties["savedGroupName"].ToString());
                createHamburgerIcon(new GroupPage(), Application.Current.Properties["savedGroupName"].ToString());
            }
            else
            {
                lblError.Text = "You must enter a group name.";
                lblError.IsVisible = true;
            }
        }

        //Create new Random String and insert into DB
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
            while (NetworkUtils.searchDBForRandom(finalString) > 0)
            {
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];

                }
                finalString = new string(stringChars);
            }

            AppProperties.setSavedGroupCode(finalString);
            return finalString;
        }//RandomString

        //Might be a better way to call this method from app instead of rewriting it for each page
        public static void createHamburgerIcon()
        {
            var menuPage = new MenuPage();
            App.navigationPage = new NavigationPage(new GroupPage { Title = Application.Current.Properties["savedGroupName"].ToString() }); //Displays username next to icon
            App.navigationPage.BarBackgroundColor = Color.Red;
            App.navigationPage.BackgroundColor = Color.Black;
            var rootPage = new RootPage();
            rootPage.Master = menuPage;
            rootPage.Detail = App.navigationPage;
            App.Current.MainPage = rootPage;
        }

        //Create the ham menu the better way
        public static void createHamburgerIcon(Page page, String title)
        {
            Page newPage = page;
            newPage.Title = title;
            var menuPage = new MenuPage();
            App.navigationPage = new NavigationPage(newPage);
            App.navigationPage.BarBackgroundColor = Color.Red;
            App.navigationPage.BackgroundColor = Color.Black;
            var rootPage = new RootPage();
            rootPage.Master = menuPage;
            rootPage.Detail = App.navigationPage;
            App.Current.MainPage = rootPage;
        }

    }
}