using System;
using System.IO;
using System.Reflection;
using System.Xml;
using Data;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;

namespace Domain
{
	public class UnitOfWorkFactory : IUnitOfWorkFactory
	{
		internal UnitOfWorkFactory()
		{
			//_configuration = null;
		}
		private static ISession _currentSession;
		private ISessionFactory _sessionFactory;
		private Configuration _configuration;
		private FluentConfiguration _fluentConfiguration;
		private const string Default_HibernateConfig = "hibernate.cfg.xml";
		#region Implementation of IUnitOfWorkFactory

		public IUnitOfWork Create()
		{
			ISession session = CreateSession();
			session.FlushMode = FlushMode.Commit;
			_currentSession = session;
			return new UnitOfWorkImplementor(this, session);
		}

		public void DisposeUnitOfWork(UnitOfWorkImplementor adapter)
		{
			CurrentSession = null;
			UnitOfWork.DisposeUnitOfWork(adapter);
		}

		private ISession CreateSession()
		{
			return SessionFactory.OpenSession();
		}

		public ISession CurrentSession
		{
			get
			{
				if(_currentSession == null)
				{
					throw new InvalidOperationException("you are not in a unit of work");
				}
				return _currentSession;
			}
			set { _currentSession = value; }
		}
		public FluentConfiguration FluentConfiguration
		{
			get
			{
				if (_configuration == null)
				{
					string hibernateConfig = Default_HibernateConfig;
					//if not rooted, assume path from base directory
					if (Path.IsPathRooted(hibernateConfig) == false)
						hibernateConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, hibernateConfig);
					if (File.Exists(hibernateConfig))
					{
						_configuration = new Configuration();
						_configuration.Configure(new XmlTextReader(hibernateConfig));
						_fluentConfiguration = Fluently.Configure(_configuration)
						.Mappings(x => x.FluentMappings.AddFromAssemblyOf<BusinessEntity>()); ;
					}
					//.Database(MsSqlConfiguration.MsSql2008.ConnectionString(t => t.FromAppSetting("connectionString")))


				}
				return _fluentConfiguration;
			}
		}

		public Configuration Configuration { get; private set; }

		public ISessionFactory SessionFactory
		{
			get
			{
				if (_sessionFactory == null)
				{
					_sessionFactory = FluentConfiguration.BuildSessionFactory();
				}
				return _sessionFactory;
			}
		}


		#endregion
	}
}