//Page created when user clicks hamburger menu
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YouSee
{
    class AppProperties: ContentPage
    {
        //Sets the group currently selected by the user
        public static String setCurrentGroup(String groupName)
        {
            //Set the current group name in the app properties
            if (Application.Current.Properties.ContainsKey("currentGroup"))
            {
                Application.Current.Properties.Remove("currentGroup");
                Application.Current.Properties.Add("currentGroup", groupName);
            }
            else
            {
                Application.Current.Properties.Add("currentGroup", groupName);
            }
            String currentGroup = Application.Current.Properties["currentGroup"].ToString();
            return currentGroup;
        }

        public static int setCurrentGroupId(int GroupID)
        {
            //Set the current group name in the app properties
            if (Application.Current.Properties.ContainsKey("currentGroupID"))
            {
                Application.Current.Properties.Remove("currentGroupID");
                Application.Current.Properties.Add("currentGroupID", GroupID);
            }
            else
            {
                Application.Current.Properties.Add("currentGroupID", GroupID);
            }
            int currentGroupID = (int)Application.Current.Properties["currentGroupID"];
            return currentGroupID;
        }

        //Returns a dicitionary with key of groupID, and value of groupName for groups user is in
        public static void setGroupsDictionary()
        {
            if (Application.Current.Properties.ContainsKey("groupsDictionary"))
            {
                Application.Current.Properties.Remove("groupsDictionary");
                Application.Current.Properties.Add("groupsDictionary", NetworkUtils.groupsDictionary);
            }
            else
            {
                Application.Current.Properties.Add("groupsDictionary", NetworkUtils.groupsDictionary);
            }

        }

        //Saves the users ID to app properties
        public static int setSavedUserId(int userID)
        {
            int userId = 0;

            //Should not need an if statement if everything else works the way it should... User should only be prompted to enter this value once
            if (Application.Current.Properties.ContainsKey("savedUserID"))
            {
                Application.Current.Properties.Remove("savedUserID");
                Application.Current.Properties.Add("savedUserID", userID);
            }
            else
            {
                App.Current.Properties.Add("savedUserID", userID);
                userId = (int)App.Current.Properties["savedUserID"];
            }
            return userId;
        }

        //Save the username to a persistent variable
        public static async void saveUserName(String userName)
        {
            if (Application.Current.Properties.ContainsKey("savedUserName"))
            {
                Application.Current.Properties.Remove("savedUserName");
                App.Current.Properties.Add("savedUserName", userName);
            }
            else
            {
                App.Current.Properties.Add("savedUserName", userName);
            }
            await App.Current.SavePropertiesAsync();
        }

        //Save the groupName in the properties... Not sure if this is still needed but I don't wanna delete until sure
        public static String setSavedGroupName(String groupName)
        {
            String group;
            if (Application.Current.Properties.ContainsKey("savedGroupName"))
            {
                Application.Current.Properties.Remove("savedGroupName");
                Application.Current.Properties.Add("savedGroupName", groupName);
            }
            else
            {
                Application.Current.Properties.Add("savedGroupName", groupName);
            }
            group = Application.Current.Properties["savedGroupName"].ToString();
            return group;
        }

        //Save the group code to app properties. Not sure if this is needed either, but don't delete yet
        public static String setSavedGroupCode(String groupCode)
        {
            String code;
            //Save the group code. Overwrite it if it exists already
            if (Application.Current.Properties.ContainsKey("savedGroupCode"))
            {
                Application.Current.Properties.Remove("savedGroupCode");
                Application.Current.Properties.Add("savedGroupCode", groupCode);
            }
            else
            {
                Application.Current.Properties.Add("savedGroupCode", groupCode);
            }
            code = Application.Current.Properties["savedGroupCode"].ToString();
            return code;
        }

    }
}
