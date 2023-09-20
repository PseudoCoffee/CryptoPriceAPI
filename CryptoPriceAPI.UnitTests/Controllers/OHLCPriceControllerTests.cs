﻿using Moq;

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
				mockLogger.Object,
				mockAggregationService.Object,
				new System.Collections.Generic.List<CryptoPriceAPI.Services.Interfaces.ICryptoService>() { mockCryptoService.Object });
		}

		[Fact]
		public async void GetCandleClosePrice_Returns_PriceDTO()
		{
			// Arrange
			CryptoPriceAPI.DTOs.PriceDTO price = CryptoPriceAPI.UnitTests.TestData.GetSameDateAndFinancialInstrumentPriceDTOs(1).First();

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
		public async void GetCandleClosePrice_Calls_CryptoService_Once()
		{
			// Arrange
			CryptoPriceAPI.DTOs.PriceDTO price = CryptoPriceAPI.UnitTests.TestData.GetSameDateAndFinancialInstrumentPriceDTOs(1).First();

			// Act
			await ohlcPriceController.GetCandleClosePriceAsync(price.DateAndHour.DateOnly, price.DateAndHour.Hour);

			// Assert
			mockCryptoService.Verify(service => service.GetPriceAsync(
				It.IsAny<CryptoPriceAPI.Data.Entities.DateAndHour>(),
				It.IsAny<CryptoPriceAPI.Data.Entities.FinancialInstrument>()), Moq.Times.Once);
		}

		[Fact]
		public async void GetCandleClosePrice_Calls_MockAggregationService_Once()
		{
			// Arrange
			CryptoPriceAPI.DTOs.PriceDTO price = CryptoPriceAPI.UnitTests.TestData.GetSameDateAndFinancialInstrumentPriceDTOs(1).First();

			// Act
			await ohlcPriceController.GetCandleClosePriceAsync(price.DateAndHour.DateOnly, price.DateAndHour.Hour);

			// Assert
			mockAggregationService.Verify(service => service.Aggregate(It.IsAny<System.Collections.Generic.List<CryptoPriceAPI.DTOs.PriceDTO>>()), Moq.Times.Once);
		}
	}
}
