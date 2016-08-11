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

namespace travelogue.Models
{
    class Content
    {
        public string id
        {
            get;
            set;
        }
        public string userid
        {
            get;
            set;
        }
        public string place
        {
            get;
            set;
        }
       
        public string desc
        {
            get;
            set;
        }
        public string one_image
        {
            get;
            set;
        }
        public string two_image
        {
            get;
            set;
        }
        public string three_image
        {
            get;
            set;
        }
        public string rating
        {
            get;
            set;
        }
        public string latitude
        {
            get;
            set;
        }
        public string longitude
        {
            get;
            set;
        }
		public string topic 
		{
			get;
			set;
		}
    }
}