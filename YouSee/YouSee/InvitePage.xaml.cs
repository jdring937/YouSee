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
            //In order to allow copy/paste (e_e) 
            //https://stackoverflow.com/questions/27570497/how-do-you-allow-users-to-copy-and-paste-from-an-xamarin-forms-label
            populateEntryGroupCode();
            entInviteCode.IsEnabled = false;
            entInviteCode.TextColor = Color.Black;
            //btnDone.Clicked += BtnDone_Clicked;
		}

        //private void BtnDone_Clicked(object sender, EventArgs e)
        //{
        //    CreatePage.createHamburgerIcon(new GroupPage(), GroupPage.groupName);
        //}

        private void populateEntryGroupCode()
        {
            int userID = (int)Application.Current.Properties["savedUserID"];
            String groupName = GroupPage.groupName;
            Console.WriteLine(groupName);
            entInviteCode.Text = NetworkUtils.getGroupCodeFromUserIdAndGroupName(userID, groupName);
            Console.WriteLine(entInviteCode.Text);
        }
    }
}