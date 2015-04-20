using System;
using System.Data;
using NHibernate;

namespace Domain
{
	public interface IUnitOfWorkImplementor : IUnitOfWork
	{
		IGenericTransaction BeginTransaction();
		IGenericTransaction BeginTransaction(IsolationLevel isolationLevel);
		void TransactionalFlush(IsolationLevel serializable);
		void TransactionalFlush();
		bool IsInActiveTransaction { get; }
		IUnitOfWorkFactory Factory { get; }
		ISession Session { get; }
		void Flush();

	}
	public class UnitOfWorkImplementor : IUnitOfWorkImplementor
	{
		private readonly IUnitOfWorkFactory _factory;
		private readonly ISession _session;
		public UnitOfWorkImplementor(IUnitOfWorkFactory unitOfWorkFactory, ISession session)
		{
			_factory = unitOfWorkFactory;
			_session = session;
		}
		
		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			_factory.DisposeUnitOfWork(this);
			_session.Dispose();
		}

		#endregion

		public IGenericTransaction BeginTransaction()
		{
			return new GenericTransaction(_session.BeginTransaction());
		}

		public IGenericTransaction BeginTransaction(IsolationLevel isolationLevel)
		{
			return new GenericTransaction(_session.BeginTransaction(isolationLevel));
		}
		public bool IsInActiveTransaction
		{
			get
			{
				return _session.Transaction.IsActive;
			}
		}

		public IUnitOfWorkFactory Factory
		{
			get { return _factory; }
		}

		public ISession Session
		{
			get { return _session; }
		}

		public void Flush()
		{
			_session.Flush();
		}
		public void TransactionalFlush(IsolationLevel isolationLevel)
		{
			IGenericTransaction tx = BeginTransaction(isolationLevel);
			
			try
			{
				//forces a flush of the current unit of work
				tx.Commit();
			}
			catch(Exception ex)
			{
				tx.Rollback();
				throw;
			}
			finally
			{
				tx.Dispose();
			}
		}

		public void TransactionalFlush()
		{
			TransactionalFlush(IsolationLevel.ReadCommitted);
		}
	}
}