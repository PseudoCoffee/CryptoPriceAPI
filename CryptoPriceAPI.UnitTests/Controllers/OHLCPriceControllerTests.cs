using Moq;

namespace CryptoPriceAPI.UnitTests.Controllers
{
	public class OHLCPriceControllerTests
	{
		private readonly CryptoPriceAPI.Controller.OHLCPriceController ohlcPriceController;

		private readonly Mock<Microsoft.Extensions.Logging.ILogger<CryptoPriceAPI.Controller.OHLCPriceController>> mockLogger;
		private readonly Mock<CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO>> mockAggregationService;
		private readonly Mock<CryptoPriceAPI.Services.Interfaces.ICryptoService> mockCryptoService;

		public OHLCPriceControllerTests()
		{
			mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<Controller.OHLCPriceController>>();
			mockAggregationService = new Mock<CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO>>();
			mockCryptoService = new Mock<CryptoPriceAPI.Services.Interfaces.ICryptoService>();

			ohlcPriceController = new CryptoPriceAPI.Controller.OHLCPriceController(
				mockAggregationService.Object,
				mockLogger.Object,
				new System.Collections.Generic.List<CryptoPriceAPI.Services.Interfaces.ICryptoService>() { mockCryptoService.Object });
		}

		[Fact]
		public async void GetCandleClosePrice_Returns_PriceDTO()
		{
			// Arrange
			CryptoPriceAPI.DTOs.PriceDTO price = GetPriceDTOs(1).First();

			mockCryptoService
				.Setup(service => service.GetPriceAsync(
					It.Is<CryptoPriceAPI.Data.Entities.DateAndHour>(dah => dah.DateTime == price.DateAndHour.DateTime),
					It.Is<CryptoPriceAPI.Data.Entities.FinancialInstrument>(f => f == CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD)))
				.ReturnsAsync(price);
			
			mockAggregationService
				.Setup(service => service.Aggregate(It.Is<System.Collections.Generic.List<CryptoPriceAPI.DTOs.PriceDTO>>(p => p.Contains(price))))
				.Returns(price);

			// Act
			CryptoPriceAPI.DTOs.PriceDTO result = await ohlcPriceController.GetCandleClosePriceAsync(price.DateAndHour.DateOnly, price.DateAndHour.Hour);

			// Assert
			Assert.Equal(price, result);
		}

		[Fact]
		public async void GetCandleClosePrice_FailsValidation()
		{
			// Arrange
			System.DateOnly dateOnly = new(2022, 1, 1);
			System.Int32 hour = 25;

			// Act & Asser
			await Assert.ThrowsAsync<ArgumentException>(async () => await ohlcPriceController.GetCandleClosePriceAsync(dateOnly, hour)).ConfigureAwait(false);
		}

		private static System.Collections.Generic.IEnumerable<CryptoPriceAPI.DTOs.PriceDTO> GetPriceDTOs(int number)
		{
			System.Collections.Generic.List<CryptoPriceAPI.DTOs.PriceDTO> list = new()
			{
				new CryptoPriceAPI.DTOs.PriceDTO {  DateAndHour = new (new System.DateOnly(2022, 1, 1), 12), FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, ClosePrice = 47039.03f },
				new CryptoPriceAPI.DTOs.PriceDTO {  DateAndHour = new (new (2022, 3, 16), 12), FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, ClosePrice = 40652.54f },
				new CryptoPriceAPI.DTOs.PriceDTO {  DateAndHour = new (new (2022, 7, 1), 12), FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, ClosePrice = 19460.875f },
				new CryptoPriceAPI.DTOs.PriceDTO {  DateAndHour = new (new(2022, 11, 16), 12), FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, ClosePrice = 16484f },
			};

			return list.Take(number);
		}
	}
}
