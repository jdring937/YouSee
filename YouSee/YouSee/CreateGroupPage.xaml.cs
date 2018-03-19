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
		public CreatePage ()
		{
			InitializeComponent ();
            btnCreateGroup.Clicked += BtnCreateGroup_Clicked;
            btnBack.Clicked += BtnBack_Clicked;
            Dictionary<int, String> userGroups = NetworkUtils.getUserGroups();
            if(MenuPage.pageCount <= 1 && userGroups.Count < 1)
            {
                btnBack.IsVisible = true;
            }
            //if (MenuPage.prevPage == null || MenuPage.prevPage == "")
            //{
            //    btnBack.IsVisible = true;
            //}
            //else if (MenuPage.prevPage == Application.Current.Properties["savedUserName"].ToString())
            //{
            //    btnBack.IsVisible = true;
            //}
            else
            {
                btnBack.IsVisible = false;
            }
        }

        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            CreatePage.createHamburgerIcon(new MainPage(), Application.Current.Properties["savedUserName"].ToString());
        }

        //When user clicks create group run spInsertGroup on DB
        private void BtnCreateGroup_Clicked(object sender, EventArgs e)
        {
            String groupName = txtGroupName.Text;
            //Dictionary<int, String> tempGroupDict = NetworkUtils.getUserGroups();
            ////List<String> tempGroupList = NetworkUtils.getUserGroups();
            //if (tempGroupDict.Values.Contains(groupName))
            //{
            //    btnSameName.IsVisible = true;
            //}
            //else
            //{
                String groupCode = RandomString();
                String hostid = Application.Current.Properties["savedUserID"].ToString();
                ////Overwrite the groupName if it already exists
                //if (Application.Current.Properties.ContainsKey("savedGroupName"))
                //{
                //    Application.Current.Properties.Remove("savedGroupName");
                //    Application.Current.Properties.Add("savedGroupName", groupName);
                //}
                //else
                //{
                //    Application.Current.Properties.Add("savedGroupName", groupName);
                //}
                AppProperties.setSavedGroupName(groupName);
                int hostID = Convert.ToInt32(hostid);
                int groupID = NetworkUtils.insertGroup(groupName, groupCode, hostID);
                AppProperties.setCurrentGroupId(groupID);

                //AppProperties.setGroupsDictionary();
                ////Add the dictionary to the app properties
                NetworkUtils.groupsDictionary.Add(groupID, groupName);
                
                //if (Application.Current.Properties.ContainsKey("groupsDictionary"))
                //{
                //    Application.Current.Properties.Remove("groupsDictionary");
                //    Application.Current.Properties.Add("groupsDictionary", NetworkUtils.groupsDictionary);
                //}
                //else
                //{
                //Application.Current.Properties.Add("groupsDictionary", NetworkUtils.groupsDictionary);
                //}

                //Updates the menupage and creates new nav stack
                //createHamburgerIcon();
                //if (Application.Current.Properties.ContainsKey("currentGroup"))
                //{
                //    Application.Current.Properties.Remove("currentGroup");
                //    Application.Current.Properties.Add("currentGroup", Application.Current.Properties["savedGroupName"]);
                //}
                //else
                //{
                //    Application.Current.Properties.Add("currentGroup", Application.Current.Properties["savedGroupName"]);
                //}
                AppProperties.setCurrentGroup(Application.Current.Properties["savedGroupName"].ToString());
                createHamburgerIcon(new GroupPage(), Application.Current.Properties["savedGroupName"].ToString());

            //}
        }

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
            while (NetworkUtils.searchDBForRandom(finalString) > 0)
            {
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];

                }
                finalString = new string(stringChars);
            }

            AppProperties.setSavedGroupCode(finalString);
            ////Save the group code. Overwrite it if it exists already
            //if (Application.Current.Properties.ContainsKey("savedGroupCode"))
            //{
            //    Application.Current.Properties.Remove("savedGroupCode");
            //    Application.Current.Properties.Add("savedGroupCode", finalString);
            //}
            //else
            //{
            //    Application.Current.Properties.Add("savedGroupCode", finalString);
            //}

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