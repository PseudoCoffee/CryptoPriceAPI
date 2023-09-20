using Microsoft.Extensions.DependencyInjection;

namespace CryptoPriceAPI.IntegrationTests.Services.Interfaces
{
	public class AServiceTests : Xunit.IClassFixture<CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory>
	{
		private readonly Microsoft.Extensions.DependencyInjection.IServiceScope scope;

		protected readonly CryptoPriceAPI.Services.Interfaces.IExternalAPICaller _externalAPICaller;
		protected readonly CryptoPriceAPI.Services.Configuration.CryptoConfiguration _bitfinexConfiguration;
		protected readonly CryptoPriceAPI.Services.Configuration.CryptoConfiguration _bitstampConfiguration;


		public AServiceTests(CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory factory)
		{
			scope = factory.Services.CreateAsyncScope();

			_externalAPICaller = scope.ServiceProvider.GetRequiredService<CryptoPriceAPI.Services.Interfaces.IExternalAPICaller>();

			_bitfinexConfiguration = new()
			{
				CandleUrlFormat = "https://api-pub.bitfinex.com/v2/candles/trade:1h:t{0}/hist?",
				UppercaseFinancialInstrument = true,
				StartFormat = "start={0}",
				EndFormat = "end={0}",
				LimitFormat = "limit={0}",
				TimeFormat = CryptoPriceAPI.Services.Configuration.TimeFormat.Milliseconds
			};

			_bitstampConfiguration = new()
			{
				CandleUrlFormat = "https://www.bitstamp.net/api/v2/ohlc/{0}/?step=3600&",
				UppercaseFinancialInstrument = false,
				StartFormat = "start={0}",
				EndFormat = "end={0}",
				LimitFormat = "limit={0}",
				TimeFormat = CryptoPriceAPI.Services.Configuration.TimeFormat.Seconds
			};
		}
	}
}
