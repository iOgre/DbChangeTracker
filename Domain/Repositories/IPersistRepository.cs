using System.Collections.Generic;

namespace Domain.Repositories
{
	public interface IPersistRepository<TEntity> where TEntity : class
	{
		bool Add(TEntity entity);
		bool Add(IEnumerable<TEntity> items);
		bool Delete(TEntity entity);
		bool Delete(IEnumerable<TEntity> entities);
	}
}