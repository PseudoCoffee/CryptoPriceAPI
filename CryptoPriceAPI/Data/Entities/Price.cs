namespace CryptoPriceAPI.Data.Entities
{
	[System.ComponentModel.DataAnnotations.Schema.Table($"{nameof(Price)}s")]
	[Microsoft.EntityFrameworkCore.PrimaryKey(nameof(SourceId), nameof(DateAndHourTicks), nameof(FinancialInstrumentName))]
	public class Price
	{
		public required System.Guid SourceId { get; set; }

		public System.Int64 DateAndHourTicks { get; private set; }

		[System.ComponentModel.DataAnnotations.Schema.NotMapped]
		public System.DateTime DateAndHour
		{
			get
			{
				return new(DateAndHourTicks);
			}
			set
			{
				// truncate the DateTime to only date and hour
				DateAndHourTicks = value.Ticks / System.TimeSpan.TicksPerHour * System.TimeSpan.TicksPerHour;
			}
		}

		public required CryptoPriceAPI.Data.Entities.FinancialInstrument FinancialInstrumentName { get; set; }

		public required System.Single ClosePrice { get; set; }
	}
}
