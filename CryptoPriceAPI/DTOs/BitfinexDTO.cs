namespace CryptoPriceAPI.DTOs.Bitfinex
{
	public class BitfinexDTO : System.Collections.Generic.List<BitfinexData>, CryptoPriceAPI.DTOs.Interfaces.IExternalDTO
	{
		public System.Single GetCloseOHCL() => this[0].Close;
	}

	public class BitfinexData : System.Collections.Generic.List<System.Double>
	{
		public System.Single Close => (System.Single)this[2];
	}
}
