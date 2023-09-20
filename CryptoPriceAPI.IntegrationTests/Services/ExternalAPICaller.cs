namespace CryptoPriceAPI.IntegrationTests.Services
{
	public class ExternalAPICaller : CryptoPriceAPI.IntegrationTests.Services.Interfaces.AServiceTests
	{
		public ExternalAPICaller(CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory factory) : base(factory)
		{

		}

		[Theory]
		[InlineData(2022,  1,  1, 12, "[[1641042000000,47108,47049,47219,46969,52.31308111]]")]
		[InlineData(2022,  4, 16, 12, "[[1650114000000,40478,40469,40491,40423.30060063,25.14146703]]")]
		[InlineData(2022,  6, 16, 12, "[[1655384400000,21168.07362808,21046,21352,20922,457.55747525]]")]
		[InlineData(2022, 10,  1, 12, "[[1664629200000,19300,19337.98080223,19403,19289.93734741,66.10866657]]")]
		public async Task GetStringResponseFrom_Bitfinex_Returns_StringAsync(System.Int32 year, System.Int32 month, System.Int32 day, System.Int32 hour, System.String expected)
		{
			// Arrange
			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(new System.DateOnly(year, month, day), hour);
			System.Uri uri = _externalAPICaller.GenerateUri(_bitfinexConfiguration, dateAndHour, CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, 1);

			// Act
			System.String jsonResponse = await _externalAPICaller.GetStringResponseFrom(uri);

			// Assert
			Assert.Equal(expected, jsonResponse);
		}

		[Theory]
		[InlineData(2022,  1,  1, 12, "{\"data\": {\"ohlc\": [{\"close\": \"47029.06\", \"high\": \"47207.27\", \"low\": \"46947.58\", \"open\": \"47103.3\", \"timestamp\": \"1641042000\", \"volume\": \"13.21418052\"}], \"pair\": \"BTC/USD\"}}")]
		[InlineData(2022,  4, 16, 12, "{\"data\": {\"ohlc\": [{\"close\": \"40461.95\", \"high\": \"40480.56\", \"low\": \"40391.94\", \"open\": \"40469.88\", \"timestamp\": \"1650114000\", \"volume\": \"7.15358733\"}], \"pair\": \"BTC/USD\"}}")]
		[InlineData(2022,  6, 16, 12, "{\"data\": {\"ohlc\": [{\"close\": \"21016.08\", \"high\": \"21329.55\", \"low\": \"20900\", \"open\": \"21146.59\", \"timestamp\": \"1655384400\", \"volume\": \"185.51158745\"}], \"pair\": \"BTC/USD\"}}")]
		[InlineData(2022, 10,  1, 12, "{\"data\": {\"ohlc\": [{\"close\": \"19334\", \"high\": \"19398\", \"low\": \"19280\", \"open\": \"19287\", \"timestamp\": \"1664629200\", \"volume\": \"12.57967046\"}], \"pair\": \"BTC/USD\"}}")]
		public async Task GetStringResponseFrom_Bitstamp_Returns_StringAsync(System.Int32 year, System.Int32 month, System.Int32 day, System.Int32 hour, System.String expected)
		{
			// Arrange
			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(new System.DateOnly(year, month, day), hour);
			System.Uri uri = _externalAPICaller.GenerateUri(_bitstampConfiguration, dateAndHour, CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, 1);

			// Act
			System.String jsonResponse = await _externalAPICaller.GetStringResponseFrom(uri);

			// Assert
			Assert.Equal(expected, jsonResponse);
		}

	}
}
