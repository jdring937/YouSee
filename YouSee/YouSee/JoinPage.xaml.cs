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
	public partial class JoinPage : ContentPage
	{
		public JoinPage ()
		{
			InitializeComponent ();
            entInviteCode.TextChanged += EntInviteCode_TextChanged;
            btnSubmit.Clicked += BtnSubmit_Clicked;
		}

        //Insert the user into group
        private void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            String groupCode = entInviteCode.Text;
            int groupID = NetworkUtils.getGroupIdFromGroupCode(groupCode);
            int userID = (int)Application.Current.Properties["savedUserID"];
            String groupName = NetworkUtils.getGroupNameFromGroupCode(groupCode);
            try
            {
                NetworkUtils.insertIntoGroup(groupID, userID);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //Display error label if code was wrong
            if(groupID == 0)
            {
                lblError.IsVisible = true;
            }
            else
            {
                try
                {
                    //Add the dictionary to the app properties
                    NetworkUtils.groupsDictionary.Add(groupID, groupName);
                    AppProperties.setGroupsDictionary();
                }
                catch(Exception ex)
                {
                    lblError.Text = "You are already a member of that group";
                    lblError.IsVisible = true;
                }
                AppProperties.setCurrentGroup(groupName);
                CreatePage.createHamburgerIcon(new GroupPage(), groupName);
            }
        }

        //Text changed event for entry. Check if max length > 8, enable button when 
        private void EntInviteCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            int maxLength = 8;
            String entText = entInviteCode.Text;
            if(entText.Length == maxLength)
            {
                btnSubmit.BackgroundColor = Color.Red;
                btnSubmit.IsEnabled = true;                
            }
            if(entText.Length > maxLength)
            {
                entText = entText.Remove(entText.Length - 1);
                entInviteCode.Text = entText;
            }
        }

	}
}