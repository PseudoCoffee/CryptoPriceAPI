using System;

namespace CryptoPriceAPI.Services
{
	public class AverageService : CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO>
	{

		private readonly Microsoft.Extensions.Logging.ILogger<CryptoPriceAPI.Services.AverageService> _logger;

		public AverageService(Microsoft.Extensions.Logging.ILogger<CryptoPriceAPI.Services.AverageService> logger)
		{
			_logger = logger;
		}

		public CryptoPriceAPI.DTOs.PriceDTO? Aggregate(System.Collections.Generic.IEnumerable<CryptoPriceAPI.DTOs.PriceDTO?> prices)
		{
			_logger.LogInformation("Aggregate({@0})", prices);

			if (null == prices)
			{
				ArgumentNullException exception = new($"{nameof(prices)}");
				_logger.LogError(exception, "{@0}", exception.Message);

				throw exception;
			}

			System.Collections.Generic.IEnumerable<CryptoPriceAPI.DTOs.PriceDTO> notNullPrices = prices.Where(price => null != price).Select(price => price!);

			CryptoPriceAPI.DTOs.PriceDTO? aggregatedPriceDTO = null;

			if (notNullPrices.Any())
			{
				if (notNullPrices.Select(price => price.DateAndHour.DateTime).Distinct().Count() > 1)
				{
					ArgumentException exception = new("Cannot aggregate prices with different timestamps.");
					_logger.LogError(exception, "{@0}", exception.Message);

					throw exception;
				}

				if (notNullPrices.Select(price => price.FinancialInstrument).Distinct().Count() > 1)
				{
					ArgumentException exception = new("Cannot aggregate prices with different financial instrument name.");
					_logger.LogError(exception, "{@0}", exception.Message);

					throw exception;
				}

				_logger.LogInformation("Aggregating {@0} prices", notNullPrices.Count());

				aggregatedPriceDTO = new()
				{
					DateAndHour = notNullPrices.First().DateAndHour,
					FinancialInstrument = notNullPrices.First().FinancialInstrument,
					ClosePrice = notNullPrices.Average(price => price.ClosePrice)
				};
			}

			return aggregatedPriceDTO;
		}
	}
}
