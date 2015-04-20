using System.Data;
using Domain;
using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests
{
	[TestFixture]
	public class UnitOfWorkImplementor_Fixture
	{
		private readonly MockRepository _mocks = new MockRepository();
		private IUnitOfWorkFactory _factory;
		private ISession _session;
		private IUnitOfWorkImplementor _uow;

		[SetUp]
		public void Setup()
		{
			_factory = _mocks.DynamicMock<IUnitOfWorkFactory>();
			_session = _mocks.DynamicMock<ISession>();
		}

		[Test]
		public void Can_Dispose_UnitOfWorkImplementor()
		{
			using (_mocks.Record())
			{
				Expect.Call(() => _factory.DisposeUnitOfWork(null)).IgnoreArguments();
				Expect.Call(_session.Dispose);
			}
			using (_mocks.Playback())
			{
				_uow = new UnitOfWorkImplementor(_factory, _session);
				_uow.Dispose();
			}
		}

		[Test]
		public void CanBeginTransaction()
		{
			using (_mocks.Record())
			{
				Expect.Call(_session.BeginTransaction()).Return(null);
			}
			using (_mocks.Playback())
			{
				_uow = new UnitOfWorkImplementor(_factory, _session);
				var transaction = _uow.BeginTransaction();
				Assert.IsNotNull(transaction);
			}
		}

		[Test]
		public void CanBeginTransactionWithIsolationLevel()
		{
			var isolationLevel = IsolationLevel.Serializable;
			using(_mocks.Record())
			{
				Expect.Call(_session.BeginTransaction(isolationLevel)).Return(null);
			}
			using(_mocks.Playback())
			{
				_uow = new UnitOfWorkImplementor(_factory, _session);
				var transaction = _uow.BeginTransaction(isolationLevel);
				Assert.IsNotNull(transaction);
			}
		}

		[Test]
		public void CanExecuteTransactionFlushWithIsolationLevel()
		{
			var tx = _mocks.StrictMock<ITransaction>();
			var session = _mocks.DynamicMock<ISession>();
			SetupResult.For(session.BeginTransaction(IsolationLevel.Serializable)).Return(tx);
			 _uow = _mocks.PartialMock<UnitOfWorkImplementor>(_factory, session);
			using(_mocks.Record())
			{
				Expect.Call(tx.Commit);
				Expect.Call(tx.Dispose);
			}
			using(_mocks.Playback())
			{
				_uow.TransactionalFlush(IsolationLevel.Serializable);
			}
		}
	}
}