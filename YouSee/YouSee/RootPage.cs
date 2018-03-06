//This class required for hamburger menu

using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace YouSee
{
    public partial class RootPage : MasterDetailPage
    {
        public RootPage()
        {
            //InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;
        }
    }
}
