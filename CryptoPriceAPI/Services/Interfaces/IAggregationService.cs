namespace CryptoPriceAPI.Services.Interfaces
{
	public interface IAggregationService<DTO> where DTO : class
	{
		public DTO Aggregate(System.Collections.Generic.IEnumerable<DTO> data);
	}
}
