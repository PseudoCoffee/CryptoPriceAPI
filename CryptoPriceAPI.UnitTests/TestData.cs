namespace CryptoPriceAPI.UnitTests
{
	public static class TestData
	{
		public static System.Collections.Generic.IEnumerable<CryptoPriceAPI.DTOs.PriceDTO> GetRandomPriceDTOs(System.Int32 number = System.Int32.MaxValue)
		{
			System.Collections.Generic.List<CryptoPriceAPI.DTOs.PriceDTO> list = new()
			{
				new CryptoPriceAPI.DTOs.PriceDTO {  DateAndHour = new (new (2022,  1,  1), 12), FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, ClosePrice = 47039.03f },
				new CryptoPriceAPI.DTOs.PriceDTO {  DateAndHour = new (new (2022,  3, 16), 12), FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, ClosePrice = 40652.54f },
				new CryptoPriceAPI.DTOs.PriceDTO {  DateAndHour = new (new (2022,  7,  1), 12), FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, ClosePrice = 19460.875f },
				new CryptoPriceAPI.DTOs.PriceDTO {  DateAndHour = new (new (2022, 11, 16), 12), FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, ClosePrice = 16484f },
			};

			return list.Take(number);
		}

		public static System.Collections.Generic.IEnumerable<CryptoPriceAPI.DTOs.PriceDTO> GetSameDateAndFinancialInstrumentPriceDTOs(System.Int32 number = System.Int32.MaxValue)
		{
			System.Collections.Generic.List<CryptoPriceAPI.DTOs.PriceDTO> list = new()
			{
				new CryptoPriceAPI.DTOs.PriceDTO {  DateAndHour = new (new (2022,  7, 16), 12), FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, ClosePrice = 47039.03f },
				new CryptoPriceAPI.DTOs.PriceDTO {  DateAndHour = new (new (2022,  7, 16), 12), FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, ClosePrice = 40652.54f },
				new CryptoPriceAPI.DTOs.PriceDTO {  DateAndHour = new (new (2022,  7, 16), 12), FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, ClosePrice = 19460.875f },
				new CryptoPriceAPI.DTOs.PriceDTO {  DateAndHour = new (new (2022,  7, 16), 12), FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, ClosePrice = 16484f },
			};

			return list.Take(number);
		}
	}
}
