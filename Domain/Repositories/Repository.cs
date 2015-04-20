using System;
using System.Linq;
using Data;
using NHibernate;
using NHibernate.Linq;

namespace Domain.Repositories
{
	public class Repository<TKey, T> : IPersistRepository<T>,
	                                   IReadOnlyRepository<TKey, T> where T : class, IEntityKey<TKey>
	{
		protected virtual ISession Session
		{
			get { return UnitOfWork.CurrentSession; }
		}

		private Boolean _performLog;
		public	Repository(bool performLog = false)
		{
			_performLog = performLog;
		}
		protected virtual ISessionFactory SessionFactory
		{
			get { return UnitOfWork.CurrentSession.GetSessionImplementation().Factory; }
		}

		public bool Add(T entity)
		{
			Session.Save(entity);
			return true;
		}

		public bool Add(System.Collections.Generic.IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				Session.Save(item);
			}
			return true;
		}


		public bool Update(T entity, bool logIt = false)
		{
			if(logIt)
			{
				if(!entity.Id.Equals(default(TKey)))
				{
					var oldValue = FindBy(entity.Id);
					
				}
			}
			Session.Update(entity);
			return true;
		}

		public bool Delete(T entity)
		{
			Session.Delete(entity);
			return true;
		}

		public bool Delete(System.Collections.Generic.IEnumerable<T> entities)
		{
			foreach (T entity in entities)
			{
				Session.Delete(entity);
			}
			return true;
		}

		public IQueryable<T> All()
		{
			return Session.Query<T>();
		}

		public T FindBy(System.Linq.Expressions.Expression<Func<T, bool>> expression)
		{
			return FilterBy(expression).SingleOrDefault();
		}

		public IQueryable<T> FilterBy(System.Linq.Expressions.Expression<Func<T, bool>> expression)
		{
			return All().Where(expression).AsQueryable();
		}

		public T FindBy(TKey id)
		{
			return Session.Get<T>(id);
		}
	}
}