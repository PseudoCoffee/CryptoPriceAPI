using Microsoft.EntityFrameworkCore;

namespace CryptoPriceAPI.Data
{
	public class CryptoPriceAPIContext : Microsoft.EntityFrameworkCore.DbContext
	{
		public CryptoPriceAPIContext(Microsoft.EntityFrameworkCore.DbContextOptions options) : base(options)
		{
		}

		protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
		{
		}

		public required DbSet<CryptoPriceAPI.Data.Entities.Source> Sources { get; set; }
		
		public required DbSet<CryptoPriceAPI.Data.Entities.Price> Prices { get; set; }
	}
}
