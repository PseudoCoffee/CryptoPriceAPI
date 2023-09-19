namespace CryptoPriceAPI.Services
{
	public class AggregationService : CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO>
	{
		public CryptoPriceAPI.DTOs.PriceDTO Aggregate(System.Collections.Generic.IEnumerable<CryptoPriceAPI.DTOs.PriceDTO> prices)
		{
			if (null == prices || !prices.Any())
			{
				throw new ArgumentNullException(nameof(prices));
			}

			if (prices.Select(price => price.DateAndHour).Distinct().Count() != 1)
			{
				throw new ArgumentException("Cannot aggregate functions with different timestamps.");
			}

			if (prices.Select(price => price.FinancialInstrumentName).Distinct().Count() != 1)
			{
				throw new ArgumentException("Cannot aggregate functions with different financial instrument name.");
			}

			CryptoPriceAPI.DTOs.PriceDTO priceDTO = new()
			{
				DateAndHour = prices.First().DateAndHour,
				FinancialInstrumentName = prices.First().FinancialInstrumentName,
				ClosePrice = prices.Average(price => price.ClosePrice)
			};

			return priceDTO;
		}
	}
}
