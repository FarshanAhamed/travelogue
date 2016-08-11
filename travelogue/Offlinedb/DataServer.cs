using System;
using System.IO;
using System.Data;
using SQLite;
using travelogue.Offlinedb;

namespace travalogue.Offlinedb
{
	public class DataServer
	{

		private static SQLiteAsyncConnection ConnectionDb(string path)
		{
			var conn = new SQLite.SQLiteAsyncConnection (path);
			return conn;
		}
		public async void Create()
		{
			string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),"travalogue.db3");

			var result = await ConnectionDb (dbPath).CreateTableAsync<Storage> ();


//			var db = new SQLiteConnection(dbPath);
//			 
//			try
//			{
//				
//				db.CreateTable<Storage>();
//			//	db.CreateTable<ProfileId>();
//							}
//			catch(Exception ex)
//			{
//				//return "Error :" + ex.Message;
//			}

		}
		public async void AddData(string Name,int Age,string Gender,string Email,String Phone,String Password){
			try{
				string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "travalogue.db3");

				Storage item=new Storage();
				//item.NameId=Name;
				item.Name=Name;
				item.Age=Age;
				item.Gender=Gender;
				item.Email=Email;
				item.Phone=Phone;
				item.Password=Password;

				var result = await ConnectionDb(dbPath).InsertAsync(item);
				//int id=1;

			/*	var obj = db.Get<ProfileId>(id);
				obj.NameId=Name;
				db.Update(obj);*/

			}
			catch(Exception ex){
				
			}
		}
		public string GetName()
		{
			try{
				string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),"travalogue.db3");
				var db = new SQLiteConnection(dbPath);

				var table = db.Table<Storage>();
				string output = "11";
				foreach (var item in table)
				{ output = "";
					output = item.Name;
				}
				return output;
			
			}
			catch(Exception ex){
				return "Error:" + ex.Message;
			}
		}
	/*	public string GetNameId(int id){
			try{
				string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),"travalogue.db3");
				var db = new SQLiteConnection(dbPath);
				//var item = db.Get<ProfileId>(id);
				return item.NameId;

			}
			catch(Exception ex){
				return "Error: "+ex.Message;
			}		
		}*/
	}
}

