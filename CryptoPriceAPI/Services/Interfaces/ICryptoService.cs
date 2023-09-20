namespace CryptoPriceAPI.Services.Interfaces
{
	public interface ICryptoService
	{
		/// <summary>
		/// Get OHLC candle price.
		/// </summary>
		/// <param name="dateAndHour"> Starting date and hour of the price. </param>
		/// <param name="financialInstrument"> Financial instrument name of the price (e.g.: BTCUSD, BTCEUR). </param>
		/// <returns> PriceDTO containing the close price of the candle. </returns>
		Task<CryptoPriceAPI.DTOs.PriceDTO> GetCandleClosePriceAsync(CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD);
	}
}