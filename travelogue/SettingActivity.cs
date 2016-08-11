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
using Android.Preferences;
using travelogue.Models;
using Microsoft.WindowsAzure.MobileServices;
using Android.Views.InputMethods;

namespace travelogue
{
    [Activity(Label = "SettingActivity")]
    public class SettingActivity : Activity
    {
		Button btnPasstemp,btnPass,btnlogout;
		EditText Pass0,Pass1,Pass2;
		LinearLayout passlayout;
		ProgressBar progressBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetDisplayShowTitleEnabled(false);
            ActionBar.SetCustomView(Resource.Layout.action_bar);
            ActionBar.SetDisplayShowCustomEnabled(true);

            SetContentView(Resource.Layout.SettingsPage);

			btnPass = FindViewById<Button> (Resource.Id.btnPass);
			btnPasstemp = FindViewById<Button> (Resource.Id.btnPasstemp);
			passlayout = FindViewById<LinearLayout> (Resource.Id.passlayout);
			btnlogout = FindViewById<Button>(Resource.Id.btnlogout);
			progressBar = FindViewById<ProgressBar> (Resource.Id.progressBar);
			Pass0 = FindViewById<EditText> (Resource.Id.txtPassold);
			Pass1 = FindViewById<EditText> (Resource.Id.txtPass1);
			Pass2 = FindViewById<EditText> (Resource.Id.txtPass2);
			passlayout.Visibility = ViewStates.Gone;
			progressBar.Visibility = ViewStates.Gone;


            LinearLayout newsbtn = FindViewById<LinearLayout>(Resource.Id.newsbtn);
            LinearLayout homebtn = FindViewById<LinearLayout>(Resource.Id.homebtn);
            LinearLayout settingsbtn = FindViewById<LinearLayout>(Resource.Id.settingsbtn);
            LinearLayout searchbtn = FindViewById<LinearLayout>(Resource.Id.searchbtn);
            LinearLayout aboutbtn = FindViewById<LinearLayout>(Resource.Id.aboutbtn);


            settingsbtn.Click += Settingsbtn_Click;
            aboutbtn.Click += Aboutbtn_Click;
            homebtn.Click += Homebtn_Click;
            newsbtn.Click += Newsbtn_Click;
			btnPass.Click+= BtnPass_Click;
            searchbtn.Click += Searchbtn_Click;
			btnPasstemp.Click+= BtnPasstemp_Click;
            btnlogout.Click += Btnlogout_Click;

            // Create your application here
        }

       async void BtnPass_Click (object sender, EventArgs e)
        {
			InputMethodManager inputManager = (InputMethodManager) this.GetSystemService(Context.InputMethodService);

			inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
			if (Pass0.Text == "" || Pass1.Text == "" || Pass2.Text == "")
			{
			
				Toast.MakeText (this, "fill all the fields", ToastLength.Short).Show ();
			} else 
			{
				ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences (this);
				var log = prefs.GetBoolean ("LoggedIn", false);
				var users = prefs.GetString ("user", "");
				if (log && users != "") 
				{
					passlayout.Visibility = ViewStates.Gone;
					progressBar.Visibility = ViewStates.Visible;
					var user = await MainActivity.MobileService.GetTable<User> ().Where (x => x.username == users).ToListAsync ();
					var loggeduser = user.FirstOrDefault ();
					if (!(Pass0.Text == loggeduser.password)) {
						Toast.MakeText (this, "password incorrect", ToastLength.Short).Show ();
					} else if (!(Pass1.Text == Pass2.Text)) {
						Toast.MakeText (this, "passwords do not match", ToastLength.Short).Show ();
					} else
					{
						try{
						loggeduser.password = Pass1.Text;
						CurrentPlatform.Init();
						await MainActivity.MobileService.GetTable<User>().UpdateAsync(loggeduser);
						}
						catch
						{
							Toast.MakeText (this, "Connection error", ToastLength.Short).Show ();

						}
					}
				}
				progressBar.Visibility = ViewStates.Gone;
				passlayout.Visibility = ViewStates.Visible;
			}

        }

        void BtnPasstemp_Click (object sender, EventArgs e)
        {
			passlayout.Visibility = ViewStates.Visible;
			btnPasstemp.Visibility = ViewStates.Gone;
        }

        void Btnlogout_Click(object sender, EventArgs e)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutBoolean("LoggedIn", false);
            editor.PutString("user","");
            editor.Apply();

           // PreferenceManager.GetDefaultSharedPreferences(this) = false;
            StartActivity(typeof(LogInActivity));
        }

        void Settingsbtn_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(SettingActivity));
        }

        void Aboutbtn_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(AboutPageActivity));
        }

        void Homebtn_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ProfileActivity));
        }

        void Newsbtn_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(NewsPage));
        }
        void Searchbtn_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(SearchPageActivity));
        }


    }

}