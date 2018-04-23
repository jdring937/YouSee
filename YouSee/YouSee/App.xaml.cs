using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace YouSee
{
	public partial class App : Application
	{
        //Add a reference to any page you want to have a hamburger menu... lookk at createHamburgerIcon() for demo
        public static NavigationPage navigationPage { get; set; }
        public static double ScreenWidth { get; internal set; }
        public static double ScreenHeight { get; internal set; }

        public App ()
		{
            InitializeComponent();
            Dictionary<int, String> userGroups = NetworkUtils.getUserGroups();
            //Determine which screen should be displayed on load
            //https://forums.xamarin.com/discussion/105085/app-launch-login-page-when-launched-first-time-next-time-when-app-is-open-enter-pin-is-asked-how
            if (Application.Current.Properties.ContainsKey("currentGroupID") && userGroups.Count > 0)
            {
                CreatePage.createHamburgerIcon(new GroupPage(), Application.Current.Properties["currentGroup"].ToString());
            }
            else if (Application.Current.Properties.ContainsKey("savedUserName"))
            {
                //createHamburgerIcon();
                CreatePage.createHamburgerIcon(new MainPage(), Application.Current.Properties["savedUserName"].ToString());
            }
            else
            {
                MainPage = new NameScreen();
            }

        }

		protected override void OnStart ()
		{
            // Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        //Create a page with hamburger menu... Not sure how to create a page dynamically. Currently only creates main page. 
        //public static void createHamburgerIcon()
        //{
        //    var menuPage = new MenuPage();
        //    //This line determines the page that will be opened with hamburger menu (Change 'MainPge' to whatever page you want)
        //    navigationPage = new NavigationPage(new MainPage{ Title = Application.Current.Properties["savedUserName"].ToString() }); //Displays username next to icon
        //    navigationPage.BarBackgroundColor = Color.Red;
        //    navigationPage.BackgroundColor = Color.Black;
        //    var rootPage = new RootPage();
        //    rootPage.Master = menuPage;
        //    rootPage.Detail = App.navigationPage;
        //    App.Current.MainPage = rootPage;
        //}
    }
}
