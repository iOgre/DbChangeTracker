using System;
using FluentNHibernate.Mapping;

namespace Data
{
	/// <summary>
	/// Class contains change stamp information, i.e. user details, date & time, etc
	/// </summary>
	public class ChangeStamp : BusinessEntity
	{
		public virtual long? UserId { get; set; }
		public virtual string UserName { get; set; }
		public virtual DateTime? Timestamp { get; set; }
	}
	public class ChangeStampMap : ClassMap<ChangeStamp>
	{

		public ChangeStampMap()
		{
			Table("ChangeStamp");
			LazyLoad();
			Id(x => x.Id).GeneratedBy.Identity().Column("Id");
			Map(x => x.UserId).Column("UserId");
			Map(x => x.UserName).Column("UserName").Not.Nullable();
			Map(x => x.Timestamp).Column("Timestamp");
			
		}
	}
	}