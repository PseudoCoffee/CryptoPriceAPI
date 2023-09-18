using Microsoft.EntityFrameworkCore;

namespace CryptoPriceAPI.Data
{
	public class CryptoPriceAPIQueryContext
	{
		private readonly CryptoPriceAPI.Data.CryptoPriceAPIContext _cryptoPriceAPIContext;

		public CryptoPriceAPIQueryContext(CryptoPriceAPI.Data.CryptoPriceAPIContext cryptoPriceAPIContext)
		{
			_cryptoPriceAPIContext = cryptoPriceAPIContext;
		}

		public System.Linq.IQueryable<CryptoPriceAPI.Data.Entities.Source> Sources => _cryptoPriceAPIContext.Sources.AsNoTracking();
		
		public System.Linq.IQueryable<CryptoPriceAPI.Data.Entities.Price> Prices => _cryptoPriceAPIContext.Prices.AsNoTracking();
	}
}
