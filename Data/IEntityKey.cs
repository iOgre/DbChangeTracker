namespace Data
{
	public interface IEntityKey<TKey>
	{
		TKey Id { get; }
	}
}