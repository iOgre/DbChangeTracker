using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Logic
{
	[DataContract]
	public class AuditTrail
	{
		[DataMember]
		public string EntityName { get; set; }

		/// <summary>
		/// Returns value of property with specified type
		/// </summary>
		/// <typeparam name="T">type of property</typeparam>
		/// <param name="propertyName">property name</param>
		/// <returns>value of type (T)</returns>
		public T GetProperty<T>(string propertyName)
		{
			var valueToReturn = GetChangedPropertyBase(propertyName);
			var result = (T) valueToReturn.NewValue;
			return result;
		}

		private ChangedPropertyBase GetChangedPropertyBase(string propertyName)
		{
			ChangedPropertyBase valueToReturn;
			if (!ChangedPropertyDictionary.TryGetValue(propertyName, out valueToReturn))
			{
				throw new ArgumentException("there is no Property named " + propertyName + " in class " + EntityName);
			}
			return valueToReturn;
		}

		/// <summary>
		/// returns value of property
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public object GetProperty(string propertyName)
		{
			var valueToReturn = GetChangedPropertyBase(propertyName);
			var result = valueToReturn.NewValue;
			return result;
		}

		public AuditTrail()
		{
			ChangedProperties = new List<ChangedProperty>();
		}

		[DataMember]
		public List<ChangedProperty> ChangedProperties { get; set; }

		[NonSerialized]
		private Dictionary<string, ChangedPropertyBase> _dictionary;

		//this helps to avoid .Single() instructions when obtaining value by key
		public Dictionary<string, ChangedPropertyBase> ChangedPropertyDictionary { 
			get
			{
				return _dictionary ??
				       (_dictionary = ChangedProperties.ToDictionary(key => key.Name, value => new ChangedPropertyBase
					       {
						       NewValue = value.NewValue,
						       OldValue = value.OldValue
					       }));
			}
		} 
	}
}