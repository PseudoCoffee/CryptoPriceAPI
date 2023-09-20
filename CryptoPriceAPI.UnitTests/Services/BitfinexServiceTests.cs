using Moq;

namespace CryptoPriceAPI.UnitTests.Services
{
	public class BitfinexServiceTests
	{
		private readonly CryptoPriceAPI.Services.BitfinexService bitfinexService;

		private readonly Mock<MediatR.IMediator> mockMediator;
		private readonly Mock<Microsoft.Extensions.Logging.ILogger<CryptoPriceAPI.Services.BitfinexService>> mockLogger;
		private readonly Mock<Microsoft.Extensions.Options.IOptions<CryptoPriceAPI.Services.Configuration.PriceSources>> mockOptions;

		public BitfinexServiceTests()
		{
			mockMediator = new Mock<MediatR.IMediator>();
			mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<CryptoPriceAPI.Services.BitfinexService>>();
			mockOptions = new Mock<Microsoft.Extensions.Options.IOptions<CryptoPriceAPI.Services.Configuration.PriceSources>>();

			bitfinexService = new CryptoPriceAPI.Services.BitfinexService(mockMediator.Object, mockLogger.Object, "", mockOptions.Object);
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
