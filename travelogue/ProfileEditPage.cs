using System;
using System.Linq;
using System.Text;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Globalization;
using travelogue.Models;
using Android.Preferences;
using System.Threading.Tasks;
using Android.Graphics;
using Microsoft.WindowsAzure.MobileServices;
using System.IO.Compression;
using Android.Views.InputMethods;
using Android.Provider;

namespace travelogue
{
    [Activity(Label = "ProfileEditPage")]
    public class ProfileEditPage : Activity
    {
		string dob;
        User loggeduser;
		Stream stream;
		string absolutepath,urlinserver=null;
	//	byte[] picData;
        EditText txtName, Year, Month, Day, txtAbout;
        RadioGroup GenderSelected;
	//	MemoryStream memStream;
        RadioButton Male, Female;
		ImageView btnImage;
		LinearLayout MainLayout;
		RelativeLayout ProgressbarHolder;
        Button btnSave, btnCancel;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ProfileEditLayout);
            txtName = FindViewById<EditText>(Resource.Id.txtName);
            ActionBar.Hide();

            Year = FindViewById<EditText>(Resource.Id.txtYear);
            Month = FindViewById<EditText>(Resource.Id.txtMonth);
            Day = FindViewById<EditText>(Resource.Id.txtDay);
            GenderSelected = FindViewById<RadioGroup>(Resource.Id.Gender);
            Female = FindViewById<RadioButton>(Resource.Id.FemaleCheck);
            Male = FindViewById<RadioButton>(Resource.Id.MaleCheck);
            txtAbout = FindViewById<EditText>(Resource.Id.txtAbout);
            btnImage = FindViewById<ImageView>(Resource.Id.btnImage);
            btnSave = FindViewById<Button>(Resource.Id.btnSave);
            btnCancel = FindViewById<Button>(Resource.Id.btnCancel);
			ProgressbarHolder = FindViewById<RelativeLayout>(Resource.Id.progressBarHolder);
			MainLayout = FindViewById<LinearLayout>(Resource.Id.MainLayout);
			ProgressbarHolder.Visibility = ViewStates.Visible;
			MainLayout.Visibility = ViewStates.Gone;
           await LoadData();
			ProgressbarHolder.Visibility = ViewStates.Gone;
			MainLayout.Visibility = ViewStates.Visible;
			// Create your application here
            btnSave.Click += BtnSave_Click;
			btnCancel.Click+= BtnCancel_Click;

            btnImage.Click += BtnImage_Click;
        }

        void BtnCancel_Click (object sender, EventArgs e)
        {
			StartActivity (typeof(ProfileActivity));
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
			InputMethodManager inputManager = (InputMethodManager) this.GetSystemService(Context.InputMethodService);


			inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
			MainLayout.Visibility = ViewStates.Gone;
			ProgressbarHolder.Visibility = ViewStates.Visible;
            string Gender;
            var SexId = GenderSelected.CheckedRadioButtonId;
            if (SexId == Male.Id)
            {
                Gender = "Male";
            }
            else
            {
                Gender = "Female";
            }

          
                try
                {

                    dob = Day.Text + "/" + Month.Text + "/" + Year.Text;



					loggeduser.name = txtName.Text;
					loggeduser.dob = this.dob;
					loggeduser.gender = Gender;
					loggeduser.about = txtAbout.Text;
					loggeduser.image = urlinserver;

                    
                    CurrentPlatform.Init();
				await MainActivity.MobileService.GetTable<User>().UpdateAsync(loggeduser);
				StartActivity(typeof(ProfileActivity));

                }
                catch
                {
					ProgressbarHolder.Visibility = ViewStates.Gone;
					MainLayout.Visibility = ViewStates.Visible;
                    Toast.MakeText(this, "Connection Error", ToastLength.Short).Show();
                }

            }
        

        private void BtnImage_Click(object sender, EventArgs e)
		{
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(
                Intent.CreateChooser(imageIntent, "Select photo"), 0);
        }

        protected async override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                
                stream = ContentResolver.OpenInputStream(data.Data);
               
                btnImage.SetImageURI(data.Data);
				absolutepath = GetPathToImage (data.Data);
				byte[] bytes = System.IO.File.ReadAllBytes(absolutepath);
				try{
				urlinserver =  await Helper.Upload(bytes);
				}
				catch{
				}
				 }
        }

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


       public async Task LoadData()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            var log = prefs.GetBoolean("LoggedIn", false);
            var users = prefs.GetString("user", "");
            if (log && users != "")
            {
                var user = await MainActivity.MobileService.GetTable<User>().Where(x => x.username == users).ToListAsync();
                loggeduser = user.FirstOrDefault();
                this.txtName.Text = loggeduser.name;
                DOB Date = new DOB(loggeduser.dob);
                this.Day.Text = Date.DD;
                this.Month.Text = Date.MM;
                this.Year.Text = Date.YYYY;
                string Gender = loggeduser.gender;
                if (Gender == "Male")
                {
                    GenderSelected.Check(Resource.Id.MaleCheck);
                }
                else
                {
                    GenderSelected.Check(Resource.Id.FemaleCheck);
                }
                this.txtAbout.Text = loggeduser.about;
				var res = loggeduser.image;
				if (!(res==null)){
					Bitmap bmp =await Helper.GetImageBitmapFromUrl (res);
					btnImage.SetImageBitmap (bmp);
				}
            }
            
        }
       public class DOB
        {
           public string DD, MM, YYYY;
            private string dob;

            public DOB(string dob)
            {
                this.dob = dob;
                var yr = DateTime.ParseExact(dob, "d/M/yyyy", CultureInfo.InvariantCulture);
                DD = yr.Day.ToString();
                MM = yr.Month.ToString();
                YYYY = yr.Year.ToString();
            }

           
        }
    }
}