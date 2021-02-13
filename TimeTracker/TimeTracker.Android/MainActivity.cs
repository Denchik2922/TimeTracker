using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace TimeTracker.Droid
{

    [Activity(Label = "TimeTracker", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Thread.CurrentThread.CurrentCulture = SetLenguage();

            Xamarin.Forms.Forms.SetFlags(new string[] { "Shapes_Experimental" });
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        private CultureInfo SetLenguage()
        {
            var ru = CultureInfo.GetCultureInfo("ru-RU");
            ru = (CultureInfo)ru.Clone();
            ru.DateTimeFormat.MonthNames =
                ru.DateTimeFormat.MonthNames
                    .Select(m => ru.TextInfo.ToTitleCase(m))
                    .ToArray();

            /*ru.DateTimeFormat.MonthGenitiveNames =
				ru.DateTimeFormat.MonthGenitiveNames
					.Select(m => ru.TextInfo.ToTitleCase(m))
					.ToArray();*/

            return ru;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}