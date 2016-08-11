
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

namespace travelogue
{
	[Activity (Label = "SearchListActivity")]			
	public class SearchListActivity : Activity
	{
		ImageView ImageOne,ImageTwo,ImageThree;
		Button btnMap;
		string id;
		Content newContent;
		TextView txtHeader,txtDesc;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			id = Intent.GetStringExtra ("user");
			SetContentView (Resource.Layout.SearchListPage);
			ImageThree = FindViewById<ImageView> (Resource.Id.ImageThree);
			ImageTwo = FindViewById<ImageView> (Resource.Id.ImageTwo);
			ImageOne = FindViewById<ImageView> (Resource.Id.ImageOne);
			btnMap = FindViewById<Button> (Resource.Id.btnMap);
			txtHeader = FindViewById<TextView> (Resource.Id.txtHeader);
			txtDesc = FindViewById<TextView> (Resource.Id.txtDesc);
			btnMap.Click+= BtnMap_Click;
			LoadPost();
		}


		void BtnMap_Click (object sender, EventArgs e)
		{
			string loc = "geo:" + newContent.latitude + "," + newContent.longitude+"?z=10";
			var geoUri = Android.Net.Uri.Parse (loc);
			var mapIntent = new Intent (Intent.ActionView, geoUri);
			StartActivity (mapIntent);
		}
		public async void LoadPost()
		{

			var mContent = await MainActivity.MobileService.GetTable<Content>().Where(x => x.id == id).ToListAsync();
			newContent = mContent.FirstOrDefault ();
			txtHeader.Text = newContent.topic;
			txtDesc.Text = newContent.desc;
		}
	}
}

