using Moq;
using Moq.Protected;
using System.Net;

namespace CryptoPriceAPI.UnitTests.Services
{
	public class ExternalAPICaller
	{
		private readonly CryptoPriceAPI.Services.ExternalAPICaller externalAPICaller;

		private readonly Mock<Microsoft.Extensions.Logging.ILogger<CryptoPriceAPI.Services.ExternalAPICaller>> mockLogger;
		private readonly Mock<System.Net.Http.HttpMessageHandler> mockHttpMessageHandler;

		public ExternalAPICaller()
		{
			mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<CryptoPriceAPI.Services.ExternalAPICaller>>();
			mockHttpMessageHandler = new Mock<System.Net.Http.HttpMessageHandler>();

			externalAPICaller = new CryptoPriceAPI.Services.ExternalAPICaller(mockLogger.Object, mockHttpMessageHandler.Object);
		}

		[Theory]
		[InlineData(2022, 1, 11, 0, CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, 5)]
		[InlineData(2022, 2, 21, 10, CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, 7)]
		[InlineData(2022, 7, 1, 12, CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, 10)]
		[InlineData(2022, 12, 24, 23, CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, 1)]
		public void PrepareURI_Returns_CorrectURI(System.Int32 year, System.Int32 month, System.Int32 day, System.Int32 hour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrument, System.Int32 limit)
		{
			// Arrange
			CryptoPriceAPI.Services.Configuration.CryptoConfiguration cryptoConfiguration = new()
			{
				CandleUrlFormat = "https://testURl.com/candle/{0}/hist?",
				UppercaseFinancialInstrument = true,
				StartFormat = "start={0}",
				EndFormat = default,
				LimitFormat = "limit={0}",
				TimeFormat = CryptoPriceAPI.Services.Configuration.TimeFormat.Seconds
			};

			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(new DateOnly(year, month, day), hour);

			System.TimeSpan differenceToUTC = dateAndHour.DateTime - dateAndHour.DateTime.ToUniversalTime();
			System.Int64 startTime = ((DateTimeOffset)dateAndHour.DateTime.Add(differenceToUTC)).ToUnixTimeSeconds();

			System.String[] urlParts = new[]
			{
				System.String.Format(cryptoConfiguration.CandleUrlFormat, financialInstrument.ToString().ToUpperInvariant()),
				System.String.Format(cryptoConfiguration.StartFormat, startTime),
				System.String.Format(cryptoConfiguration.LimitFormat, limit)
			};

			System.Uri expectedUri = new(System.String.Concat(urlParts[0], System.String.Join('&', urlParts[1..])));

			// Act
			System.Uri uri = externalAPICaller.GenerateUri(cryptoConfiguration, dateAndHour, financialInstrument, limit);

			// Assert
			Assert.Equal(expectedUri, uri);
		}

		[Fact]
		public async Task GetJSONResponseFrom_Returns_JsonResponseAsync()
		{
			// Arrange
			System.Uri uri = new("https://goosssgle.com/");
			IProtectedMock<System.Net.Http.HttpMessageHandler> mockProtectedHttpMessageHandler = mockHttpMessageHandler.Protected();
			System.String stringResponse = "{ \"hello\": \"world\" }";

			mockProtectedHttpMessageHandler.Setup<Task<System.Net.Http.HttpResponseMessage>>
				(
					"SendAsync",
					ItExpr.IsAny<System.Net.Http.HttpRequestMessage>(),
					ItExpr.IsAny<System.Threading.CancellationToken>()
				)
				.ReturnsAsync(new System.Net.Http.HttpResponseMessage()
				{
					StatusCode = System.Net.HttpStatusCode.OK,
					Content = new System.Net.Http.StringContent(stringResponse)
				});

			// Act
			var response = await externalAPICaller.GetStringResponseFrom(uri);

			// Assert
			Assert.Equal(stringResponse, response);
		}
	}
}
