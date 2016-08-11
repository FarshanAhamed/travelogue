using System;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Globalization;
using travelogue.Models;
using Android.Preferences;
using Android.Graphics;
using System.IO;
//using static Android.Support.V7.App.ActionBar;
//using static Android.Support.V7.App.ActionBar.LayoutParams;
//using static Android.Support.V7.Widget.Toolbar;
using System.IO.Compression;
using System.Net;
using System.Threading.Tasks;


namespace travelogue
{
    [Activity (Label = "ProfileActivity")]			
	public class ProfileActivity : Activity
	{TextView Name, Gender, Age, About;
		
        ImageButton EditProfile; 
		ImageView pic;
		LinearLayout MainLayout;
		RelativeLayout ProgressbarHolder;

		protected override async void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
            
            
            ActionBar.SetDisplayShowHomeEnabled(false);
			ActionBar.SetDisplayShowTitleEnabled(false);

	

			ActionBar.SetCustomView(Resource.Layout.action_bar);
			ActionBar.SetDisplayShowCustomEnabled(true);


			SetContentView (Resource.Layout.ProfilePage);



			Name = FindViewById<TextView> (Resource.Id.txtName);
			Gender = FindViewById<TextView> (Resource.Id.txtGender);
			Age = FindViewById<TextView> (Resource.Id.txtAge);
			About = FindViewById<TextView> (Resource.Id.txtAbout);
		//	Photos = FindViewById<Button> (Resource.Id.btnPhotos);
            EditProfile=FindViewById<ImageButton>(Resource.Id.btnEditProfile);
			LinearLayout newsbtn = FindViewById<LinearLayout> (Resource.Id.newsbtn);
			LinearLayout homebtn = FindViewById<LinearLayout> (Resource.Id.homebtn);
			LinearLayout settingsbtn = FindViewById<LinearLayout> (Resource.Id.settingsbtn);
			LinearLayout searchbtn = FindViewById<LinearLayout> (Resource.Id.searchbtn);
			LinearLayout aboutbtn = FindViewById<LinearLayout> (Resource.Id.aboutbtn);
			ProgressbarHolder = FindViewById<RelativeLayout>(Resource.Id.progressBarHolder);
			MainLayout = FindViewById<LinearLayout>(Resource.Id.MainLayout);
			//pbar = FindViewById<ProgressBar> (Resource.Id.progressBar);
			pic = FindViewById<ImageView> (Resource.Id.imageView1);


			await LoadData ();



            EditProfile.Click += EditProfile_Click;
            settingsbtn.Click += Settingsbtn_Click;
            aboutbtn.Click+= Aboutbtn_Click;
			homebtn.Click+= Homebtn_Click;
			newsbtn.Click += Newsbtn_Click;
			searchbtn.Click+= Searchbtn_Click;
			//Photos.Click+= Photos_Click;
			}
		public override void OnBackPressed()
		{
			//do nothing
		}
        private void EditProfile_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ProfileEditPage));
        }

        void Pic_Click (object sender, EventArgs e)
		{
			//do nothing
		}

		int ConvertDOB(string dob){

			var now = DateTime.Now;
			var yr = DateTime.ParseExact (dob, "d/M/yyyy", CultureInfo.InvariantCulture);
			return (now.Year - yr.Year);
		}
		public async Task LoadData(){
			ProgressbarHolder.Visibility = ViewStates.Visible;
			MainLayout.Visibility = ViewStates.Gone;
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            var log =    prefs.GetBoolean("LoggedIn",false);
            var users = prefs.GetString("user","");
			try{
            if (log && users != "")
            {

                var user = await MainActivity.MobileService.GetTable<User>().Where(x => x.username == users).ToListAsync();
                var loggeduser = user.FirstOrDefault();
                this.Name.Text = loggeduser.name;
                this.Gender.Text = loggeduser.gender;
                this.Age.Text = (ConvertDOB(loggeduser.dob)).ToString();
				this.About.Text = loggeduser.about;
				var result = loggeduser.image;
				if (result == null) {
					Toast.MakeText (this, "No image to show", ToastLength.Short).Show ();
				} else {
					Bitmap bmp =await Helper.GetImageBitmapFromUrl (result);
					pic.SetImageBitmap (bmp);


				}
				ProgressbarHolder.Visibility = ViewStates.Gone;
				MainLayout.Visibility=ViewStates.Visible;
            }
            else {
                Toast.MakeText(this, "Not Logged in", ToastLength.Short).Show();
				StartActivity (typeof(LogInActivity));
            }
			}
			catch{
				Toast.MakeText (this, "Connection Error", ToastLength.Short).Show ();
			}

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

		void Newsbtn_Click (object sender, EventArgs e)
		{
			StartActivity (typeof(NewsPage));
		}
		void Searchbtn_Click (object sender, EventArgs e)
		{
			StartActivity (typeof(SearchPageActivity));
		}

		void Photos_Click (object sender, EventArgs e)
		{
            StartActivity(typeof(PhotoGridActivity));
		}

	}
}

