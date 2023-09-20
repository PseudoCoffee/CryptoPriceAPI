namespace CryptoPriceAPI.Services.Helper
{
	[AttributeUsage(AttributeTargets.Class)]
	public class PriceSourceNameAttribute : System.Attribute
	{
		public System.String PriceSourceKey { get; set; }

		public PriceSourceNameAttribute(System.String priceSourceKey)
		{
			PriceSourceKey = priceSourceKey;
		}
	}
}
