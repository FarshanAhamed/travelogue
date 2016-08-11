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

namespace Bunky_1._0
{
    [Activity(Label = "Subjects", Theme = "@style/CustomActionBarTheme")]
    public class Subjects : Activity
    {
        private List<Subj> mItems;
        private ListView mListView;

        public IEnumerable<object> table { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetDisplayShowTitleEnabled(false);
            ActionBar.SetCustomView(Resource.Layout.action_bar);
            ActionBar.SetDisplayShowCustomEnabled(true);


            SetContentView(Resource.Layout.Subjects);

            Button AdderBut = FindViewById<Button>(Resource.Id.BtnAddSubject);
            mListView = FindViewById<ListView>(Resource.Id.ListSubjects);
            mItems = new List<Subj>();            

            SubjectList adapter = new SubjectList(this, mItems);

            mListView.Adapter = adapter;
            AdderBut.Click += AdderBut_Click;
        }

        private void AdderBut_Click(object sender, EventArgs e)
        {
            EditText Name = FindViewById<EditText>(Resource.Id.TxtSubjNameField);
            if (Name.Text.Length > 0)
            {
                mItems.Add(new Subj() { SubName = Name.Text });
                Name.Text = "";
                Name.Hint = "Name";

                DBRepository dbr = new DBRepository();
                dbr.InsertSubj(Name.Text);
            }
        }
    }
}