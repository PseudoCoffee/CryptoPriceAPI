using CryptoPriceAPI.Data.Entities;
using CryptoPriceAPI.DTOs;

namespace CryptoPriceAPI.Services.Interfaces
{
	public interface ICryptoService
	{
		Task<PriceDTO> GetPrice(DateOnly date, int hour, FinancialInstrument financialInstrumentName = FinancialInstrument.BTCUSD);
	}
}