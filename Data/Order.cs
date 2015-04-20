using FluentNHibernate.Mapping;

namespace Data
{
	public class Order : BusinessEntity
	{
		public virtual int Amount { get; set; }
		public virtual string OrderContent { get; set; }
	}

	public partial class OrderMap : ClassMap<Order>
	{

		public OrderMap()
		{
			Table("[Order]");
			LazyLoad();
			Id(x => x.Id).GeneratedBy.Identity().Column("Id");
			References(x => x.Created).Column("CreatedId");
			References(x => x.Modified).Column("ModifiedId");
			Map(x => x.Name).Column("Name").Not.Nullable();
			Map(x => x.Amount).Column("Amount").Not.Nullable();
			Map(x => x.OrderContent).Column("OrderContent").Not.Nullable();
		}
	}
}