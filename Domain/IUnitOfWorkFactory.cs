using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;

namespace Domain
{
	public interface IUnitOfWorkFactory
	{
		IUnitOfWork Create();
		void DisposeUnitOfWork(UnitOfWorkImplementor adapter);
		ISession CurrentSession { get; set; }
		FluentConfiguration FluentConfiguration { get; }
		Configuration Configuration { get; }
		ISessionFactory  SessionFactory { get;}
	}
}
