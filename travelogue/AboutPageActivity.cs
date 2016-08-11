
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

namespace travelogue
{
	[Activity (Label = "AboutPageActivity")]			
	public class AboutPageActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			ActionBar.SetCustomView(Resource.Layout.action_bar);
			ActionBar.SetDisplayShowCustomEnabled(true);
			SetContentView (Resource.Layout.AboutPage);

			//action bar events
			LinearLayout newsbtn = FindViewById<LinearLayout> (Resource.Id.newsbtn);
			LinearLayout homebtn = FindViewById<LinearLayout> (Resource.Id.homebtn);
			LinearLayout settingsbtn = FindViewById<LinearLayout> (Resource.Id.settingsbtn);
			LinearLayout searchbtn = FindViewById<LinearLayout> (Resource.Id.searchbtn);
			LinearLayout aboutbtn = FindViewById<LinearLayout> (Resource.Id.aboutbtn);
			// Create your application here

			Button btnContact=FindViewById<Button>(Resource.Id.btnContact);
			btnContact.Click+= BtnContact_Click;
            //actionbar actions
            settingsbtn.Click += Settingsbtn_Click;
            aboutbtn.Click+= Aboutbtn_Click;
			homebtn.Click+= Homebtn_Click;
			newsbtn.Click += Newsbtn_Click;
			searchbtn.Click+= Searchbtn_Click;
		}
        void Settingsbtn_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(SettingActivity));
        }
        void Aboutbtn_Click (object sender, EventArgs e)
		{
			StartActivity (typeof(AboutPageActivity));
		}

		void Homebtn_Click (object sender, EventArgs e)
		{
			StartActivity (typeof(ProfileActivity));
		}
		void Searchbtn_Click (object sender, EventArgs e)
		{
			StartActivity (typeof(SearchPageActivity));
		}
		void Newsbtn_Click (object sender, EventArgs e)
		{
			StartActivity (typeof(NewsPage));
		}
		void BtnContact_Click (object sender, EventArgs e)
		{
			Intent i = new Intent(Intent.ActionSend);
			i.SetType("message/rfc822");
			i.PutExtra(Intent.ExtraEmail, new String[] { "developers.tripit@gmail.com" });
			i.PutExtra(Intent.ExtraSubject, "Trip it issues");
			i.PutExtra(Intent.ExtraText, "Enter your feedback here");
			try
			{
				StartActivity(Intent.CreateChooser(i, "Send mail..."));
			}
			catch 
			{
				
				Toast.MakeText(this, "There are no email clients installed.", ToastLength.Short).Show();
			}
		}
	}
}

