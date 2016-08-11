using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using travelogue.Models;
using Android.Graphics.Drawables;
using Android.Preferences;

namespace travelogue
{
	[Activity(Label = "News")]
	public class NewsPage : Activity
	{
		private List<Content> mItems,user;
		private ListView mListView;
		string mUser;
		Button ImgAddNews;
		//public Content objec

		public IEnumerable<object> table { get; private set; }

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetDisplayShowTitleEnabled(false);
            ActionBar.SetCustomView(Resource.Layout.action_bar);
            ActionBar.SetDisplayShowCustomEnabled(true);

            SetContentView(Resource.Layout.NewsPage);

            //actionbar events
            LinearLayout newsbtn = FindViewById<LinearLayout>(Resource.Id.newsbtn);
            LinearLayout homebtn = FindViewById<LinearLayout>(Resource.Id.homebtn);
            LinearLayout settingsbtn = FindViewById<LinearLayout>(Resource.Id.settingsbtn);
            LinearLayout searchbtn = FindViewById<LinearLayout>(Resource.Id.searchbtn);
            LinearLayout aboutbtn = FindViewById<LinearLayout>(Resource.Id.aboutbtn);
            ImgAddNews = FindViewById<Button>(Resource.Id.ImgAddNews);
            mListView = FindViewById<ListView>(Resource.Id.ListSubjects);
            user = await MainActivity.MobileService.GetTable<Content>().ToListAsync();
            
            mItems = new List<Content>();

            NewsList adapter = new NewsList(this, user);

            mListView.Adapter = adapter;
			mListView.ItemClick+= MListView_ItemClick;
			mListView.ItemLongClick+= MListView_ItemLongClick;
			mListView.RefreshDrawableState ();
			//AddButton
            ImgAddNews.Click += ImgAddNews_Click;

            //actionbar actions
            settingsbtn.Click += Settingsbtn_Click;
            aboutbtn.Click += Aboutbtn_Click;
            homebtn.Click += Homebtn_Click;
            newsbtn.Click += Newsbtn_Click;
            searchbtn.Click += Searchbtn_Click;
        }

       void MListView_ItemLongClick (object sender, AdapterView.ItemLongClickEventArgs e)
        {
			LayoutInflater layoutinflator = (LayoutInflater)BaseContext.GetSystemService (LayoutInflaterService);
			View popupview = layoutinflator.Inflate (Resource.Layout.Popup, null);

			PopupWindow popupwindow = new PopupWindow (popupview, Android.Views.ViewGroup.LayoutParams.WrapContent, Android.Views.ViewGroup.LayoutParams.WrapContent);
			popupwindow.SetBackgroundDrawable (new BitmapDrawable ());
			popupwindow.OutsideTouchable = (true);
			popupwindow.ShowAsDropDown (mListView, 0, 0);

			//Button btnEdit=popupview.FindViewById<Button>(Resource.Id.btnEdit);
			Button btnDel=popupview.FindViewById<Button>(Resource.Id.btnDel);
			btnDel.Click+=async delegate {
				var mUser = user[e.Position].id;
				try{
				ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
				var log = prefs.GetBoolean("LoggedIn", false);
				var users = prefs.GetString("user", "");
				var result=await MainActivity.MobileService.GetTable<Content>().Where (x=>x.id==mUser&&x.userid==users).ToListAsync();
				var obj=result.FirstOrDefault();
				await MainActivity.MobileService.GetTable<Content>().DeleteAsync(obj);
				StartActivity(typeof(NewsPage));
				}
				catch{
					Toast.MakeText(this,"Unable to delete",ToastLength.Short).Show();	
				}
			};

		}

        

        void MListView_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
        {
			try{
				
				var mUser = user[e.Position].id;
				var intent = new Intent(this,typeof(DescriptionActivity));
				intent.PutExtra("user",mUser);
	    		StartActivity (intent);
			}catch{
				
			}
        }
		public string UserReturner()
		{
			return mUser;
		}
        void ImgAddNews_Click (object sender, EventArgs e)
		{
			StartActivity (typeof(AddNewsActivity));	
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