
using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Microsoft.WindowsAzure.MobileServices;
using travelogue.Models;
using Android.Preferences;
using Android.Views.InputMethods;

namespace travelogue
{
    [Activity (Label = "LogInActivity")]			
	public class LogInActivity : Activity
	{
        EditText username, password;
        RelativeLayout ProgressbarHolder;
        LinearLayout MainLayout;
        Button btnSignUp, btnLogin, btnFbLogin,btnSearch;
        
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
            ActionBar.Hide();
            SetContentView(Resource.Layout.LoginPage);
            // Create your application here

            Toast.MakeText(this, "Login to Continue", ToastLength.Short).Show();

            btnSignUp = FindViewById<Button> (Resource.Id.btnSignUp);
            btnLogin = FindViewById<Button> (Resource.Id.btnLogin);
			btnFbLogin = FindViewById<Button> (Resource.Id.btnFbLogin);
			btnSearch = FindViewById<Button> (Resource.Id.btnSearch);
            username = FindViewById<EditText>(Resource.Id.txtUser);
            password = FindViewById<EditText>(Resource.Id.txtPass);
            ProgressbarHolder = FindViewById<RelativeLayout>(Resource.Id.progressBarHolder);
            MainLayout = FindViewById<LinearLayout>(Resource.Id.MainLayout);
			btnLogin.Click += BtnLogin_Click;
			btnSignUp.Click+= BtnSignUp_Click;
			btnSearch.Click+= BtnSearch_Click;
            ProgressbarHolder.Visibility = ViewStates.Gone;
        }

		void BtnSearch_Click (object sender, EventArgs e)
		{
			StartActivity (typeof(SearchPageActivity));
		}

      



        async void BtnLogin_Click(object sender, EventArgs e)
        {

			InputMethodManager inputManager = (InputMethodManager) this.GetSystemService(Context.InputMethodService);

			inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);  

           if (username.Text == "" || password.Text == "")
            {
                Toast.MakeText(this, "Fill all the fields", ToastLength.Short).Show();
            }
            else
            {
                MainLayout.Visibility = ViewStates.Gone;
                ProgressbarHolder.Visibility = ViewStates.Visible;
                CurrentPlatform.Init();
                var checkuser = await MainActivity.MobileService.GetTable<User>().Where(x => x.username == username.Text).ToListAsync();
                if (checkuser.Count == 1)
                {
                    //username found
                    if (checkuser[0].password == password.Text)
                    {
                        Toast.MakeText(this, "Successfully logged in", ToastLength.Short).Show();
                        ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                        ISharedPreferencesEditor editor = prefs.Edit();
                        editor.PutString("user", checkuser[0].username);
                        editor.PutBoolean("LoggedIn", true);
                        editor.Apply();        // applies changes asynchronously on newer APIs 

                        StartActivity(typeof(ProfileActivity));
                    }
                    else
                    {
						ProgressbarHolder.Visibility = ViewStates.Gone;
                        MainLayout.Visibility = ViewStates.Visible;
                         Toast.MakeText(this, "Username/password is incorrect", ToastLength.Long).Show();

                    }
                }
                else
                {
                    MainLayout.Visibility = ViewStates.Visible;
                    ProgressbarHolder.Visibility = ViewStates.Gone;
                    Toast.MakeText(this, "Username/password is incorrect", ToastLength.Long).Show();

                }
            }
        }
        public override void OnBackPressed()
        {
            //do nothing
        }
    		void BtnSignUp_Click (object sender, EventArgs e)
		{
			StartActivity (typeof(SignUpActivity));
		}











        //		const string FacebookAppId = "592462364251092";
        //
        //		// You must get this token authorizing by either using Facebook App or a WebView.
        //		// Please review included samples.
        //		string userToken;
        //
        //		void PostToMyWall ()
        //		{
        //			FacebookClient fb = new FacebookClient (userToken);
        //			string myMessage = "Hello from Xamarin";
        //
        //			fb.PostTaskAsync ("me/feed", new { message = myMessage }).ContinueWith (t => {
        //				if (!t.IsFaulted) {
        //					string message = "Great, your message has been posted to you wall!";
        //					Console.WriteLine (message);
        //				}
        //			});
        //		}
        //
        //
        //		void PrintFriendsNames ()
        //		{
        //			// This uses Facebook Query Language
        //			// See https://developers.facebook.com/docs/technical-guides/fql/ for more information.
        //			var query = string.Format("SELECT uid,name,pic_square FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1={0}) ORDER BY name ASC", "me()");
        //			FacebookClient fb = new FacebookClient (userToken);
        //
        //			fb.GetTaskAsync ("fql", new { q = query }).ContinueWith (t => {
        //				if (!t.IsFaulted) {
        //					var result = (IDictionary<string, object>)t.Result;
        //					var data = (IList<object>)result["data"];
        //					var count = data.Count;
        //					var message = string.Format ("You have {0} friends", count);
        //					Console.WriteLine (message);
        //
        //					foreach (IDictionary<string, object> friend in data)
        //						Console.WriteLine ((string) friend["name"]);
        //				}
        //			});



      /*  public override void OnBackPressed()
        {
            
          new Android.Support.V7.App.AlertDialog.Builder(this).SetTitle("Are you sure to exit the app?").SetPositiveButton(Android.Resource.String.Yes, onClickYes).SetNegativeButton(Android.Resource.String.No, onClickNo).Show();
        }

        private void onClickNo(object sender, DialogClickEventArgs e)
        {
            //Do Nothing
        }
        private void onClickYes(object sender, DialogClickEventArgs e)
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }*/

    }

}








	


