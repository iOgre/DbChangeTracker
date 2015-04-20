using System;
using NHibernate;

namespace Domain
{
	public interface IUnitOfWork : IDisposable
	{
		void TransactionalFlush();
	}
}