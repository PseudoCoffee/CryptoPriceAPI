namespace CryptoPriceAPI.Services.Configuration
{
	public enum TimeFormat
	{
		Seconds,
		Milliseconds
	}

	public class CryptoConfiguration
	{
		public required System.String CandleUrl { get; set; }

		public required System.String Start { get; set; }

		public System.String? End { get; set; }
		
		public System.String? Step { get; set; }
		
		public required System.String Limit { get; set; }

		public required TimeFormat TimeFormat { get; set; }
	}
}
