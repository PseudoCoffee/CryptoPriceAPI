namespace CryptoPriceAPI.Services.Interfaces
{
	public abstract class ICryptoService<ExternalDTO> where ExternalDTO : class
	{
		public abstract Task<CryptoPriceAPI.DTOs.PriceDTO> GetPrice(System.DateOnly date, System.Int32 hour, CryptoPriceAPI.Data.Entities.FinancialInstrumentName financialInstrumentName);

		public abstract Task<ExternalDTO> CallExternalAPI(System.DateOnly date, System.Int32 hour, CryptoPriceAPI.Data.Entities.FinancialInstrumentName financialInstrumentName, System.Int32 interval = 3600, System.Int32 limit = 1);
	}
}
