using System;
using NHibernate;

namespace Domain
{
	class GenericTransaction : IGenericTransaction
	{
		private ITransaction _transaction;
		public GenericTransaction(ITransaction transaction)
		{
			_transaction = transaction;
		}
		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			_transaction.Dispose();
		}

		#endregion

		#region Implementation of IGenericTransaction

		public void Commit()
		{
			_transaction.Commit();
		}

		public void Rollback()
		{
			_transaction.Rollback();
		}

		#endregion

	}
}