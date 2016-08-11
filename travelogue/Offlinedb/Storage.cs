using System;
using System.Data;
using System.IO;
using SQLite;
namespace travelogue.Offlinedb
{
	 [Table("Storage")]
	class Storage
	{
		[PrimaryKey,AutoIncrement,Column("_Id")]
		public string Id { get; set; }

		[MaxLength(20)]
		public string Name { get; set; }

		[MaxLength(6)]
		public string Gender { get; set; }

		public int Age{ get; set;}
		[MaxLength(10)]
		public string Username{ get; set; }
		public string Password{ get; set;}

		[MaxLength(40)]
		public string Email{ get; set; }
		[MaxLength(10)]
		public string Phone{ get; set; }
	}
	/*[Table("ProfileId")]
	class ProfileId
	{
		[PrimaryKey,AutoIncrement,Column("_Id")]
		public int Id{ get; set; }

		[MaxLength(20)]
		public string NameId{get;set;}

	}*/
}

