namespace CryptoPriceAPI.DTOs
{
	public class PriceDTO
	{
		public System.DateTime? DateAndHour { get; set; }

		public CryptoPriceAPI.Data.Entities.FinancialInstrumentName? FinancialInstrumentName { get; set; }

		public System.Single? ClosePrice { get; set; }
	}
}
