using Android.App;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;
using Android.Preferences;
using Android.Content;


namespace travelogue
{
    [Activity(Label = "Trip it!", MainLauncher = true, Theme = "@android:style/Theme.NoTitleBar", Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        //connection to azure
        public static MobileServiceClient MobileService = new MobileServiceClient("http://tripit.azurewebsites.net");

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Set our custom view
            //ActionBar.SetCustomView(Resource.Layout.action_bar);
            //ActionBar.SetDisplayShowCustomEnabled(true);
            // Set our view from the "main" layout resource


            // Get our button from the layout resource,
            // and attach an event to it
            //  ActionBar.Hide();
            //Shared preferences
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            var log = prefs.GetBoolean("LoggedIn", false);
            if (log)
            { StartActivity(typeof(ProfileActivity)); }
            else
            {

                SetContentView(Resource.Layout.Main);
                TextView start = FindViewById<TextView>(Resource.Id.myButton);
                start.PaintFlags = Android.Graphics.PaintFlags.UnderlineText;
                start.Click += Start_Click;
            }
        }

        void Start_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(LogInActivity));
        }

    }
}


