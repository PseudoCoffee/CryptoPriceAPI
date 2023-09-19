namespace CryptoPriceAPI.DTOs
{
	public class PriceDTO
	{
		public required System.DateTime DateAndHour { get; set; }

		public required CryptoPriceAPI.Data.Entities.FinancialInstrument FinancialInstrument { get; set; }

		public System.String FinancialInstrumentName => Enum.GetName(typeof(CryptoPriceAPI.Data.Entities.FinancialInstrument), FinancialInstrument)!;

		public required System.Single ClosePrice { get; set; }
	}
}
