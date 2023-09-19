namespace CryptoPriceAPI.DTOs
{
	public class PriceDTO
	{
		public required System.DateTime DateAndHour { get; set; }

		public required CryptoPriceAPI.Data.Entities.FinancialInstrument FinancialInstrumentName { get; set; }

		public required System.Single ClosePrice { get; set; }
	}
}
