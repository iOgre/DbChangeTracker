using System;
using System.Reflection;
using Domain;
using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests
{
	[TestFixture]
	public class UnitOfWorkWithFactoryFixture
	{
		private readonly MockRepository _mocks = new MockRepository();
		private IUnitOfWorkFactory _factory;
		private IUnitOfWork _unitOfWork;
		private ISession _session;
		[SetUp]
		public void Setup()
		{
			_factory = _mocks.DynamicMock<IUnitOfWorkFactory>();
			_unitOfWork = _mocks.DynamicMock<IUnitOfWork>();
			_session = _mocks.DynamicMock<ISession>();
			PrepareFactory();
			_mocks.BackToRecordAll();
			SetupResult.For(_factory.Create()).Return(_unitOfWork);
			SetupResult.For(_factory.CurrentSession).Return(_session);
			_mocks.ReplayAll();
		}

		private void PrepareFactory()
		{
			var fieldInfo = typeof (UnitOfWork).GetField("_unitOfWorkFactory",
			                                             BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);
			fieldInfo.SetValue(null, _factory);
		}

		[TearDown]
		public void TearDown()
		{
			_mocks.VerifyAll();
			// assert that the UnitOfWork is reset
			var fieldInfo = typeof(UnitOfWork).GetField("_innerUnitOfWork",
				BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);
			fieldInfo.SetValue(null, null);
		}

		[Test]
		public void StartingUOWIfAlreadyStartedThrowsAnException()
		{
			UnitOfWork.Start();
			Assert.Throws<InvalidOperationException>(() => UnitOfWork.Start());
		}

		[Test]
		public void CanAccessCurrentUOW()
		{
			IUnitOfWork uow = UnitOfWork.Start();
			var current = UnitOfWork.Current;
			Assert.AreSame(uow, current);
		}
		[Test]
		public void AccessingCurrentUOWIfNotStartedThrows()
		{
				Assert.Throws<InvalidOperationException>(() =>
					{
						var cur = UnitOfWork.Current;
					});
			
		}

		[Test]
		public void IsUOWStarted()
		{
			Assert.IsFalse(UnitOfWork.IsStarted);
			var uow = UnitOfWork.Start();
			Assert.IsTrue(UnitOfWork.IsStarted);
		}

		[Test]
		public void CanGetSessionIfUOWStarted()
		{
			using(UnitOfWork.Start())
			{
				var session = UnitOfWork.CurrentSession;
				Assert.IsNotNull(session);
			}
		}

		
		
	}
}