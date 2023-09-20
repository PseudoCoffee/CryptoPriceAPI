namespace CryptoPriceAPI.Services
{
	public class AverageService : CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO>
	{
		public CryptoPriceAPI.DTOs.PriceDTO Aggregate(System.Collections.Generic.IEnumerable<CryptoPriceAPI.DTOs.PriceDTO> prices)
		{
			if (null == prices)
			{
				throw new ArgumentNullException($"{nameof(prices)}");
			}

			if (!prices.Any())
			{
				throw new ArgumentException($"{nameof(prices)} contains no elements.");
			}

			if (prices.Select(price => price.DateAndHour.DateTime).Distinct().Count() != 1)
			{
				throw new ArgumentException("Cannot aggregate prices with different timestamps.");
			}

			if (prices.Select(price => price.FinancialInstrument).Distinct().Count() != 1)
			{
				throw new ArgumentException("Cannot aggregate prices with different financial instrument name.");
			}

			CryptoPriceAPI.DTOs.PriceDTO priceDTO = new()
			{
				DateAndHour = prices.First().DateAndHour,
				FinancialInstrument = prices.First().FinancialInstrument,
				ClosePrice = prices.Average(price => price.ClosePrice)
			};

			return priceDTO;
		}
	}
}
