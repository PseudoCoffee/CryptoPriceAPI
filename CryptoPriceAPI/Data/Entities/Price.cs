namespace CryptoPriceAPI.Data.Entities
{
	[System.ComponentModel.DataAnnotations.Schema.Table($"{nameof(Price)}s")]
	[Microsoft.EntityFrameworkCore.PrimaryKey(nameof(SourceId), nameof(DateAndHourTicks), nameof(FinancialInstrument))]
	public class Price
	{
		public required System.Guid SourceId { get; set; }

		public System.Int64 DateAndHourTicks { get; private set; }

		[System.ComponentModel.DataAnnotations.Schema.NotMapped]
		public CryptoPriceAPI.Data.Entities.DateAndHour DateAndHour
		{
			get
			{
				return new DateAndHour(DateAndHourTicks);
			}
			set
			{
				DateAndHourTicks = value.DateTime.Ticks;
			}
		}

		public required CryptoPriceAPI.Data.Entities.FinancialInstrument FinancialInstrument { get; set; }

		public required System.Single ClosePrice { get; set; }
	}
}
