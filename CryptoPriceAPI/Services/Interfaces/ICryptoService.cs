namespace CryptoPriceAPI.Services.Interfaces
{
	public interface ICryptoService
	{
		Task<CryptoPriceAPI.DTOs.PriceDTO> GetPriceAsync(CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD);
	}
}