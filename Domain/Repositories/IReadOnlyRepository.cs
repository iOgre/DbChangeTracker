using System;
using System.Linq;
using System.Linq.Expressions;
using Data;
using NHibernate.Persister.Entity;

namespace Domain.Repositories
{
	public interface IReadOnlyRepository<TKey, TEntity> where TEntity : class, IEntityKey<TKey>
	{
		IQueryable<TEntity> All();
		TEntity FindBy(Expression<Func<TEntity, bool>> expression);
		IQueryable<TEntity> FilterBy(Expression<Func<TEntity, bool>> expression);
		TEntity FindBy(TKey id);
	}
}
