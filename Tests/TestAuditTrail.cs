using System;
using System.Linq;
using Logic;
using NUnit.Framework;

namespace Tests
{
	class TestClass
	{
		public int Id { get; set; }
		public long LongValue { get; set; }
		public DateTime? DateTimeNullable { get; set; }
		public string TextField { get; set; }
		public double RealValue { get; set; }
	}
	[TestFixture]
	public class TestAuditTrail
	{
		private TestClass _first;
		private TestClass _second;

		[SetUp]
		public void Setup()
		{
			_first = new TestClass
				{
					Id = 13,
					DateTimeNullable = null,
					LongValue = 3933,
					TextField = "this is first",
					RealValue = 4.34
				};

			_second = new TestClass
			{
				Id = 123,
				DateTimeNullable = DateTime.Parse("10/10/10"),
				LongValue = 393322,
				TextField = "this is second",
				RealValue = 7.484
			};

		}

		[Test]
		public void CanPopulateChanges()
		{
			string xmlWithChanges = _first.CompareChanges(_second);
			//convert xmlWithChanges into object
			AuditTrail auditTrail = xmlWithChanges.DeserializeFromString<AuditTrail>();
			Assert.AreEqual("Tests.TestClass", auditTrail.EntityName);
			Assert.AreEqual(5, auditTrail.ChangedProperties.Count);
			Assert.AreEqual(4.34, auditTrail.ChangedProperties.First(t => t.Name == "RealValue").OldValue);
			Assert.AreEqual(13, auditTrail.ChangedProperties.First(t => t.Name == "Id").OldValue);
			Assert.AreEqual(3933, auditTrail.ChangedProperties.First(t => t.Name == "LongValue").OldValue);
			Assert.AreEqual("this is first", auditTrail.ChangedProperties.First(t => t.Name == "TextField").OldValue);

			Assert.AreEqual(7.484, auditTrail.ChangedProperties.First(t => t.Name == "RealValue").NewValue);
			Assert.AreEqual(123, auditTrail.ChangedProperties.First(t => t.Name == "Id").NewValue);
			Assert.AreEqual(393322, auditTrail.ChangedProperties.First(t => t.Name == "LongValue").NewValue);
			Assert.AreEqual("this is second", auditTrail.ChangedProperties.First(t => t.Name == "TextField").NewValue);

			//trying to access property
			Assert.AreEqual(7.484, auditTrail.GetProperty<double>("RealValue"));

		}
	}
}