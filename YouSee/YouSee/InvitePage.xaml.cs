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
	public partial class InvitePage : ContentPage
	{
		public InvitePage ()
		{
			InitializeComponent ();
            entInviteCode.Text = Application.Current.Properties["savedGroupCode"].ToString();
            //In order to allow copy/paste (e_e) 
            //https://stackoverflow.com/questions/27570497/how-do-you-allow-users-to-copy-and-paste-from-an-xamarin-forms-label
            entInviteCode.IsEnabled = false;
            entInviteCode.TextColor = Color.Black;
            btnDone.Clicked += BtnDone_Clicked;
		}

        private void BtnDone_Clicked(object sender, EventArgs e)
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