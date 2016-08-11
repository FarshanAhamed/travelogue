using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using System.Drawing;
using System.IO;
using travelogue.Models;

namespace travelogue
{
	class NewsList : BaseAdapter<Content>
	{
		private List<Content> mItems;
		private Context mContext;
		public NewsList(Context context, List<Content> items)
		{
			mItems = items;
			mContext = context;
		}
		public override int Count
		{
			get
			{
				return mItems.Count;
			}
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override Content this[int position]
		{
			get
			{
				return mItems[position];
			}
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View row = convertView;
			if (row == null)
			{
				row = LayoutInflater.From(mContext).Inflate(Resource.Layout.news_list, null, false);
			}
			TextView txtPlace= row.FindViewById<TextView>(Resource.Id.txtPlace);
            ImageView imgPlace = row.FindViewById<ImageView>(Resource.Id.imgPlace);
            TextView txtRating = row.FindViewById<TextView>(Resource.Id.txtRating);
            txtPlace.Text = mItems[position].place;
			//imgPlace.setim=mItems[
			//imgPlace.SetImageURI (mItems [position].one_image);
            //imgPlace.SetImageBitmap(ByteToImage(mItems[position].Image));
            if (mItems[position].rating == null)
            {
                txtRating.Visibility = ViewStates.Gone;
            }
            else
            {
                txtRating.Visibility = ViewStates.Visible;
                txtRating.Text = mItems[position].rating + "/5";
            }
            return row;
		}
        public static Bitmap ByteToImage(byte[] blob)
        {
            Bitmap bmm = BitmapFactory.DecodeByteArray(blob, 0, blob.Length);
            return bmm;
        }



    }
}