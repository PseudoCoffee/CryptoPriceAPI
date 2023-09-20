namespace CryptoPriceAPI.UnitTests.Data.Entities
{
	public class DateAndHourTests
	{
		[Theory]
		[InlineData(1999, 2, 21, 23)]
		[InlineData(2010, 10, 25, 0)]
		[InlineData(2016, 2, 29, 2)]
		[InlineData(2023, 4, 4, 17)]
		public void DateAndHour_Returns_ExactDateTime(System.Int32 year, System.Int32 month, System.Int32 day, System.Int32 hour)
		{
			// Arrange
			System.DateOnly dateOnly = new(year, month, day);
			System.DateTime dateTime = dateOnly.ToDateTime(new TimeOnly(hour, 0));

			// Act
			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(dateOnly, hour);

			// Assert
			Assert.Equal(dateTime, dateAndHour.DateTime);
		}

		[Theory]
		[InlineData(0)]						// (year: 0, month: 0, day: 0, hour: 0)
		[InlineData(634234752000000000)]	// (year: 2010, month: 10, day: 25, hour: 0)
		[InlineData(635923080000000000)]	// (year: 2016, month: 2, day: 29, hour: 2)
		[InlineData(3155378940000000000)]   // (year: 9999, month: 12, day: 31, hour: 23)
		public void DateAndHour_Returns_ExactTicks(System.Int64 ticks)
		{
			// Arrange

			// Act
			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(ticks);

			// Assert
			Assert.Equal(ticks, dateAndHour.DateTime.Ticks);
		}

		[Theory]
		[InlineData(1999, 2, 21, 24)]
		[InlineData(2010, 10, 25, -1)]
		[InlineData(2016, 2, 29, 200)]
		[InlineData(2023, 4, 4, -17)]
		public void DateAndHour_Throws_ArgumentException(System.Int32 year, System.Int32 month, System.Int32 day, System.Int32 hour)
		{            
			// Arrange
			System.DateOnly dateOnly = new(year, month, day);

			// Act & Assert
			Assert.Throws<ArgumentException>(() => new CryptoPriceAPI.Data.Entities.DateAndHour(dateOnly, hour));
		}

		[Theory]
		[InlineData(System.Int64.MinValue)]
		[InlineData(-1)]
		[InlineData(3155378976000000000)]
		[InlineData(System.Int64.MaxValue)]
		public void DateAndHour_Throws_ArgumentOutOfRangeException(System.Int64 ticks)
		{
			// Arrange

			// Act & Assert
			Assert.Throws<ArgumentOutOfRangeException>(() => new CryptoPriceAPI.Data.Entities.DateAndHour(ticks));
		}
	}
}
