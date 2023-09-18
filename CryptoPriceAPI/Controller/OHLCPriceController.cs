namespace CryptoPriceAPI.Controller
{
	[Microsoft.AspNetCore.Mvc.ApiController]
	public class OHLCPriceController : Microsoft.AspNetCore.Mvc.ControllerBase
	{
		private readonly CryptoPriceAPI.Services.Interfaces.ICryptoService<CryptoPriceAPI.DTOs.BitstampDTO> _bitstamp;
		private readonly Microsoft.Extensions.Logging.ILogger<OHLCPriceController> _logger;

		public OHLCPriceController(CryptoPriceAPI.Services.Interfaces.ICryptoService<CryptoPriceAPI.DTOs.BitstampDTO> bitstamp, Microsoft.Extensions.Logging.ILogger<OHLCPriceController> logger)
		{
			_bitstamp = bitstamp;
			_logger = logger;
		}

		[Microsoft.AspNetCore.Mvc.HttpGet]
		[Microsoft.AspNetCore.Mvc.Route("GetOHLCPrice")]
		public async Task<CryptoPriceAPI.DTOs.PriceDTO> GetOHLCPrice()
		{
			_logger.LogInformation("GetOHLCPrice()");

			System.DateOnly dateOnly = new(2023, 1, 1);
			System.Int32 hour = 0;
			CryptoPriceAPI.Data.Entities.FinancialInstrumentName financialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrumentName.BTCUSD;

			var ret = await _bitstamp.CallExternalAPI(dateOnly, hour, financialInstrument);
			return null;
		}
	}
}
