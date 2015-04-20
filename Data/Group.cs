using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace Data
{
	public class Group : BusinessEntity
	{
		public virtual IList<Customer> Customers { get; set; }

		public Group()
		{
			Customers = new List<Customer>();
		}
	}

	public  class GroupMap : ClassMap<Group>
	{

		public GroupMap()
		{
			Table("Group");
			LazyLoad();
			Id(x => x.Id).GeneratedBy.Identity().Column("Id");
			References(x => x.Created).Column("CreatedId");
			References(x => x.Modified).Column("ModifiedId");
			Map(x => x.Name).Column("Name").Not.Nullable();
			HasManyToMany<Customer>(x => x.Customers)
				.Table("Customer_Group")
				.Inverse();
		}
	}
}