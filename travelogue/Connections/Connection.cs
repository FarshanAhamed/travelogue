using System;
using travelogue.Models;
using Android.Widget;
using System.Threading.Tasks;


namespace travelogue
{
	//public class TravelDB
	//{

	//	SQLiteAsyncConnection Connection {get;}
	//	public static string Root { get; set; } = String.Empty;
	//	public TravelDB()
	//	{
	//		var location = "travel.db";
	//		location = System.IO.Path.Combine (System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), location);
	//		Connection = new SQLiteAsyncConnection (location);
	//		Connection.CreateTableAsync<User>();

	//	}

	 
		async public Task<int> RegisterUser(User user)
		{
			var result = await Connection.InsertAsync(user);
			return result;
		}

		async public Task<User> LoadUserData(){
			var result = await Connection.Table<User> ().FirstOrDefaultAsync ();
			return result;
		}




	}
}

