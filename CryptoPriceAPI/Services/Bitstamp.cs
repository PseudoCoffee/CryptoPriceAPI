namespace CryptoPriceAPI.Services
{
	public class Bitstamp
	{
		private readonly MediatR.IMediator _mediator;
		private readonly Microsoft.Extensions.Logging.ILogger<Bitstamp> _logger;
		private readonly System.String _sourceName;
		private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

		public Bitstamp(MediatR.IMediator mediator, Microsoft.Extensions.Logging.ILogger<Bitstamp> logger, System.String sourceName, Microsoft.Extensions.Configuration.IConfiguration configuration)
		{
			_mediator = mediator;
			_logger = logger;
			_sourceName = sourceName;
			_configuration = configuration;	
		}

		/// <summary>
		/// Get the close price of the specified <paramref name="financialInstrumentName"/> at given <paramref name="date"/> and <paramref name="hour"/> during a 1hr windows
		/// </summary>
		/// <param name="date"></param>
		/// <param name="hour"></param>
		/// <param name="financialInstrumentName"></param>
		/// <returns></returns>
		public async Task<CryptoPriceAPI.DTOs.PriceDTO> GetPrice(System.DateOnly date, System.Int32 hour, CryptoPriceAPI.Data.Entities.FinancialInstrumentName financialInstrumentName)
		{
			_logger.LogInformation("GetPrice({@0}, {@1})", date, hour);

			CryptoPriceAPI.Data.Entities.Price? price = await _mediator.Send(new CryptoPriceAPI.Queries.GetPriceQuery(_sourceName, date, hour, financialInstrumentName));

			if (price == null)
			{

			}


			return new CryptoPriceAPI.DTOs.PriceDTO();
		}
	}
}
