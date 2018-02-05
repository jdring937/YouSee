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
		public NameScreen ()
		{
			InitializeComponent ();
            btnUsername.Clicked += btnUsername_Clicked;
        }
        private void btnUsername_Clicked(object sender, EventArgs e)
        {
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
        }
    }
}