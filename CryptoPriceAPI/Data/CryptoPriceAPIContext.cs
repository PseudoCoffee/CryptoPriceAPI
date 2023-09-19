namespace CryptoPriceAPI.Data
{
	public class CryptoPriceAPIContext : Microsoft.EntityFrameworkCore.DbContext
	{
		public CryptoPriceAPIContext(Microsoft.EntityFrameworkCore.DbContextOptions options) : base(options)
		{
			this.Database.EnsureCreated();
		}

		protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
		{
		}

		protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CryptoPriceAPI.Data.Entities.Source>()
				.HasMany<CryptoPriceAPI.Data.Entities.Price>()
				.WithOne()
				.HasForeignKey(price => price.SourceId)
				.HasPrincipalKey(source => source.Id)
				.OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
		}

		public required Microsoft.EntityFrameworkCore.DbSet<CryptoPriceAPI.Data.Entities.Source> Sources { get; set; }

		public required Microsoft.EntityFrameworkCore.DbSet<CryptoPriceAPI.Data.Entities.Price> Prices { get; set; }
	}
}
