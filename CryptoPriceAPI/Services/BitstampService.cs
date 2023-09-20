namespace CryptoPriceAPI.Services
{
	[CryptoPriceAPI.Services.Helper.PriceSourceName(priceSourceKey: "bitstamp")]
	public class BitstampService : CryptoPriceAPI.Services.Interfaces.ACryptoService<CryptoPriceAPI.DTOs.Bitstamp.BitstampDTO>
	{
		public BitstampService(
			Microsoft.Extensions.Logging.ILogger<BitstampService> logger,
			MediatR.IMediator mediator,
			CryptoPriceAPI.Services.Interfaces.IExternalAPICaller externalAPICaller,
			System.String? sourceName,
			Microsoft.Extensions.Options.IOptions<CryptoPriceAPI.Services.Configuration.PriceSources> options) : base(logger, mediator, externalAPICaller, sourceName, options)
		{
		}
	}
}
