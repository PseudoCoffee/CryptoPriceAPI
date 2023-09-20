using Moq;

namespace CryptoPriceAPI.UnitTests.Services
{
	public class BitstampServiceTests
	{
		private readonly CryptoPriceAPI.Services.BitstampService bitstampService;

		private readonly Mock<Microsoft.Extensions.Logging.ILogger<CryptoPriceAPI.Services.BitstampService>> mockLogger;
		private readonly Mock<MediatR.IMediator> mockMediator;
		private readonly Mock<CryptoPriceAPI.Services.Interfaces.IExternalAPICaller> mockExternalAPICaller;
		private readonly Mock<Microsoft.Extensions.Options.IOptions<CryptoPriceAPI.Services.Configuration.PriceSources>> mockOptions;
		private readonly System.String sourceName;
		private readonly CryptoPriceAPI.Services.Configuration.CryptoConfiguration _cryptoConfiguration;

		#region Constructor helpers

		private static System.String GetPriceSourceName()
		{
			CryptoPriceAPI.Services.Helper.PriceSourceNameAttribute? priceSourceNameAttribute =
				(CryptoPriceAPI.Services.Helper.PriceSourceNameAttribute?)Attribute.GetCustomAttribute(
					typeof(CryptoPriceAPI.Services.BitstampService), typeof(CryptoPriceAPI.Services.Helper.PriceSourceNameAttribute));

			return priceSourceNameAttribute?.PriceSourceKey ?? throw new Exception($"Expected {nameof(CryptoPriceAPI.Services.Helper.PriceSourceNameAttribute)} not found or empty/null for {nameof(CryptoPriceAPI.Services.BitstampService)}");
		}

		private static CryptoPriceAPI.Services.Configuration.PriceSources GetPriceSources(System.String priceSource)
		{
			return new CryptoPriceAPI.Services.Configuration.PriceSources()
			{
				{
					priceSource,
					CryptoPriceAPI.UnitTests.TestData.GetCryptoConfigurations(1).First()
				}
			};
		}

		#endregion

		public BitstampServiceTests()
		{
			mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<CryptoPriceAPI.Services.BitstampService>>();
			mockMediator = new Mock<MediatR.IMediator>();
			mockExternalAPICaller = new Mock<CryptoPriceAPI.Services.Interfaces.IExternalAPICaller>();
			mockOptions = new Mock<Microsoft.Extensions.Options.IOptions<CryptoPriceAPI.Services.Configuration.PriceSources>>();

			sourceName = GetPriceSourceName();

			CryptoPriceAPI.Services.Configuration.PriceSources priceSources = GetPriceSources(sourceName);

			_cryptoConfiguration = priceSources[sourceName];

			mockOptions
				.Setup(options => options.Value)
				.Returns(priceSources);

			bitstampService = new CryptoPriceAPI.Services.BitstampService(mockLogger.Object, mockMediator.Object, mockExternalAPICaller.Object, sourceName, mockOptions.Object);
		}

		[Fact]
		public async void GetPriceAsync_CallsMediator_GetSourceByNameQuery_OnceAsync()
		{
			// Arrange
			SetupDefaultReturnStringForExternalApiCaller();

			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(new DateOnly(2023, 1, 1), 0);
			CryptoPriceAPI.Data.Entities.Source source = CryptoPriceAPI.UnitTests.TestData.GetSources(1).First();
			source.Name = sourceName;

			mockMediator
				.Setup(mediator => mediator.Send(It.IsAny<CryptoPriceAPI.Queries.GetSourceByNameQuery>(), It.IsAny<System.Threading.CancellationToken>()))
				.ReturnsAsync(source);

			// Act
			await bitstampService.GetCandleClosePriceAsync(dateAndHour);

			// Assert
			mockMediator.Verify(service => service.Send(It.IsAny<CryptoPriceAPI.Queries.GetSourceByNameQuery>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
		}

		[Fact]
		public async Task GetPriceAsync_Throws_NullReferenceException_SourceAsyncAsync()
		{
			// Arrange
			SetupDefaultReturnStringForExternalApiCaller();

			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(new DateOnly(2023, 1, 1), 0);

			mockMediator
				.Setup(mediator => mediator.Send(It.IsAny<CryptoPriceAPI.Queries.GetSourceByNameQuery>(), It.IsAny<System.Threading.CancellationToken>()))
				.ReturnsAsync((CryptoPriceAPI.Data.Entities.Source?)null);

			// Act & Assert
			NullReferenceException exception = await Assert.ThrowsAsync<NullReferenceException>(async () => await bitstampService.GetCandleClosePriceAsync(dateAndHour));
			Assert.Equal("source", exception.Message);
		}

		[Fact]
		public async void GetPriceAsync_CallsMediator_GetPriceQuery_OnceAsync()
		{
			// Arrange
			SetupDefaultReturnStringForExternalApiCaller();

			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(new DateOnly(2023, 1, 1), 0);
			CryptoPriceAPI.Data.Entities.Source source = CryptoPriceAPI.UnitTests.TestData.GetSources(1).First();
			source.Name = sourceName;

			mockMediator
				.Setup(mediator => mediator.Send(It.IsAny<CryptoPriceAPI.Queries.GetSourceByNameQuery>(), It.IsAny<System.Threading.CancellationToken>()))
				.ReturnsAsync(source);

			// Act
			await bitstampService.GetCandleClosePriceAsync(dateAndHour);

			// Assert
			mockMediator.Verify(service => service.Send(It.IsAny<CryptoPriceAPI.Queries.GetPriceQuery>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
		}

		[Fact]
		public async void GetPriceAsync_PriceNotInDB_CallsExternalAPI_GenerateUri_OnceAsync()
		{
			// Arrange
			SetupDefaultReturnStringForExternalApiCaller();

			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(new DateOnly(2023, 1, 1), 0);
			CryptoPriceAPI.Data.Entities.Source source = CryptoPriceAPI.UnitTests.TestData.GetSources(1).First();
			source.Name = sourceName;
			System.Uri uri = new("https://www.test.com");

			mockMediator
				.Setup(mediator => mediator.Send(It.IsAny<CryptoPriceAPI.Queries.GetSourceByNameQuery>(), It.IsAny<System.Threading.CancellationToken>()))
				.ReturnsAsync(source);

			mockMediator
				.Setup(mediator => mediator.Send(It.IsAny<CryptoPriceAPI.Queries.GetPriceQuery>(), It.IsAny<System.Threading.CancellationToken>()))
				.ReturnsAsync((CryptoPriceAPI.Data.Entities.Price?)null);

			mockExternalAPICaller
				.Setup(externalAPICaller => externalAPICaller.GenerateUri(
					It.Is<CryptoPriceAPI.Services.Configuration.CryptoConfiguration>(cc => cc == _cryptoConfiguration),
					It.Is<CryptoPriceAPI.Data.Entities.DateAndHour>(dah => dah == dateAndHour),
					It.IsAny<CryptoPriceAPI.Data.Entities.FinancialInstrument>(),
					It.IsAny<System.Int32>()))
				.Returns(uri);

			// Act
			await bitstampService.GetCandleClosePriceAsync(dateAndHour);

			// Assert
			mockExternalAPICaller.Verify(service => service.GenerateUri(
					It.Is<CryptoPriceAPI.Services.Configuration.CryptoConfiguration>(cc => cc == _cryptoConfiguration),
					It.Is<CryptoPriceAPI.Data.Entities.DateAndHour>(dah => dah == dateAndHour),
					It.IsAny<CryptoPriceAPI.Data.Entities.FinancialInstrument>(),
					It.IsAny<System.Int32>()),
				Times.Once);
		}

		[Fact]
		public async void GetPriceAsync_PriceNotInDB_CallsExternalAPI_GetStringResponseFrom_OnceAsync()
		{
			// Arrange
			SetupDefaultReturnStringForExternalApiCaller();

			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(new DateOnly(2023, 1, 1), 0);
			CryptoPriceAPI.Data.Entities.Source source = CryptoPriceAPI.UnitTests.TestData.GetSources(1).First();
			source.Name = sourceName;
			System.Uri uri = new("https://www.test.com");
			System.String replyMessage = $"{{\"data\": {{\"ohlc\": [{{\"close\": \"16521\", \"high\": \"16532\", \"low\": \"16507\", \"open\": \"16530\", \"timestamp\": \"1672531200\", \"volume\": \"17.05204457\"}}], \"pair\": \"BTC/USD\"}}}}";

			mockMediator
				.Setup(mediator => mediator.Send(It.IsAny<CryptoPriceAPI.Queries.GetSourceByNameQuery>(), It.IsAny<System.Threading.CancellationToken>()))
				.ReturnsAsync(source);

			mockMediator
				.Setup(mediator => mediator.Send(It.IsAny<CryptoPriceAPI.Queries.GetPriceQuery>(), It.IsAny<System.Threading.CancellationToken>()))
				.ReturnsAsync((CryptoPriceAPI.Data.Entities.Price?)null);

			mockExternalAPICaller
				.Setup(externalAPICaller => externalAPICaller.GenerateUri(
					It.IsAny<CryptoPriceAPI.Services.Configuration.CryptoConfiguration>(),
					It.IsAny<CryptoPriceAPI.Data.Entities.DateAndHour>(),
					It.IsAny<CryptoPriceAPI.Data.Entities.FinancialInstrument>(),
					It.IsAny<System.Int32>()))
				.Returns(uri);

			mockExternalAPICaller
				.Setup(externalAPICaller => externalAPICaller.GetStringResponseFrom(It.IsAny<System.Uri>()))
				.ReturnsAsync(replyMessage);

			// Act
			await bitstampService.GetCandleClosePriceAsync(dateAndHour);

			// Assert
			mockExternalAPICaller.Verify(service => service.GetStringResponseFrom(It.Is<System.Uri>(_uri => _uri == uri)), Times.Once);
		}

		[Fact]
		public async void GetPriceAsync_PriceNotInDB_CallsMediator_AddPriceCommandAsync()
		{
			// Arrange
			SetupDefaultReturnStringForExternalApiCaller();

			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(new DateOnly(2023, 1, 1), 0);
			CryptoPriceAPI.Data.Entities.Source source = CryptoPriceAPI.UnitTests.TestData.GetSources(1).First();
			source.Name = sourceName;
			System.Uri uri = new("https://www.test.com");
			System.String replyMessage = $"{{\"data\": {{\"ohlc\": [{{\"close\": \"16521\", \"high\": \"16532\", \"low\": \"16507\", \"open\": \"16530\", \"timestamp\": \"1672531200\", \"volume\": \"17.05204457\"}}], \"pair\": \"BTC/USD\"}}}}";

			mockMediator
				.Setup(mediator => mediator.Send(It.IsAny<CryptoPriceAPI.Queries.GetSourceByNameQuery>(), It.IsAny<System.Threading.CancellationToken>()))
				.ReturnsAsync(source);

			mockMediator
				.Setup(mediator => mediator.Send(It.IsAny<CryptoPriceAPI.Queries.GetPriceQuery>(), It.IsAny<System.Threading.CancellationToken>()))
				.ReturnsAsync((CryptoPriceAPI.Data.Entities.Price?)null);

			mockExternalAPICaller
				.Setup(externalAPICaller => externalAPICaller.GenerateUri(
					It.IsAny<CryptoPriceAPI.Services.Configuration.CryptoConfiguration>(),
					It.IsAny<CryptoPriceAPI.Data.Entities.DateAndHour>(),
					It.IsAny<CryptoPriceAPI.Data.Entities.FinancialInstrument>(),
					It.IsAny<System.Int32>()))
				.Returns(uri);

			mockExternalAPICaller
				.Setup(externalAPICaller => externalAPICaller.GetStringResponseFrom(It.IsAny<System.Uri>()))
				.ReturnsAsync(replyMessage);

			// Act
			await bitstampService.GetCandleClosePriceAsync(dateAndHour);

			// Assert
			mockMediator.Verify(service => service.Send(It.IsAny<CryptoPriceAPI.Commands.AddPriceCommand>(), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
		}

		[Fact]
		public async void GetPriceAsync_PriceNotInDB_ReturnsCorrectPriceDTOAsync()
		{
			// Arrange
			SetupDefaultReturnStringForExternalApiCaller();

			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(new DateOnly(2023, 1, 1), 0);
			CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD;
			CryptoPriceAPI.Data.Entities.Source source = CryptoPriceAPI.UnitTests.TestData.GetSources(1).First();
			source.Name = sourceName;
			System.Uri uri = new("https://www.test.com");
			System.Single priceValue = 16571;
			System.String replyMessage = $"{{\"data\": {{\"ohlc\": [{{\"close\": \"{priceValue}\", \"high\": \"16532\", \"low\": \"16507\", \"open\": \"16530\", \"timestamp\": \"1672531200\", \"volume\": \"17.05204457\"}}], \"pair\": \"BTC/USD\"}}}}";

			mockMediator
				.Setup(mediator => mediator.Send(It.IsAny<CryptoPriceAPI.Queries.GetSourceByNameQuery>(), It.IsAny<System.Threading.CancellationToken>()))
				.ReturnsAsync(source);

			mockMediator
				.Setup(mediator => mediator.Send(It.IsAny<CryptoPriceAPI.Queries.GetPriceQuery>(), It.IsAny<System.Threading.CancellationToken>()))
				.ReturnsAsync((CryptoPriceAPI.Data.Entities.Price?)null);

			mockExternalAPICaller
				.Setup(externalAPICaller => externalAPICaller.GenerateUri(
					It.IsAny<CryptoPriceAPI.Services.Configuration.CryptoConfiguration>(),
					It.IsAny<CryptoPriceAPI.Data.Entities.DateAndHour>(),
					It.IsAny<CryptoPriceAPI.Data.Entities.FinancialInstrument>(),
					It.IsAny<System.Int32>()))
				.Returns(uri);

			mockExternalAPICaller
				.Setup(externalAPICaller => externalAPICaller.GetStringResponseFrom(It.IsAny<System.Uri>()))
				.ReturnsAsync(replyMessage);

			// Act
			CryptoPriceAPI.DTOs.PriceDTO result = await bitstampService.GetCandleClosePriceAsync(dateAndHour, financialInstrument);

			// Assert
			Assert.True(dateAndHour.DateTime == result.DateAndHour.DateTime && financialInstrument == result.FinancialInstrument && priceValue == result.ClosePrice);
		}

		[Fact]
		public async void GetPriceAsync_PriceInDB_ReturnsSamePriceDTOAsInDBAsync()
		{
			// Arrange
			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(new DateOnly(2023, 1, 1), 0);
			CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD;

			CryptoPriceAPI.Data.Entities.Source source = CryptoPriceAPI.UnitTests.TestData.GetSources(1).First();
			source.Name = sourceName;
			CryptoPriceAPI.Data.Entities.Price price = CryptoPriceAPI.UnitTests.TestData.GetRandomPrices(1).First();
			price.DateAndHour = dateAndHour;

			mockMediator
				.Setup(mediator => mediator.Send(It.IsAny<CryptoPriceAPI.Queries.GetSourceByNameQuery>(), It.IsAny<System.Threading.CancellationToken>()))
				.ReturnsAsync(source);

			mockMediator
				.Setup(mediator => mediator.Send(It.IsAny<CryptoPriceAPI.Queries.GetPriceQuery>(), It.IsAny<System.Threading.CancellationToken>()))
				.ReturnsAsync(price);

			// Act
			CryptoPriceAPI.DTOs.PriceDTO result = await bitstampService.GetCandleClosePriceAsync(dateAndHour, financialInstrument);

			// Assert
			Assert.True(dateAndHour.DateTime == result.DateAndHour.DateTime && financialInstrument == result.FinancialInstrument && price.ClosePrice == result.ClosePrice);
		}

		// No test can run without this
		private void SetupDefaultReturnStringForExternalApiCaller()
		{
			mockExternalAPICaller
				.Setup(externalAPICaller => externalAPICaller.GetStringResponseFrom(It.IsAny<System.Uri>()))
				.ReturnsAsync($"{{\"data\": {{\"ohlc\": [{{\"close\": \"16521\", \"high\": \"16532\", \"low\": \"16507\", \"open\": \"16530\", \"timestamp\": \"1672531200\", \"volume\": \"17.05204457\"}}], \"pair\": \"BTC/USD\"}}}}");
		}
	}
}
