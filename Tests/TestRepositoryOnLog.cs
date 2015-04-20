using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Domain;
using Domain.Repositories;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class TestRepositoryOnLog
	{
		[TearDown]
		public void TearDown()
		{
			//here we will remove added item from db
			using (UnitOfWork.Start())
			{
				var repository = new Repository<long, Log>();
				IQueryable<Log> toDelete = repository.FilterBy(t => t.UserId == -100);
				repository.Delete(toDelete.ToList());
			}
		}
		[Test]
		public void CustomerRepository()
		{
			using(UnitOfWork.Start())
			{
				var repository = new Repository<long, Customer>();
				var customer = repository.FindBy(1);
				//IEnumerable<Order> p = customer.Orders.Where(t => t.Amount == 8);
				//var z = customer.Orders.Count(t => t.Amount == 8);
				customer.Orders.Count();
				//Assert.AreEqual(8, p);
				Assert.IsNotNull(customer);
				Assert.AreEqual(1, customer.Id);

			}
		}

		[Test]
		public void CanAddLog()
		{
			using (UnitOfWork.Start())
			{
				var repository = new Repository<long, Log>();
				var dt = DateTime.Now;
				for(var i = 0 ; i < 10; i++)
				{
					var logEntry = new Log()
					{
						ChangeStamp = dt,
						EntityId = -199,
						EntityName = Guid.NewGuid().ToString() +" TestEntityId",
						ChangedProperties = "TestChangedProperties",
						UserId = -100
					};
					repository.Add(logEntry);

				}
				
			

			}
		}
	}
}