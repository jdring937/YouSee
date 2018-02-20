using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YouSee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewPageJoin : ContentPage
    {
        static ObservableCollection<string> Items;

        public ListViewPageJoin()
        {
            InitializeComponent();
            btnBack.Clicked += btnBack_Clicked;
            Items = new ObservableCollection<string>();

            MyListView.ItemsSource = groups();//Add DB data here
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            //PromptAsync();
            await DisplayAlert("Joined!", "You have joined " + e.Item, "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }


        public static ObservableCollection<string> groups()
        {
            //Used to select 'username' column from returned results
            String myString = "groupName";

            //Returns users from DB
            String query = "SELECT groupName FROM tGroup";

            //String used to connect to DB
            String connString = @"Server=youseedatabase.cxj5odskcws0.us-east-2.rds.amazonaws.com,1433;DataBase=yousee;User ID=youseeDatabase; Password=yousee18";

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    SqlCommand execCmd = new SqlCommand(query, sqlConn);
                    execCmd.Connection.Open();

                    SqlDataAdapter daGroups = new SqlDataAdapter(execCmd);
                    DataTable dtGroups = new DataTable();
                    daGroups.Fill(dtGroups);

                    //Add filtered data to Groups list
                    for (int i = 0; i < dtGroups.Rows.Count; i++)
                    {
                        DataRow dr = dtGroups.Rows[i];
                        Items.Add(dr[myString].ToString());
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
