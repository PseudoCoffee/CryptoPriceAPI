namespace CryptoPriceAPI.DTOs
{
	public class BitstampDTO : CryptoPriceAPI.DTOs.Interfaces.IExternalDTO
	{
		public BitstampData Data { get; set; } = new BitstampData();

		public System.Single GetCloseOHCL() => Data.Ohlc[0].Close;
	}

	public class BitstampData
	{
		public System.Collections.Generic.List<Ohlc> Ohlc { get; set; } = new();
	}

	public class Ohlc
	{
		public required System.Single Close { get; set; }
	}
}
