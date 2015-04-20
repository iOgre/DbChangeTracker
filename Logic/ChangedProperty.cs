using System.Runtime.Serialization;

namespace Logic
{
	[DataContract]
	public class ChangedPropertyBase
	{
		[DataMember]
		public object OldValue { get; set; }
		[DataMember]
		public object NewValue { get; set; }
	}

	[DataContract]
	public class ChangedProperty : ChangedPropertyBase
	{
		[DataMember]
		public string Name { get; set; }
	}
}