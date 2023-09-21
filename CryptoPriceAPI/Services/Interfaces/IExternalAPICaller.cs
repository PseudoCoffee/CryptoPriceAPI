namespace CryptoPriceAPI.Services.Interfaces
{
	public interface IExternalAPICaller
	{
		/// <summary>
		/// Generate a valid URI that can be used to be fetch crypto prices.
		/// </summary>
		/// <param name="cryptoConfiguration"> Configuration object for the crypto service URI. </param>
		/// <param name="dateAndHour"> Starting date and hour of the price. </param>
		/// <param name="financialInstrument"> Financial instrument name of the price (e.g.: BTCUSD, BTCEUR). </param>
		/// <param name="limit"> Number of entries to fetch. </param>
		/// <returns> An URI adjusted for UTC+0 timezone. </returns>
		public System.Uri GenerateUri(
			CryptoPriceAPI.Services.Configuration.CryptoConfiguration cryptoConfiguration,
			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour,
			CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD,
			System.Int32 limit = 1);

		/// <summary>
		/// Call the given <paramref name="uri"/> and return the resulting string.
		/// </summary>
		/// <param name="uri"> URI to be called. </param>
		/// <returns> A string with the body response of the <paramref name="uri"/>. </returns>
		public Task<System.String> GetStringResponseFrom(System.Uri uri);
	}
}
