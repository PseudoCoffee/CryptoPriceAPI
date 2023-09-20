namespace CryptoPriceAPI.Controllers
{
	[Microsoft.AspNetCore.Mvc.ApiController]
	public class OHLCPriceController : Microsoft.AspNetCore.Mvc.ControllerBase
	{
		private readonly Microsoft.Extensions.Logging.ILogger<OHLCPriceController> _logger;
		private readonly CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO> _aggregationService;

		private readonly System.Collections.Generic.IEnumerable<CryptoPriceAPI.Services.Interfaces.ICryptoService> _externalServices;

		public OHLCPriceController(
			Microsoft.Extensions.Logging.ILogger<OHLCPriceController> logger,
			CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO> aggregationService,
			System.Collections.Generic.IEnumerable<CryptoPriceAPI.Services.Interfaces.ICryptoService> externalServices
			)
		{
			_aggregationService = aggregationService ?? throw new ArgumentNullException(nameof(aggregationService));

			if (null == externalServices || !externalServices.Any()) 
			{
				throw new InvalidOperationException($"{nameof(externalServices)} is null or contains no elements.");
			}
			_externalServices = externalServices;
			
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <summary>
		/// Get close candle price of BTCUSD from <paramref name="dateOnly"/> at end of <paramref name="hour"/> over the span of an hour aggregated from multiple sources.
		/// </summary>
		/// <param name="dateOnly">Date of the candle price</param>
		/// <param name="hour">Starting hour of the candle price</param>
		/// <returns></returns>
		[Microsoft.AspNetCore.Mvc.HttpGet]
		[Microsoft.AspNetCore.Mvc.Route("GetCandleClosePrice")]
		public async Task<CryptoPriceAPI.DTOs.PriceDTO> GetCandleClosePriceAsync(System.DateOnly dateOnly, [System.ComponentModel.DataAnnotations.Range(0, 23)] System.Int32 hour)
		{
			_logger.LogInformation("GetCandleClosePrice({@0}, {@1})", dateOnly, hour);

			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(dateOnly, hour);

			System.Collections.Generic.List<CryptoPriceAPI.DTOs.PriceDTO> prices = new();

			foreach (CryptoPriceAPI.Services.Interfaces.ICryptoService externalService in _externalServices)
			{
				prices.Add(await externalService.GetPriceAsync(dateAndHour));
			}

			return _aggregationService.Aggregate(prices);
		}
	}
}
