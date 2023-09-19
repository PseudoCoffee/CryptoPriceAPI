namespace CryptoPriceAPI.Controller
{
	[Microsoft.AspNetCore.Mvc.ApiController]
	public class OHLCPriceController : Microsoft.AspNetCore.Mvc.ControllerBase
	{
		private readonly System.Collections.Generic.IEnumerable<CryptoPriceAPI.Services.Interfaces.ICryptoService> _externalServices;
		private readonly CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO> _aggregationService;

		private readonly Microsoft.Extensions.Logging.ILogger<OHLCPriceController> _logger;

		public OHLCPriceController(
			CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO> aggregationService,
			Microsoft.Extensions.Logging.ILogger<OHLCPriceController> logger,
			System.Collections.Generic.IEnumerable<CryptoPriceAPI.Services.Interfaces.ICryptoService> externalServices
			)
		{
			_aggregationService = aggregationService ?? throw new ArgumentNullException(nameof(aggregationService));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));

			if (null == externalServices || !externalServices.Any()) 
			{
				throw new InvalidOperationException($"{nameof(externalServices)} is null or contains no elements.");
			}

			_externalServices = externalServices;
		}

		/// <summary>
		/// Get close candle price of BTCUSD from <paramref name="dateOnly"/> at end of <paramref name="hour"/> over the span of an hour aggregated from multiple sources.
		/// </summary>
		/// <param name="dateOnly">Date of the candle price</param>
		/// <param name="hour">Starting hour of the candle price</param>
		/// <returns></returns>
		[Microsoft.AspNetCore.Mvc.HttpGet]
		[Microsoft.AspNetCore.Mvc.Route("GetCandleClosePrice")]
		public async Task<CryptoPriceAPI.DTOs.PriceDTO> GetCandleClosePrice(System.DateOnly dateOnly, [System.ComponentModel.DataAnnotations.Range(0, 23)] System.Int32 hour)
		{
			_logger.LogInformation("GetCandleClosePrice({@0}, {@1})", dateOnly, hour);

			System.Collections.Generic.List<CryptoPriceAPI.DTOs.PriceDTO> prices = new();

			foreach (CryptoPriceAPI.Services.Interfaces.ICryptoService externalService in _externalServices)
			{
				prices.Add(await externalService.GetPrice(dateOnly, hour));
			}

			return _aggregationService.Aggregate(prices);
		}
	}
}
