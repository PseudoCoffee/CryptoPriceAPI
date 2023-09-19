namespace CryptoPriceAPI.Services
{
	public class BitstampService : CryptoPriceAPI.Services.Interfaces.ACryptoService<CryptoPriceAPI.DTOs.Bitstamp.BitstampDTO>
	{
		public BitstampService(
			MediatR.IMediator mediator,
			Microsoft.Extensions.Logging.ILogger<BitstampService> logger,
			System.String sourceName,
			Microsoft.Extensions.Options.IOptions<CryptoPriceAPI.Services.Configuration.PriceSources> options) : base(mediator, logger, sourceName, options)
		{
		}
	}
}
