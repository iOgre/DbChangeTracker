using System;
using Domain;
using NHibernate;
using NHibernate.Engine;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class UnitOfWorkFactoryFixture
	{
		private IUnitOfWorkFactory _factory;

		[SetUp]
		public void Setup()
		{
			_factory = (IUnitOfWorkFactory) Activator.CreateInstance(typeof (UnitOfWorkFactory), true);
		}

		[Test]
		public void CanCreateUOW()
		{
			IUnitOfWork implementor = _factory.Create();
			Assert.IsNotNull(implementor);
			Assert.IsNotNull(_factory.CurrentSession);
			Assert.AreEqual(FlushMode.Commit, _factory.CurrentSession.FlushMode);
		
		}

		[Test]
		public void Can_configure_NHibernate()
		{
			var configuration = _factory.FluentConfiguration.BuildConfiguration();
			Assert.IsNotNull(configuration);
			Assert.AreEqual("NHibernate.Connection.DriverConnectionProvider",
							configuration.Properties["connection.provider"]);
			Assert.AreEqual("NHibernate.Dialect.MsSql2008Dialect",
							configuration.Properties["dialect"]);
			Assert.AreEqual("NHibernate.Driver.SqlClientDriver",
							configuration.Properties["connection.driver_class"]);
			Assert.AreEqual("Data Source=localhost;Database=Logging;Integrated Security=SSPI;",
							configuration.Properties["connection.connection_string"]);
		}

		[Test]
		public void Can_create_and_access_session_factory()
		{
			var sessionFactory = _factory.SessionFactory;
			Assert.IsNotNull(sessionFactory);
			Assert.AreEqual("NHibernate.Dialect.MsSql2008Dialect", (sessionFactory as ISessionFactoryImplementor).Dialect.ToString());
		}
		[Test]
		public void AccessingCurrentSessionIfNoOpenedSessionThrows()
		{
			Assert.Throws<InvalidOperationException>(() =>
				{
					var session = _factory.CurrentSession;
				}
				);
		}
	}
}