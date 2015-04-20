		namespace Data
{
	/// <summary>
	/// Base class for all business entities to whom Id/Name pair is applicable
	/// </summary>
	public abstract class BusinessEntity : IEntityKey<long>
	{
		/// <summary>
		/// Entity Id
		/// </summary>
		public virtual long Id { get; set; }

		/// <summary>
		/// Entity name
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// Created stamp
		/// </summary>
		public virtual ChangeStamp Created { get; set; }

		/// <summary>
		/// Changed stamp
		/// </summary>
		public virtual ChangeStamp Modified { get; set; }
	}
}