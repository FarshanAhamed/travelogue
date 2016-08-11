
using System;
using Android.App;
using Android.OS;
using Android.Views;
using System.IO;

using Android.Widget;
using Plugin.Geolocator;
using travelogue.Models;
using Android.Content;
using Android.Preferences;
using System.Linq;
using Plugin.Geolocator.Abstractions;
using Microsoft.WindowsAzure.MobileServices;
using Android.Graphics;
using Android.Views.InputMethods;
using System.Collections.Generic;
using Android.Provider;


namespace travelogue
{
    [Activity(Label = "AddNewsActivity")]
    public class AddNewsActivity : Activity
    {


	//	string paths;
		//Stream stream;
		EditText Place, Description,Topic,Addrlat,Addrlon;
		string urlinserver;
		ImageView ImageSpace;

		public List<ImageView> image;
        Button btnAdd, btnDiscard, GetAddress;
		LinearLayout MainLayout;
        RelativeLayout progressBarHolder;
        public Position position=null;
		int PLACE_PICKER_REQUEST = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddNewsLayout);
            ActionBar.Hide();
            Place = FindViewById<EditText>(Resource.Id.txtPlace);
			Topic = FindViewById<EditText> (Resource.Id.txtTopic);
			Addrlat = FindViewById<EditText> (Resource.Id.txtAddrlat);
			Addrlon = FindViewById<EditText> (Resource.Id.txtAddrlon);
			Description = FindViewById<EditText>(Resource.Id.txtDesc);
 			ImageSpace = FindViewById<ImageView>(Resource.Id.ImageOne);
		//	image[1] = FindViewById<ImageView>(Resource.Id.ImageTwo);
		//	image[2] = FindViewById<ImageView>(Resource.Id.ImageThree);
            GetAddress = FindViewById<Button>(Resource.Id.btnGetAdd);
            btnAdd = FindViewById<Button>(Resource.Id.btnSave);
            btnDiscard = FindViewById<Button>(Resource.Id.btnCancel);
            MainLayout = FindViewById<LinearLayout>(Resource.Id.MainLayout);
            progressBarHolder = FindViewById<RelativeLayout>(Resource.Id.progressBarHolder);

            btnDiscard.Click += BtnDiscard_Click;
            btnAdd.Click += BtnAdd_Click;
            GetAddress.Click += GetAddress_Click;
			ImageSpace.Click += BtnImage_Click;
			//image[1].Click += BtnImage_Click;
			//image[2].Click+= BtnImage_Click;


        }

        private async void GetAddress_Click(object sender, EventArgs e)
        {

        try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                Toast.MakeText(this, "Location saved", ToastLength.Long).Show();

            }
            catch
            {
                Toast.MakeText(this, "Location Error", ToastLength.Short).Show();
            }

			/*var builder = new PlacePicker.IntentBuilder();
			StartActivityForResult(builder.Build(this), PLACE_PICKER_REQUEST);

			 PLACE_PICKER_REQUEST = 1;
			PlacePicker.IntentBuilder builder = new PlacePicker.IntentBuilder();*/

			//Context context = getApplicationContext();
		//	StartActivityForResult(builder.Build(this), PLACE_PICKER_REQUEST);
        }




        async void BtnAdd_Click(object sender, EventArgs e)
        {
		//to hide keyboard
			InputMethodManager inputManager = (InputMethodManager) this.GetSystemService(Context.InputMethodService);

			inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);

            User loggeduser;
           
            MainLayout.Visibility = ViewStates.Gone;
            progressBarHolder.Visibility = ViewStates.Visible;

			//connection to database
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            var log = prefs.GetBoolean("LoggedIn", false);
            var users = prefs.GetString("user", "");
            if (log && users != "")
            {
				//var res = await Helper.UploadImage (stream);
                var userr = await MainActivity.MobileService.GetTable<User>().Where(x => x.username == users).ToListAsync();
                loggeduser = userr.FirstOrDefault();


                try {

					//image path set
					if(!(urlinserver==null))
					{
						var lat=position.Latitude.ToString();


						var lon=position.Longitude.ToString();
						if(!(lat==null||lon==null))
						{

					// new object to database
                    Content newuser = new Content()
                    {
                        place = Place.Text,

                        latitude = position.Latitude.ToString(),
                        longitude = position.Longitude.ToString(),

                        userid = loggeduser.username,
                        desc = Description.Text,
						one_image=urlinserver,

						topic=Topic.Text,
					
                    };
						}
						else
						{
							Content newuser = new Content()
							{
								place = Place.Text,
								userid = loggeduser.username,
								latitude=Addrlat.Text,
								longitude=Addrlon.Text,
								desc = Description.Text,
								one_image=urlinserver,

							};
						
                    CurrentPlatform.Init();
                    await MainActivity.MobileService.GetTable<Content>().InsertAsync(newuser);
						}
                    MainLayout.Visibility = ViewStates.Visible;
                    progressBarHolder.Visibility = ViewStates.Gone;
                    StartActivity(typeof(NewsPage));
                }
				}
                catch 
                {
                    Toast.MakeText(this, "Connection error, try again later", ToastLength.Short).Show();
                }
            }
        }

        void BtnDiscard_Click (object sender, EventArgs e)
		{
			StartActivity(typeof(NewsPage));
		}
		private void BtnImage_Click(object sender, EventArgs e)
		{
			var imageIntent = new Intent (Intent.ActionPick);
			imageIntent.SetType ("image/*");
			imageIntent.PutExtra (Intent.ExtraAllowMultiple,true);
			imageIntent.SetAction (Intent.ActionGetContent);
			StartActivityForResult (Intent.CreateChooser (imageIntent, "Select Picture"), 0);
		}

		protected async override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
		{
			base.OnActivityResult (requestCode, resultCode, intent);
			bool braked = false;
			string path = "";

			try {
				if (resultCode == Result.Ok) {
					//paths = new List<string> ();
					if (intent != null) {
						ClipData clipData = intent.ClipData;
						if (clipData != null) {

							for (int i = 0; i < clipData.ItemCount; i++) {

								if (i > 2) {
									braked = true;
									break;
								}
								ClipData.Item item = clipData.GetItemAt (i);
								Android.Net.Uri uri = item.Uri;

								//In case you need image's absolute path
								path = GetPathToImage (uri);
								ImageSpace.SetImageURI(uri);
								byte[] bytes = System.IO.File.ReadAllBytes(path);
								urlinserver = await Helper.Upload(bytes);
								//paths.Add (urlinserver);}
						}

					} else {
						Android.Net.Uri uri = intent.Data;
						path = GetPathToImage(uri);
						ImageSpace.SetImageURI(uri);
						byte[] bytes = System.IO.File.ReadAllBytes(path);
						urlinserver = await Helper.Upload(bytes);

						//paths.Add (urlinserver);
					}
					//MessagingCenter.Send<IGestureListener, List<string>> (this, "ImagesPath", paths);

					if (braked == true) {
						Toast.MakeText (this, " image will be uploaded", ToastLength.Long).Show ();
					}

					}

				}
			
			}//Send the paths to forms
		catch (Exception ex) {

			Toast.MakeText (this, "Unable to open, error:" + ex.ToString(), ToastLength.Long).Show ();
		}
	
		}
		/*private Bitmap DecodeBitmapFromStream (Android.Net.Uri data, int requestedWidth, int requestedHeight)
		{
			//Decode with InJustDecodeBounds = true to check dimensions
			Stream stream = ContentResolver.OpenInputStream(data);
			BitmapFactory.Options options = new BitmapFactory.Options();
			options.InJustDecodeBounds = true;
			BitmapFactory.DecodeStream(stream);

			//Calculate InSamplesize
			options.InSampleSize = CalculateInSampleSize(options, requestedWidth, requestedHeight);

			//Decode bitmap with InSampleSize set
			stream = ContentResolver.OpenInputStream(data); //Must read again
			options.InJustDecodeBounds = false;
			Bitmap bitmap = BitmapFactory.DecodeStream(stream, null, options);
			return bitmap;
		}*/
	/*	private int CalculateInSampleSize (BitmapFactory.Options options, int requestedWidth, int requestedHeight)
		{
			//Raw height and widht of image
			int height = options.OutHeight;
			int width = options.OutWidth;
			int inSampleSize = 1;

			if (height > requestedHeight || width > requestedWidth)
			{
				//the image is bigger than we want it to be
				int halfHeight = height / 2;
				int halfWidth = width / 2;

				while((halfHeight / inSampleSize) > requestedHeight && (halfWidth / inSampleSize) > requestedWidth)
				{
					inSampleSize *= 2;
				}

			}

			return inSampleSize;
		}*/
		private string GetPathToImage(Android.Net.Uri uri)
		{
			string doc_id = "";
			using (var c1 = ContentResolver.Query (uri, null, null, null, null)) {
				c1.MoveToFirst ();
				string document_id = c1.GetString (0);
				doc_id = document_id.Substring (document_id.LastIndexOf (":") + 1);
			}

			string path = null;

			// The projection contains the columns we want to return in our query.
			string selection = MediaStore.Images.Media.InterfaceConsts.Id + " =? ";
			using (var cursor = ManagedQuery(MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] {doc_id}, null))
			{
				if (cursor == null) return path;
				var columnIndex = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);
				cursor.MoveToFirst();
				path = cursor.GetString(columnIndex);
			}
			return path;
		}


	}
}

