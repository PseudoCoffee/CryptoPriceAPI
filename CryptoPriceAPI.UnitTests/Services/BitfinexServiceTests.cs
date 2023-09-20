using CryptoPriceAPI.Services.Helper;
using Moq;
using System.Linq.Expressions;

namespace CryptoPriceAPI.UnitTests.Services
{
	public class BitfinexServiceTests
	{
		private readonly CryptoPriceAPI.Services.BitfinexService bitfinexService;

		private readonly Mock<Microsoft.Extensions.Logging.ILogger<CryptoPriceAPI.Services.BitfinexService>> mockLogger;
		private readonly Mock<MediatR.IMediator> mockMediator;
		private readonly Mock<CryptoPriceAPI.Services.Interfaces.IExternalAPICaller> mockExternalAPICaller;
		private readonly Mock<Microsoft.Extensions.Options.IOptions<CryptoPriceAPI.Services.Configuration.PriceSources>> mockOptions;

		#region Constructor helpers
		
		private static System.String GetPriceSourceName()
		{
			CryptoPriceAPI.Services.Helper.PriceSourceNameAttribute? priceSourceNameAttribute =
				(CryptoPriceAPI.Services.Helper.PriceSourceNameAttribute?)Attribute.GetCustomAttribute(
					typeof(CryptoPriceAPI.Services.BitfinexService), typeof(CryptoPriceAPI.Services.Helper.PriceSourceNameAttribute));

			return priceSourceNameAttribute?.PriceSourceKey ?? throw new Exception($"Expected {nameof(CryptoPriceAPI.Services.Helper.PriceSourceNameAttribute)} not found or empty/null for {nameof(CryptoPriceAPI.Services.BitfinexService)}");
		}

		private static CryptoPriceAPI.Services.Configuration.PriceSources GetPriceSources(System.String priceSource)
		{
			return new CryptoPriceAPI.Services.Configuration.PriceSources()
			{
				{
					priceSource,
					new CryptoPriceAPI.Services.Configuration.CryptoConfiguration ()
					{
						CandleUrlFormat = "",
						UppercaseFinancialInstrument = default,
						StartFormat = "",
						EndFormat = default,
						LimitFormat = "",
						TimeFormat = default
					}
				}
			};
		}

		#endregion

		public BitfinexServiceTests()
		{
			mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<CryptoPriceAPI.Services.BitfinexService>>();
			mockMediator = new Mock<MediatR.IMediator>();
			mockExternalAPICaller = new Mock<CryptoPriceAPI.Services.Interfaces.IExternalAPICaller>();
			mockOptions = new Mock<Microsoft.Extensions.Options.IOptions<CryptoPriceAPI.Services.Configuration.PriceSources>>();

			System.String priceSource = GetPriceSourceName();

			CryptoPriceAPI.Services.Configuration.PriceSources priceSources = GetPriceSources(priceSource);

			mockOptions
				.Setup(options => options.Value)
				.Returns(priceSources);

			bitfinexService = new CryptoPriceAPI.Services.BitfinexService(mockLogger.Object, mockMediator.Object, mockExternalAPICaller.Object, priceSource, mockOptions.Object);
		}

		[Fact]
		public void Test()
		{
			// Arrange

			// Act

			// Test
		}
	}
}
