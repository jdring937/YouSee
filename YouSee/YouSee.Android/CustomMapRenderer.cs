using System;
using System.Collections.Generic;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using YouSee.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using YouSee;
using Android.Widget;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace YouSee.Droid
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {
        List<CustomPin> customPins;
        String imgName = null;
        int resImage;
        int caseSwitch = 0;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                customPins = formsMap.CustomPins;
                Control.GetMapAsync(this);
            }
        }
        //Set image by string
        //https://stackoverflow.com/questions/39938391/how-to-change-the-imageview-source-dynamically-from-a-string-xamarin-android
        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            var marker2 = new MarkerOptions();

            if (pin.Label == "My Position!")
            {
                caseSwitch = 0;
                imgName = "pin";                
                resImage = Resources.GetIdentifier(imgName, "drawable", "com.companyname.YouSee");
                marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
                marker.SetTitle(pin.Label);
                marker.SetSnippet(pin.Address);
                marker.SetIcon(BitmapDescriptorFactory.FromResource(resImage));               
                return marker;
            }
            else
            {
                switch(caseSwitch){
                    case 0:
                        imgName = "pin" + 0;
                        caseSwitch++;
                        break;
                    case 1:
                        imgName = "pin" + 1;
                        caseSwitch++;
                        break;
                    case 2:
                        imgName = "pin" + 2;
                        caseSwitch++;
                        break;
                    case 3:
                        imgName = "pin" + 3;
                        caseSwitch++;
                        break;
                    case 4:
                        imgName = "pin" + 4;
                        caseSwitch++;
                        break;
                    case 5:
                        imgName = "pin" + 5;
                        caseSwitch++;
                        break;
                    case 6:
                        imgName = "pin" + 6;
                        caseSwitch = 0;
                        break;
                }
                resImage = Resources.GetIdentifier(imgName, "drawable", "com.companyname.YouSee");
                marker2.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
                marker2.SetTitle(pin.Label);
                marker2.SetSnippet(pin.Address);
                //marker2.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin0));
                marker2.SetIcon(BitmapDescriptorFactory.FromResource(resImage));
                return marker2;
            }
        }

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var customPin = GetCustomPin(e.Marker);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }

            if (!string.IsNullOrWhiteSpace(customPin.Url))
            {
                var url = Android.Net.Uri.Parse(customPin.Url);
                var intent = new Intent(Intent.ActionView, url);
                intent.AddFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(intent);
            }
        }

        CustomPin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            throw new NotImplementedException();
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            throw new NotImplementedException();
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete

}

