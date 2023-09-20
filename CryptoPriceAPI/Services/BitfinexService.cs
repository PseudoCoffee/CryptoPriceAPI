namespace CryptoPriceAPI.Services
{
	[CryptoPriceAPI.Services.Helper.PriceSourceName(priceSourceKey: "bitfinex")]
	public class BitfinexService : CryptoPriceAPI.Services.Interfaces.ACryptoService<CryptoPriceAPI.DTOs.Bitfinex.BitfinexDTO>
	{
		public BitfinexService(
			Microsoft.Extensions.Logging.ILogger<BitfinexService> logger,
			MediatR.IMediator mediator,
			CryptoPriceAPI.Services.Interfaces.IExternalAPICaller externalAPICaller,
			System.String? sourceName,
			Microsoft.Extensions.Options.IOptions<CryptoPriceAPI.Services.Configuration.PriceSources> options) : base(logger, mediator, externalAPICaller, sourceName, options)
		{
		}
	}
}
