
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
using travelogue.Models;
using Android.Views.InputMethods;
using Android.Preferences;

namespace travelogue
{
	[Activity (Label = "SearchPageActivity")]			
	public class SearchPageActivity : Activity
	{
		private List<Content> user;
		private ListView nListView;
		//string nUser;
		TextView txtNoResult;
		ImageButton btnSearchList;
		EditText txtSearch;
		ProgressBar pbar;
		bool log;
		string users;
		public IEnumerable<object> table { get; private set; }

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your application here
			ActionBar.SetCustomView (Resource.Layout.action_bar);
			ActionBar.SetDisplayShowCustomEnabled (true);

			SetContentView (Resource.Layout.SearchPage);
			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
			 log = prefs.GetBoolean("LoggedIn", false);
			 users = prefs.GetString("user", "");

			if (!(log && users != ""))
			{
				ActionBar.Hide ();
			}
			//actionbar events
			LinearLayout newsbtn = FindViewById<LinearLayout> (Resource.Id.newsbtn);
			LinearLayout homebtn = FindViewById<LinearLayout> (Resource.Id.homebtn);
			LinearLayout settingsbtn = FindViewById<LinearLayout> (Resource.Id.settingsbtn);
			LinearLayout searchbtn = FindViewById<LinearLayout> (Resource.Id.searchbtn);
			LinearLayout aboutbtn = FindViewById<LinearLayout> (Resource.Id.aboutbtn);
			nListView = FindViewById<ListView>(Resource.Id.searchList);
			txtSearch = FindViewById<EditText> (Resource.Id.txtSearch);
			txtNoResult = FindViewById<TextView> (Resource.Id.txtNoResult);
			btnSearchList = FindViewById<ImageButton> (Resource.Id.btnSearchList);
			pbar = FindViewById<ProgressBar> (Resource.Id.progressBar);
			pbar.Visibility=ViewStates.Gone;
			txtNoResult.Visibility=ViewStates.Gone;

			//var user = await MainActivity.MobileService.GetTable<Content>().Where(x => x.place ==txtSearch.Text ).ToListAsync();

            //action bar actions
			btnSearchList.Click+= BtnSearch_Click;
            settingsbtn.Click += Settingsbtn_Click;
            aboutbtn.Click += Aboutbtn_Click;
			homebtn.Click += Homebtn_Click;
			newsbtn.Click += Newsbtn_Click;
			searchbtn.Click+= Searchbtn_Click;
		}

		async void BtnSearch_Click (object sender, EventArgs e)
		{
			InputMethodManager inputManager = (InputMethodManager) this.GetSystemService(Context.InputMethodService);

			inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
			try
			{
				nListView.Visibility=ViewStates.Gone;
				pbar.Visibility=ViewStates.Visible;
				 user = await MainActivity.MobileService.GetTable<Content>().Where(x => x.place.ToUpper().Contains(txtSearch.Text.ToUpper())).ToListAsync();
				if (user.Count==0){
					pbar.Visibility=ViewStates.Gone;
					txtNoResult.Visibility=ViewStates.Visible;}
				else{
					txtNoResult.Visibility=ViewStates.Gone;
				NewsList adapter = new NewsList(this, user);
				 nListView.Adapter = adapter;

				nListView.Visibility=ViewStates.Visible;
				pbar.Visibility=ViewStates.Gone;

				 nListView.ItemClick+= NListView_ItemClick;

				}
			}
			catch
			{
				nListView.Visibility=ViewStates.Gone;
				pbar.Visibility=ViewStates.Visible;
					
				Toast.MakeText (this, "Connection error", ToastLength.Short).Show ();
			}
		}

		void NListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			try{
				if (!(log && users != "")){
				var mUser = user[e.Position].id;
				var intent = new Intent(this,typeof(SearchListActivity));
				intent.PutExtra("user",mUser);
				StartActivity (intent);
				}
				else
				{
					var mUser = user[e.Position].id;
					var intent = new Intent(this,typeof(DescriptionActivity));
					intent.PutExtra("user",mUser);
					StartActivity (intent);
				}
			}catch{

			}
		}
        void Settingsbtn_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(SettingActivity));
        }
        void Searchbtn_Click (object sender, EventArgs e)
		{
			StartActivity (typeof(SearchPageActivity));
		}
		void Aboutbtn_Click (object sender, EventArgs e)
		{
			StartActivity (typeof(AboutPageActivity));
		}

		void Homebtn_Click (object sender, EventArgs e)
		{
			StartActivity (typeof(ProfileActivity));
		}

		void Newsbtn_Click (object sender, EventArgs e)
		{
			StartActivity (typeof(NewsPage));
		}
		

	}
}

