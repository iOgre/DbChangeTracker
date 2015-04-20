using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Data;
using Domain;
using Domain.Repositories;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Logic;
using NHibernate;
using NHibernate.Linq;

namespace NHibernateTask
{

	class Program
	{
		static void Main(string[] args)
		{
			using(var uow = UnitOfWork.Start())
			{
				using (var session = UnitOfWork.CurrentSession)
				{
					using (var transaction = session.BeginTransaction())
					{
						session.Query<Log>();
					}
				}
				var readRepository = new Repository<long, Log>(false);
				var logEntries = readRepository.All().ToList();
				foreach(var logEntry in logEntries)
				{
					Console.WriteLine(logEntry.Id + " " + logEntry.EntityName);
				}
			}
			Console.ReadLine();
		}

		private static ISessionFactory CreateSessionFactory()
		{
			var configure = Fluently.Configure()
				.Database(MsSqlConfiguration.MsSql2008.ConnectionString(t => t.FromAppSetting("connectionString")))
				.Mappings(x => x.FluentMappings.AddFromAssembly(Assembly.Load("Data")));
			
			var sessionFactory = configure
				.BuildSessionFactory();

			return sessionFactory;
		}
	}

	

}
