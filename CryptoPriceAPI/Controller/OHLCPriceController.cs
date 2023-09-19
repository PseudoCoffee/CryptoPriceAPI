namespace CryptoPriceAPI.Controller
{
	[Microsoft.AspNetCore.Mvc.ApiController]
	public class OHLCPriceController : Microsoft.AspNetCore.Mvc.ControllerBase
	{
		private readonly CryptoPriceAPI.Services.Interfaces.ICryptoService<CryptoPriceAPI.DTOs.BitstampDTO> _bitstampService;
		private readonly Microsoft.Extensions.Logging.ILogger<OHLCPriceController> _logger;

		public OHLCPriceController(CryptoPriceAPI.Services.Interfaces.ICryptoService<CryptoPriceAPI.DTOs.BitstampDTO> bitstampService, Microsoft.Extensions.Logging.ILogger<OHLCPriceController> logger)
		{
			_bitstampService = bitstampService;
			_logger = logger;
		}

		[Microsoft.AspNetCore.Mvc.HttpGet]
		[Microsoft.AspNetCore.Mvc.Route("GetOHLCPrice")]
		public async Task<CryptoPriceAPI.DTOs.PriceDTO> GetOHLCPrice()
		{
			_logger.LogInformation("GetOHLCPrice()");

			System.DateOnly dateOnly = new(2023, 1, 1);
			System.Int32 hour = 0;
			CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD;

			var result = await _bitstampService.GetPrice(dateOnly, hour, financialInstrument);
			
			return result;
		}
	}
}
