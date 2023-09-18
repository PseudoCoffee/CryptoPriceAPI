namespace CryptoPriceAPI.Data.Entities
{
	[System.ComponentModel.DataAnnotations.Schema.Table($"{nameof(Price)}s")]
	[Microsoft.EntityFrameworkCore.PrimaryKey(nameof(SourceName), nameof(_dateAndHourTicks))]
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
	}
}
