namespace CryptoPriceAPI.DTOs
{
	public class BitstampDTO
	{
		public Data Data { get; set; } = new Data();
	}

	public class Data
	{
		public System.Collections.Generic.List<Ohlc> Ohlc { get; set; } = new();
	}

	public class Ohlc
	{
		public required System.Single Close { get; set; }

		public required Int64 Timestamp { get; set; }
	}
}
