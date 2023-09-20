namespace CryptoPriceAPI.Services
{
	[CryptoPriceAPI.Services.Helper.PriceSourceName(priceSourceKey: "bitfinex")]
	public class BitfinexService : CryptoPriceAPI.Services.Interfaces.ACryptoService<CryptoPriceAPI.DTOs.Bitfinex.BitfinexDTO>
	{
		public BitfinexService(
			MediatR.IMediator mediator,
			Microsoft.Extensions.Logging.ILogger<BitfinexService> logger,
			System.String sourceName,
			Microsoft.Extensions.Options.IOptions<CryptoPriceAPI.Services.Configuration.PriceSources> options) : base(mediator, logger, sourceName, options)
		{
		}
	}
}
