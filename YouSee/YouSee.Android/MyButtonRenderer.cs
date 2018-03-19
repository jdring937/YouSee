using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using YouSee;
using YouSee.Droid;

[assembly: ExportRenderer(typeof(MyButton), typeof(MyButtonRenderer))]
namespace YouSee.Droid
{
    public class MyButtonRenderer : ButtonRenderer
    {
        //MyButton will no longer auto-caps everything
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e) { base.OnElementChanged(e); var button = this.Control; button.SetAllCaps(false); button.SetPadding(5, 5, 5, 5); button.SetPaddingRelative(5, 5, 5, 5); }

        //This is needed to prevent obsolete error, even though it's never called
        public MyButtonRenderer(Context context) : base(context)
        {

        }
    }

}