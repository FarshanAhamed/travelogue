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
    class SubjectList : BaseAdapter<Subj>
    {
        private List<Subj> mItems;
        private Context mContext;
        public SubjectList(Context context, List<Subj> items)
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
        public override Subj this[int position]
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.Subjects_Lists, null, false);
            }
            TextView TxtName = row.FindViewById<TextView>(Resource.Id.textSubjName);
            TextView TxtNameFirst = row.FindViewById<TextView>(Resource.Id.txtSubFirst);
            TxtName.Text = mItems[position].SubName;
            string FirstSubjLett = mItems[position].SubName;
            var firstCharOfString = FirstSubjLett[0];
            string FirstLetter = Convert.ToString(firstCharOfString);
            TxtNameFirst.Text = FirstLetter;

            return row;
        }
    }
}