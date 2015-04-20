using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Data;
using Domain;
using Domain.Repositories;
using NHibernate.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using MockRepository = Rhino.Mocks.MockRepository;

namespace Tests
{
	[TestFixture]
    public class IUnitOfWorkTests
	{
		private readonly MockRepository _mocks = new MockRepository();
		[Test]
		public void can_start_uow()
		{
			var factory = _mocks.DynamicMock<IUnitOfWorkFactory>();
			var unitOfWork = _mocks.DynamicMock<IUnitOfWork>();

			var fieldInfo = typeof (UnitOfWork).GetField("_unitOfWorkFactory",
			                                             BindingFlags.Static | BindingFlags.SetField | BindingFlags.NonPublic);
			fieldInfo.SetValue(null, factory);
			using(_mocks.Record())
			{
				Expect.Call(factory.Create()).Return(unitOfWork);
			}
			using(_mocks.Playback())
			{
				
			}
		}

		[Test]
		public void zzz()
		{
			using (var uow = UnitOfWork.Start())
			{
				using (var session = UnitOfWork.CurrentSession)
				{
					using (var transaction = session.BeginTransaction())
					{
						var logss = session.Query<Log>().ToList();
					}
				}
				var readRepository = new Repository<long, Log>(false);
				var logEntries = readRepository.All().ToList();
				foreach (var logEntry in logEntries)
				{
					Console.WriteLine(logEntry.Id + " " + logEntry.EntityName);
				}
			}
		}

	}
}
