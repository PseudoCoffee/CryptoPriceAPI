namespace CryptoPriceAPI.Services.Interfaces
{
	public interface IExternalAPICaller
	{
		public System.Uri GenerateUri(
			CryptoPriceAPI.Services.Configuration.CryptoConfiguration cryptoConfiguration,
			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour,
			CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD,
			System.Int32 limit = 1);
		public Task<System.String> GetStringResponseFrom(System.Uri url);
	}
}
