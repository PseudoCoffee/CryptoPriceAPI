namespace CryptoPriceAPI.Services.Configuration
{
	public enum TimeFormat
	{
		Seconds,
		Milliseconds
	}

	public class CryptoConfiguration
	{
		public required System.String CandleUrlFormat { get; set; }

		public required System.Boolean UppercaseFinancialInstrument { get; set; }

		public System.Boolean TimeOffset {  get; set; }

		public required System.String StartFormat { get; set; }

		public System.String? EndFormat { get; set; } = default;

		public System.String? StepFormat { get; set; } = default;

		public required System.String LimitFormat { get; set; }

		public required TimeFormat TimeFormat { get; set; }
	}

	public class PriceSources : System.Collections.Generic.Dictionary<System.String, CryptoConfiguration>
	{
	}
}
