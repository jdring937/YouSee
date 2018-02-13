//YT GeoLocation tutorial: https://www.youtube.com/watch?v=pH1WaO-5LDk
//Drawing lines between two points: https://stackoverflow.com/questions/13433648/draw-a-line-between-two-point-on-a-google-map-using-jquery

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Geolocator;

namespace YouSee
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            btnGetLocation.Clicked += BtnGetLocation_Clicked;
		}

        private async void BtnGetLocation_Clicked(object sender, EventArgs e)
        {
        }

        
    }
}
