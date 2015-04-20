using System;
using System.Reflection;
using Data;
using Domain;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class TestUsageUnitOfWork
	{
		[SetUp]
		public void Setup()
		{
			//UnitOfWork.FluentConfiguration.Mappings(x => x.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()));
			//new SchemaExport(UnitOfWork.Configuration).Execute(false, true, true, false);

		}

		[Test]
		public void CanAddAndRemoveEntityToDb()
		{
			long id;
			using (UnitOfWork.IsStarted ? UnitOfWork.Current : UnitOfWork.Start())
			{
				var logOne = new Log
				{
					ChangeStamp = DateTime.Now,
					ChangedProperties = "changed-properties-one",
					EntityName = "entityName1",
					UserId = 38,
					EntityId = 44
				};
				UnitOfWork.CurrentSession.SaveOrUpdate(logOne);
				UnitOfWork.Current.TransactionalFlush();

			}
		}

	}
}