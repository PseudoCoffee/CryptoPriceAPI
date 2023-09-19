namespace CryptoPriceAPI.DTOs
{
	public class PriceDTO
	{
		public required CryptoPriceAPI.Data.Entities.DateAndHour DateAndHour { get; set; }

		public required CryptoPriceAPI.Data.Entities.FinancialInstrument FinancialInstrument { get; set; }

		public required System.Single ClosePrice { get; set; }
	}
}
