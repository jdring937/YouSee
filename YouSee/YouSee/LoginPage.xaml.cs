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
        public LoginPage()
        {
            InitializeComponent();
            btnSubmit.Clicked += BtnSubmit_Clicked;
            btnBack.Clicked += BtnBack_Clicked;
        }

        //Go back to create user screen
        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NameScreen();
        }

        //Login
        private void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(entPassword.Text) || String.IsNullOrEmpty(entUsername.Text))
            {
                lblError.Text = "Please enter your credentials first.";
                lblError.IsVisible = true;
            }
            else
            {
                int userID = NetworkUtils.Login(entUsername.Text, entPassword.Text);
                if (userID == 0)
                {
                    lblError.Text = "Not a valid username and password.";
                    lblError.IsVisible = true;
                }
                else
                {
                    AppProperties.setSavedUserId(userID);
                    AppProperties.saveUserName(entUsername.Text);
                    if (Application.Current.Properties.ContainsKey("currentGroupID"))
                    {
                        String groupName = Application.Current.Properties["currentGroup"].ToString();
                        CreatePage.createHamburgerIcon(new GroupPage(), groupName);
                    }
                    else
                    {
                        CreatePage.createHamburgerIcon(new MainPage(), entUsername.Text);
                    }
                }
            }
        }
    }
}