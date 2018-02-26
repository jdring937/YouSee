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
	public partial class CreatePage : ContentPage
	{
        
		public CreatePage ()
		{
			InitializeComponent ();
            btnCreateGroup.Clicked += BtnCreateGroup_Clicked;
		}

        //When user clicks create group run spInsertGroup on DB
        private void BtnCreateGroup_Clicked(object sender, EventArgs e)
        {
            String groupName = txtGroupName.Text;
            String groupCode = RandomString();
            String hostid = Application.Current.Properties["savedUserID"].ToString();
            Application.Current.Properties.Add("savedGroupName", groupName);
            int hostID = Convert.ToInt32(hostid);

            NetworkUtils.insertGroup(groupName, groupCode, hostID);

            //Change to groupPage
            createHamburgerIcon();
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

            //Save the group code. Overwrite it if it exists already
            if (Application.Current.Properties.ContainsKey("savedGroupCode"))
            {
                Application.Current.Properties.Remove("savedGroupCode");
                Application.Current.Properties.Add("savedGroupCode", finalString);
            }
            else
            {
                Application.Current.Properties.Add("savedGroupCode", finalString);
            }

            return finalString;
        }//RandomString


        //Might be a better way to call this method from app instead of rewriting it for each page
        public static void createHamburgerIcon()
        {
            var menuPage = new MenuPage();
            //This line determines the page that will be opened with hamburger menu (Change 'MainPge' to whatever page you want)
            App.navigationPage = new NavigationPage(new GroupPage { Title = Application.Current.Properties["savedUserName"].ToString() }); //Displays username next to icon
            App.navigationPage.BarBackgroundColor = Color.Red;
            App.navigationPage.BackgroundColor = Color.Black;
            var rootPage = new RootPage();
            rootPage.Master = menuPage;
            rootPage.Detail = App.navigationPage;
            App.Current.MainPage = rootPage;
        }

    }
}