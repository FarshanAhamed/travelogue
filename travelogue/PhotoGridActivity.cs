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

namespace travelogue
{
    [Activity(Label = "PhotoGridActivity")]
    public class PhotoGridActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PhotoGrid);
            GridView gridview = FindViewById<GridView>(Resource.Id.gridView1);
            gridview.Adapter = new ImageAdapter(this);
            // Create your application here
        }
    }
}