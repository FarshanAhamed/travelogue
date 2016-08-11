
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using travelogue.Models;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using Android.Views.InputMethods;

namespace travelogue
{
	[Activity (Label = "SignUpActivity")]			
	public class SignUpActivity : Activity
	{
		LinearLayout MainLayout;
		RelativeLayout ProgressbarHolder;
		EditText txtFirst, txtLast,Day,Year,Month,Email, Phone, Pass1, Pass2,username;
		RadioGroup GenderSelected;
		RadioButton Female, Male;
		string Gender,Name,dob;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
            ActionBar.Hide();
            // Create your application here
            SetContentView(Resource.Layout.SignUpPage);
			txtFirst = FindViewById<EditText> (Resource.Id.txtFirst);
			username = FindViewById<EditText> (Resource.Id.txtUser);
			txtLast = FindViewById<EditText> (Resource.Id.txtLast);
			Year = FindViewById<EditText> (Resource.Id.txtYear);
			Month = FindViewById<EditText> (Resource.Id.txtMonth);
			Day = FindViewById<EditText> (Resource.Id.txtDay);
			GenderSelected = FindViewById<RadioGroup> (Resource.Id.Gender);
			Female = FindViewById<RadioButton> (Resource.Id.FemaleCheck);
		    Male = FindViewById<RadioButton> (Resource.Id.MaleCheck);
			Email = FindViewById<EditText> (Resource.Id.txtEmail);
			Phone = FindViewById<EditText> (Resource.Id.txtPhone);
			Pass1 = FindViewById<EditText> (Resource.Id.txtPass1);
			Pass2 = FindViewById<EditText> (Resource.Id.txtPass2);
			ProgressbarHolder = FindViewById<RelativeLayout>(Resource.Id.progressBarHolder);
			MainLayout = FindViewById<LinearLayout>(Resource.Id.MainLayout);
			Button btnSignUp = FindViewById<Button> (Resource.Id.btnSignUp);
			btnSignUp.Click+= BtnSignUp_Click;
			//CalendarView Date = FindViewById<CalendarView> (Resource.Id.calendarView1);
			ProgressbarHolder.Visibility = ViewStates.Gone;
			MainLayout.Visibility = ViewStates.Visible;
		}

		 async void BtnSignUp_Click (object sender, EventArgs e)
		{
			InputMethodManager inputManager = (InputMethodManager) this.GetSystemService(Context.InputMethodService);

			inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);


			var SexId = GenderSelected.CheckedRadioButtonId;
			if (SexId == Male.Id) {
				Gender ="Male";
			} else {
				Gender = "Female";
			}
		
            
            if (txtFirst.Text == "" || txtLast.Text == "" || Day.Text == "" || Month.Text == "" || Year.Text == ""
				||username.Text==""|| Email.Text == "" || Phone.Text == "" || Pass1.Text == "" || Pass2.Text == "") {
				
						

				Toast.MakeText(this, "Fill all the fields", ToastLength.Short).Show();
            } 
			if(Pass1.Text != Pass2.Text) {
                Toast.MakeText(this, "Passwords does not match.", ToastLength.Short).Show();

            }

			MainLayout.Visibility = ViewStates.Gone;
			ProgressbarHolder.Visibility = ViewStates.Visible;
			CurrentPlatform.Init();
			var check1 = await MainActivity.MobileService.GetTable<User>().Where(x => x.username == username.Text).ToListAsync();
            if(check1.Count==1)
                {
				ProgressbarHolder.Visibility = ViewStates.Gone;
				MainLayout.Visibility = ViewStates.Visible;
                Toast.MakeText(this, "Username already exists! \n Try a diiferent username", ToastLength.Short).Show();
            }
            else {
                try
                {
                    Name = txtFirst.Text + " " + txtLast.Text;
                    dob = Day.Text + "/" + Month.Text + "/" + Year.Text;
                    User myuser = new User()
                    {
                        username = username.Text,
                        password = Pass1.Text,
                        name = Name,
                        dob = this.dob,
                        email = Email.Text,
                        gender = Gender,
                        mobileno = Phone.Text,
						about="Write about you..",
                        
                    };
                    CurrentPlatform.Init();
					 await MainActivity.MobileService.GetTable<User>().InsertAsync(myuser);
                    //	var Connection = new TravelDB ();
                    //	var result = await Connection.RegisterUser (myuser);
					//if (result > 0) {
					//	Toast.MakeText(this,"Sign Up successful",ToastLength.Long).Show();
					
					StartActivity(typeof(LogInActivity));
                }
                catch
                {
                    Toast.MakeText(this, "Error in Registering", ToastLength.Short).Show();
                }
           
           
            }  


		}

	}
	}