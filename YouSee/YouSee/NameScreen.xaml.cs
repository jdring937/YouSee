using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YouSee
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NameScreen : ContentPage
	{
        //make userID publicly available (read-only)
        public int userID { get; private set; }

        public NameScreen ()
		{
			InitializeComponent ();
            btnUsername.Clicked += btnUsername_Clicked;
            btnLogin.Clicked += BtnLogin_Clicked;
        }

        private void BtnLogin_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }

        //Add the user to the DB
        private void btnUsername_Clicked(object sender, EventArgs e)
        {
            //String userName = entryName.Text;
            //String password = entryPassword.Text;
            //Make sure the user entered a username
            if (string.IsNullOrWhiteSpace(entryName.Text))
            {
                lblNoUserName.IsVisible = true;
            }
            else
            {
                //Insert user in DB and save username
                insertUser();
                if (userID == 0)
                {
                    lblNoUserName.Text = "That user name already exists. Please choose a different username.";
                    lblNoUserName.IsVisible = true;
                }
                else
                {
                    AppProperties.saveUserName(entryName.Text);

                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string filename = Path.Combine(path, "YouSee.txt");

                    using (var streamWriter = new StreamWriter(filename, false))
                    {
                        streamWriter.WriteLine(entryName.Text);
                    }

                    using (var streamReader = new StreamReader(filename))
                    {
                        string content = streamReader.ReadToEnd();
                        System.Diagnostics.Debug.WriteLine(content);
                    }
                    //Launch MainPage

                    CreatePage.createHamburgerIcon(new MainPage(), Application.Current.Properties["savedUserName"].ToString());
                }
            }
        }

        //Insert user, set UserID
        private void insertUser()
        {
            String ipAddress = NetworkUtils.GetLocalIPAddress();
            String userName = entryName.Text;
            String password = entryPassword.Text;
            //Executes SP and returns userID
            Console.WriteLine(userName);
                userID = NetworkUtils.insertUser(ipAddress, userName, password);           
        }

        ////Save the username to a persistent variable
        //private async void saveUserName()
        //{
        //    App.Current.Properties.Add("savedUserName", entryName.Text);
        //    await App.Current.SavePropertiesAsync();
        //}
    }
}