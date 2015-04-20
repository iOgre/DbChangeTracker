using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using NHibernate;
namespace Data
{
	public class Log : IEntityKey<long>
	{
		public virtual long Id { get; set; }
		public virtual string EntityName { get; set; }
		public virtual int EntityId { get; set; }
		public virtual System.DateTime ChangeStamp { get; set; }
		public virtual int UserId { get; set; }
		public virtual string ChangedProperties { get; set; }

		}
	public  class LogMap : ClassMap<Log>
	{

		public LogMap()
		{
			Table("Log");
			LazyLoad();
			Id(x => x.Id).GeneratedBy.Identity().Column("Id");
			Map(x => x.EntityName).Column("EntityName").Not.Nullable();
			Map(x => x.EntityId).Column("EntityId").Not.Nullable();
			Map(x => x.ChangeStamp).Column("ChangeStamp").Not.Nullable();
			Map(x => x.UserId).Column("UserId").Not.Nullable();
			Map(x => x.ChangedProperties).Column("ChangedProperties").Not.Nullable();
		}
	}
}
