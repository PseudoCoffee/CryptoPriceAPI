namespace CryptoPriceAPI.Services.Interfaces
{
	public interface IAggregationService<DTO> where DTO : class
	{
		/// <summary>
		/// Aggregate multiple <paramref name="data"/> into a single <typeparamref name="DTO"/>.
		/// </summary>
		/// <param name="data"> Data to be aggregated. </param>
		/// <returns> An aggregated result of all <paramref name="data"/>. </returns>
		public DTO? Aggregate(System.Collections.Generic.IEnumerable<DTO?> data);
	}
}
