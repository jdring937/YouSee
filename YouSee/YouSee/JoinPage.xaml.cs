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
            btnBack.Clicked += BtnBack_Clicked;
            Dictionary<int, String> userGroups = NetworkUtils.getUserGroups();
            if (MenuPage.pageCount <= 1 && userGroups.Count < 1)
            {
                btnBack.IsVisible = true;
            }
            //if(MenuPage.prevPage == null || MenuPage.prevPage == "")
            //{
            //    btnBack.IsVisible = true;
            //}
            //else if(MenuPage.prevPage == Application.Current.Properties["savedUserName"].ToString())
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
            //var otherPage = new MainPage { Title = Application.Current.Properties["savedUserName"].ToString() };
            //var homePage = App.navigationPage.Navigation.NavigationStack.First();
            //App.navigationPage.Navigation.InsertPageBefore(otherPage, homePage);
            //App.navigationPage.PopToRootAsync(false);
        }

        //Insert the user into group
        private void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            String groupCode = entInviteCode.Text;
            int groupID = NetworkUtils.getGroupIdFromGroupCode(groupCode);
            int userID = (int)Application.Current.Properties["savedUserID"];
            Console.WriteLine(NetworkUtils.groupsDictionary.Count);
            if (NetworkUtils.groupsDictionary.Keys.Contains(groupID))
            {
                lblError.Text = "You are already a member of that group.";
                lblError.IsVisible = true;
            }
            else 
            {
                String groupName = NetworkUtils.getGroupNameFromGroupCode(groupCode);
                //if (NetworkUtils.groupsDictionary.Values.Contains(groupName))
                //{
                //    lblError.Text = "You are already a member of a group with that name.";
                //}
                //else
                //{
                    try
                    {
                        //Add the dictionary to the app properties
                        NetworkUtils.groupsDictionary.Add(groupID, groupName);
                        AppProperties.setGroupsDictionary();
                        NetworkUtils.insertIntoGroup(groupID, userID);
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "You are already a member of that group";
                        lblError.IsVisible = true;
                    }
                    AppProperties.setCurrentGroup(groupName);
                    AppProperties.setCurrentGroupId(groupID);
                    CreatePage.createHamburgerIcon(new GroupPage(), groupName);

                    //Display error label if code was wrong
                    if (groupID == 0)
                    {
                        lblError.IsVisible = true;
                    }
                //}
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