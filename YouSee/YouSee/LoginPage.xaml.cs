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
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
            btnSubmit.Clicked += BtnSubmit_Clicked;
		}

        private void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            try
            {
                int userID = NetworkUtils.Login(entUsername.Text, entPassword.Text);
                AppProperties.setSavedUserId(userID);
                AppProperties.saveUserName(entUsername.Text);
                CreatePage.createHamburgerIcon(new MainPage(), entUsername.Text);
            }
            catch
            {
                lblError.Text = "Something went wrong. Please make sure your username and password are correct.";
            }
        }
    }
}