using System;
namespace travelogue.Models
{
	public class User
	{
		public User ()
		{
		}
		//[AutoIncrement, PrimaryKey]
		public string id {
			get;
			set;
		}
		public string username {
			get;
			set;
		}
		public string password {
			get;
			set;
		}
			
		public string name {
			get;
			set;
		}

		public string gender {
			get;
			set;
		}
		public string dob {
			get;
			set;
		}
		public string mobileno {
			get;
			set;
		}
		public string email {
			get;
			set;
		}
        public string about
        {
            get;
            set;
        }
        public string image
        {
            get;
            set;
        }
      
	}
}

