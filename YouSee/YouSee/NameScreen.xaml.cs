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
        }

        private void btnUsername_Clicked(object sender, EventArgs e)
        {
            //This works
            insertUser();
            saveUserName();

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
            //App.Current.MainPage = new MainPage();

            App.createHamburgerIcon();
        }


        //Insert user dynamically, set UserID
        private void insertUser()
        {
            String ipAddress = NetworkUtils.GetLocalIPAddress();
            String userName = entryName.Text;

            //Executes SP and returns userID
            userID = NetworkUtils.insertUser(ipAddress, userName);
            Console.WriteLine("User ID = " + userID.ToString());
        }

        //Save the username to a persistent variable
        private async void saveUserName()
        {
            App.Current.Properties.Add("savedUserName", entryName.Text);
            await App.Current.SavePropertiesAsync();
        }
    }
}