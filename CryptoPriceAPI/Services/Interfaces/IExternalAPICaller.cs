namespace CryptoPriceAPI.Services.Interfaces
{
	public interface IExternalAPICaller
	{
		public Task<System.String> GetJSONResponseFrom(System.String url);

		public System.String PrepareURL(
			CryptoPriceAPI.Services.Configuration.CryptoConfiguration cryptoConfiguration,
			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour,
			CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD,
			System.Int32 limit = 1);
	}
}
