namespace CryptoPriceAPI.DTOs.Bitstamp
{
	public class BitstampDTO : CryptoPriceAPI.DTOs.Interfaces.IExternalDTO
	{
		public Data Data { get; set; } = new Data();

		public System.Single GetCloseOHCL() => Data.Ohlc[0].Close;
	}

	public class Data
	{
		public System.Collections.Generic.List<Ohlc> Ohlc { get; set; } = new();
	}

	public class Ohlc
	{
		public required System.Single Close { get; set; }
	}
}
