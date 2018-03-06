using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YouSee
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewPageJoin : ContentPage
    {
        //User info that is used later on
        static string grabGroupID;
        static string userName = "";
        static string grabUserID;
        static int groupID;
        static int userID;
        //Returns users from DB
        static String grabGroupQuery = "SELECT groupName FROM tGroup";
        //String used to connect to DB
        static string connString = @"Server=youseedatabase.cxj5odskcws0.us-east-2.rds.amazonaws.com,1433;DataBase=yousee;User ID=youseeDatabase; Password=yousee18";
        //The file with the username the user picked
        static string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        static string filename = Path.Combine(path, "YouSee.txt");
        //A Observable Collection that will hold all the groups that are in the DBs
        static ObservableCollection<string> Items = groups();

        public ListViewPageJoin()
        {
            InitializeComponent();
            //Creating a click for the back button
            btnBack.Clicked += btnBack_Clicked;
            //A Observable Collection that will hold all the groups that are in the DBs
            Items = new ObservableCollection<string>();
            //Setting the source of the List View to the DB groups
            MyListView.ItemsSource = groups();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs groupNames)
        {
            if (groupNames.Item == null)
            {
                return;
            }
            //SQL for grabing the groupID from the DB
            grabGroupID = "SELECT groupID FROM tGroup WHERE(GroupName = N'" + groupNames.Item + "') ";

            using (var streamReader = new StreamReader(filename))
            {
                userName = streamReader.ReadLine();
            }
            //SQL for grabing the UserID from the DB
            grabUserID = "SELECT userID FROM tUsers WHERE(userName = N'" + userName + "') ";

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    SqlCommand execCmd = new SqlCommand(grabGroupID, sqlConn);
                    execCmd.Connection.Open();

                    SqlDataAdapter daGroups = new SqlDataAdapter(execCmd);
                    DataTable dtGroups = new DataTable();
                    daGroups.Fill(dtGroups);

                    //Add filtered data to Groups list
                    for (int i = 0; i < dtGroups.Rows.Count; i++)
                    {
                        DataRow dr = dtGroups.Rows[i];
                        groupID = Convert.ToInt32(dr["groupID"]);
                    }

                    execCmd = new SqlCommand(grabUserID, sqlConn);

                    daGroups = new SqlDataAdapter(execCmd);
                    dtGroups = new DataTable();
                    daGroups.Fill(dtGroups);

                    //Add filtered data to Groups list
                    for (int i = 0; i < dtGroups.Rows.Count; i++)
                    {
                        DataRow dr = dtGroups.Rows[i];
                        userID = Convert.ToInt32(dr["userID"]);
                    }

                    joinGroup(userID, groupID);

                }
            }
            catch (Exception ex)
            {
                //Error for if something goes wrong
                Console.WriteLine(ex);
            }
            await DisplayAlert("Joined!", "You have joined " + groupNames.Item, "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            //Take the user back to the main page
            App.createHamburgerIcon();
        }

        public static void joinGroup(int userID, int groupID)
        {

            using (SqlConnection openCon = new SqlConnection(connString))
            {
                //SQL for Inserting the user and their group to the tGroup_User table
                string insertUserToGroup = "INSERT into tGroup_User (GroupID,UserID) VALUES (@groupID,@userID)";

                using (SqlCommand queryinsertUserToGroup = new SqlCommand(insertUserToGroup, openCon))
                {         
                    //SQL stuff for parameters
                    queryinsertUserToGroup.Parameters.AddWithValue("@groupID", groupID);
                    queryinsertUserToGroup.Parameters.AddWithValue("@userID", userID);
                    openCon.Open();
                    //Where the magic happens
                    queryinsertUserToGroup.ExecuteNonQuery();
                }
            }
        }

        public static ObservableCollection<string> groups()
        {

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    SqlCommand execCmd = new SqlCommand(grabGroupQuery, sqlConn);
                    execCmd.Connection.Open();

                    SqlDataAdapter daGroups = new SqlDataAdapter(execCmd);
                    DataTable dtGroups = new DataTable();
                    daGroups.Fill(dtGroups);

                    //Add filtered data to Groups list
                    for (int i = 0; i < dtGroups.Rows.Count; i++)
                    {
                        DataRow dr = dtGroups.Rows[i];
                        Items.Add(dr["groupName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Items;
        }
    }
}
