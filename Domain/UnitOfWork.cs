using System;
using Domain.Repositories;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;

namespace Domain
{
	public static class UnitOfWork
	{
		/// <summary>
		/// starts new unit of work
		/// </summary>
		/// <returns>Unit of work</returns>
		public static IUnitOfWork Start()
		{
			if (CurrentUnitOfWork != null)
			{
				throw new InvalidOperationException("You cannot start more than one unit of work at the same time.");
			}
			var unitOfWork = _unitOfWorkFactory.Create();
			CurrentUnitOfWork = unitOfWork;
			return unitOfWork;
		}

		/// <summary>
		/// StartDirty starts new UOW or takes existing
		/// </summary>
		/// <returns></returns>
		public static IUnitOfWork StartDirty()
		{
			if(IsStarted)
			{
				return Current;
			}
			return Start();
		}

		public const string CurrentUnitOfWorkKey = "CurrentUnitOfWork.Key";

		private static IUnitOfWorkFactory _unitOfWorkFactory = new UnitOfWorkFactory();
		private static IUnitOfWork _innerUnitOfWork;

		private static IUnitOfWork CurrentUnitOfWork
		{
			get { return Local.Data[CurrentUnitOfWorkKey] as IUnitOfWork; }
			set { Local.Data[CurrentUnitOfWorkKey] = value; }
		}

		public static IUnitOfWork Current
		{
			get
			{
				if (CurrentUnitOfWork == null)
				{
					throw new InvalidOperationException("You are not in a unit of work.");
				}
				return CurrentUnitOfWork;
			}
		}

		/// <summary>
		/// Returns current session
		/// </summary>
		public static ISession CurrentSession
		{
			get { return _unitOfWorkFactory.CurrentSession; }
			internal set { _unitOfWorkFactory.CurrentSession = value; }
		}

		/// <summary>
		/// Checks if UOW was already started
		/// </summary>
		public static bool IsStarted
		{
			get { return CurrentUnitOfWork != null; }
		}

		public static FluentConfiguration FluentConfiguration
		{
			get { return _unitOfWorkFactory.FluentConfiguration; }
		}
		public static void DisposeUnitOfWork(IUnitOfWorkImplementor unitOfWork)
		{
			CurrentUnitOfWork = null;
		}
	}
}
