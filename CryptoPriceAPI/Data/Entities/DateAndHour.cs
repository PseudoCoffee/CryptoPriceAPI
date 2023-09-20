namespace CryptoPriceAPI.Data.Entities
{
	public class DateAndHour
	{
		public required System.DateOnly DateOnly { get; set; }

		public required System.Int32 Hour { get; set; }


		[System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
		public DateAndHour(System.DateOnly dateOnly, System.Int32 hour)
		{
			if (hour < 0 || hour >= 24)
			{
				throw new ArgumentException($"{nameof(hour)} was outside of range [0, 23), value: {hour}", nameof(hour));
			}

			DateOnly = dateOnly;
			Hour = hour;
		}

		[System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
		public DateAndHour(System.Int64 ticks) : this(DateOnly.FromDateTime(new System.DateTime(ticks)), new System.DateTime(ticks).Hour)
		{
		}

		public System.DateTime DateTime => DateOnly.ToDateTime(new TimeOnly(Hour, 0));
	}
}
