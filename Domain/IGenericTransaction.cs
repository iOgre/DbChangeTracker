using System;

namespace Domain
{
	public interface IGenericTransaction : IDisposable
	{
		void Commit();
		void Rollback();

	}
}