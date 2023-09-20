using Moq;

namespace CryptoPriceAPI.UnitTests.Services
{
	public class AverageServiceTests
	{
		private readonly CryptoPriceAPI.Services.AverageService averageService;

		private readonly Mock<Microsoft.Extensions.Logging.ILogger<CryptoPriceAPI.Services.AverageService>> mockLogger;

		public AverageServiceTests()
		{
			mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<CryptoPriceAPI.Services.AverageService>>();

			averageService = new CryptoPriceAPI.Services.AverageService(mockLogger.Object);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(4)]
		[InlineData(5)]
		public void Aggregate_Returns_AveragePrice(System.Int32 count)
		{
			// Arrange
			System.Collections.Generic.IEnumerable<DTOs.PriceDTO> prices = CryptoPriceAPI.UnitTests.TestData.GetSameDateAndFinancialInstrumentPriceDTOs(count);
			System.Single averagePrice = prices.Select(price => price.ClosePrice).Average();

			// Act
			CryptoPriceAPI.DTOs.PriceDTO result = averageService.Aggregate(prices);

			// Assert
			Assert.Equal(result.ClosePrice, averagePrice);
		}

		[Fact]
		public void Aggregate_Throws_ArgumentNullException()
		{
			// Arrange

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => averageService.Aggregate(null!));
		}

		[Fact]
		public void Aggregate_Throws_ArgumentException_NoElements()
		{
			// Arrange

			// Act & Assert
			ArgumentException exception = Assert.Throws<ArgumentException>(() => averageService.Aggregate(new System.Collections.Generic.List<CryptoPriceAPI.DTOs.PriceDTO>()));
			Assert.Equal($"prices contains no elements.", exception.Message);
		}

		[Theory]
		[InlineData(2)]
		[InlineData(3)]
		public void Aggregate_Throws_ArgumentException_DifferentTimestamps(System.Int32 count)
		{
			// Arrange

			// Act & Assert
			ArgumentException exception = Assert.Throws<ArgumentException>(() => averageService.Aggregate(CryptoPriceAPI.UnitTests.TestData.GetRandomPriceDTOs(count)));
			Assert.Equal($"Cannot aggregate prices with different timestamps.", exception.Message);
		}
	}
}
