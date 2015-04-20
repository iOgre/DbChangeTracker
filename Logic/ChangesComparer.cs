using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Data;

namespace Logic
{
	public static class ChangesComparer
    {
		/// <summary>
		/// Compares changes between two objects of same type, and returns diff as xml
		/// </summary>
		/// <typeparam name="T">type of parameters, used for compile-time checking of type equality</typeparam>
		/// <param name="beforeChanges">object of type T before changes</param>
		/// <param name="afterChanges">object of type T after changes</param>
		/// <returns>xml string that contains changed properties</returns>
		public static string CompareChanges<T>(this T beforeChanges, T afterChanges) where T: class
		{
			if(beforeChanges == null)
			{
				throw new ArgumentNullException("beforeChanges");
			}
			if (afterChanges == null)
			{
				throw new ArgumentNullException("afterChanges");
			}
			var beforeType = beforeChanges.GetType();
			var afterType = afterChanges.GetType();
			if(beforeType != afterType)
			{
				throw new ArgumentException("parameters should be of one type");
			}
			//now obtain public properties of primitive, or string, or DateTime or generic types
			
			var properties = GetProperties<T>(beforeType);
			var changesInfo = new AuditTrail
				{
					EntityName = beforeType.FullName,
				};
			foreach (var property in properties)
			{
				Type propType = property.PropertyType;
				if(propType.IsGenericType)
				{
					propType = propType.GetGenericArguments()[0];
				}
				var beforeValue =  property.GetValue(beforeChanges);
				var afterValue = property.GetValue(afterChanges);
				string beforeValueString;
				string afterValueString;
				//compares with null, because it's possible to work with nullable fields
				if(propType == typeof(DateTime))
				{
					beforeValueString = beforeValue != null ? ((DateTime) beforeValue).ToString("F") : null;
					afterValueString = afterValue != null ? ((DateTime)afterValue).ToString("F") : null;
				} else
				{
					beforeValueString = beforeValue != null ? beforeValue.ToString() : null;
					afterValueString = afterValue != null ? afterValue.ToString() : null;
				}
				if(beforeValueString != afterValueString)
				{
					changesInfo.ChangedProperties.Add(new ChangedProperty
						{
							Name = property.Name,
							NewValue = afterValue,
							OldValue = beforeValue
						});
				}
			}
			if(changesInfo.ChangedProperties.Count > 0)
			{
				var result = changesInfo.SerializeToString();
				return result;
			}
			return string.Empty;

		}

	    private static IEnumerable<PropertyInfo> GetProperties<T>(Type beforeType) where T : class
	    {
		    var publicProperties =
			    beforeType.GetProperties(BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public);
		    var properties =
			    publicProperties.Where(
				    t => t.PropertyType.IsPrimitive || t.PropertyType.IsGenericType || 
						t.PropertyType == typeof (string) || t.PropertyType == typeof (DateTime)).
				    ToList();
		    return properties;
	    }
    }

	public static class SerializationHelper
	{
		/// <summary>
		/// Serializes object into xml string
		/// </summary>
		/// <param name="obj">object to serialize</param>
		/// <returns>xml string</returns>
		public static string SerializeToString(this object obj)
		{
			var serializer = new DataContractSerializer(obj.GetType());
			using (StringWriter writer = new StringWriter())
			{
				using(var xmlWriter = new XmlTextWriter(writer))
				{
					xmlWriter.Formatting = Formatting.Indented;
					serializer.WriteObject(xmlWriter, obj);
					writer.Flush();
					return writer.ToString();
				}
			}
		}

		/// <summary>
		/// Deserializes xml string into object of given type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static T DeserializeFromString<T>(this string xml) where T : class
		{
			var serializer = new DataContractSerializer(typeof(T));
			using (TextReader t = new StringReader(xml))
			{
				using(var xmlReader = new XmlTextReader(t))
				{
					T result = serializer.ReadObject(xmlReader) as T;
					return result;
				}
			}
		}
	}
	
}
