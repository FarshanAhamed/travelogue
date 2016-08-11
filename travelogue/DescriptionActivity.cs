
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
using System.Globalization;
using Microsoft.WindowsAzure.MobileServices;
using Android.Graphics;
using System.Net;

namespace travelogue
{
	[Activity (Label = "DescriptionActivity")]			
	public class DescriptionActivity : Activity
	{
		NewsPage Np = new NewsPage();
		ImageView BtnImage;
		Button btnMap;
		Content newContent;
		TextView txtHeader,txtDesc;
		string mUser,id;
		RatingBar postRating;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			id = Intent.GetStringExtra ("user");


			SetContentView (Resource.Layout.DescriptionPage);
			BtnImage = FindViewById<ImageView> (Resource.Id.ImageOne);
			//BtnImage[1] = FindViewById<ImageView> (Resource.Id.ImageTwo);
			//BtnImage[2] = FindViewById<ImageView> (Resource.Id.ImageThree);
			btnMap = FindViewById<Button> (Resource.Id.btnMap);
			txtHeader = FindViewById<TextView> (Resource.Id.txtHeader);
			txtDesc = FindViewById<TextView> (Resource.Id.txtDesc);
			postRating = FindViewById<RatingBar> (Resource.Id.postRating);
			ActionBar.Hide ();
			btnMap.Click+= BtnMap_Click;
			//mUser = Np.UserReturner ();
			postRating.RatingBarChange+= PostRating_RatingBarChange;
		
			LoadPost();
			}

		async void PostRating_RatingBarChange (object sender, RatingBar.RatingBarChangeEventArgs e)
		{
			float newrating = postRating.Rating;
			if (newContent.rating == null)
				newContent.rating = newrating.ToString("F1");
			else {
				try{
				float currentrating = float.Parse (newContent.rating, CultureInfo.InvariantCulture.NumberFormat);
				newrating = (currentrating + newrating) / 2;

				newContent.rating = newrating.ToString ("F1");
				CurrentPlatform.Init();
				await MainActivity.MobileService.GetTable<Content>().UpdateAsync(newContent);
					postRating.Rating=newrating;
				Toast.MakeText (this, "New rating saved", ToastLength.Short).Show ();
				}
				catch{
					Toast.MakeText (this, "Rating not saved", ToastLength.Short).Show ();
				}
			}
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
			var url1 = newContent.one_image;
			//var url2 = newContent.two_image;
			//var url3 = newContent.three_image;
			if (url1==null) {
				Toast.MakeText (this, "No image", ToastLength.Short).Show ();
			} else {
				var bmp1 = GetImageBitmapFromUrl (url1);
				//var bmp2 = GetImageBitmapFromUrl (url2);
				// bmp3 = GetImageBitmapFromUrl (url3);
				BtnImage.
				SetImageBitmap (bmp1);
				//BtnImage[1].SetImageBitmap (bmp2);
				//BtnImage[2].SetImageBitmap (bmp3);
			}

				

		}

		private Bitmap GetImageBitmapFromUrl(string url)
		{
			Bitmap imageBitmap = null;

			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
				{
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
				}
			}

			return imageBitmap;
		}
	}
}

