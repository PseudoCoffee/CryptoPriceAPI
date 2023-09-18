namespace CryptoPriceAPI.Data.Entities
{
	[System.ComponentModel.DataAnnotations.Schema.Table($"{nameof(Price)}s")]
	[Microsoft.EntityFrameworkCore.PrimaryKey(nameof(SourceName), nameof(_dateAndHourTicks), nameof(FinancialInstrumentName))]
	public class Price
	{
		public required System.String SourceName { get; set; }

		[System.ComponentModel.DataAnnotations.Schema.Column("DateAndHour")]
		private long _dateAndHourTicks;

		[System.ComponentModel.DataAnnotations.Schema.NotMapped]
		public required System.DateTime DateAndHour
		{
			get
			{
				return new(_dateAndHourTicks);
			}
			set
			{
				// truncate the DateTime to only date and hour
				_dateAndHourTicks = value.Ticks / System.TimeSpan.TicksPerHour * System.TimeSpan.TicksPerHour;
			}
		}

		public required CryptoPriceAPI.Data.Entities.FinancialInstrumentName FinancialInstrumentName { get; set; }

		public required System.Single ClosePrice { get; set; }
	}
}
