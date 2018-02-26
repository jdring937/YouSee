//Page created when user clicks hamburger menu
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
	public partial class MenuPage : ContentPage
	{
		public MenuPage ()
		{
            Title = "Menu";
            InitializeComponent ();
            btnJoinSm.Clicked += btnJoinSm_Clicked;

        }

        private void btnJoinSm_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new ListViewPageJoin();
        }
    }
}