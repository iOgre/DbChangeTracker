using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace Data
{
	public class Customer : BusinessEntity
	{
		public virtual IList<Group> Groups { get; set; }
		public virtual IList<Order> Orders { get; set; }
		public Customer()
		{
			Groups = new List<Group>();
			Orders = new List<Order>();
		}
	}
	public class CustomerMap : ClassMap<Customer>
	{

		public CustomerMap()
		{
			Table("Customer");
			LazyLoad();
			Id(x => x.Id).GeneratedBy.Identity().Column("Id");
			References(x => x.Created).Column("CreatedId");
			References(x => x.Modified).Column("ModifiedId");
			HasMany<Order>(x => x.Orders).KeyColumn("CustomerId").Table("[Order]").ExtraLazyLoad();
			Map(x => x.Name).Column("Name").Not.Nullable();
			HasManyToMany<Group>(x => x.Groups)
				.Table("Customer_Group");



		}
	}
}