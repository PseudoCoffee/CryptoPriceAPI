using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoPriceAPI.IntegrationTests
{
	public class IntegrationTestWebAppFactory : Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<CryptoPriceAPI.Program>, Xunit.IAsyncLifetime
	{
		private readonly Testcontainers.PostgreSql.PostgreSqlContainer dbContainer = new Testcontainers.PostgreSql.PostgreSqlBuilder()
			.WithImage("postgres:15")
			.WithDatabase("CryptoPriceAPI.IntegrationTests")
			.WithUsername("postgress_test")
			.WithPassword("postgress_test")
			.Build();

		protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
		{
			builder.ConfigureTestServices(services =>
			{
				var descriptor = services.SingleOrDefault(service => service.ServiceType == typeof(Microsoft.EntityFrameworkCore.DbContextOptions<CryptoPriceAPI.Data.CryptoPriceAPIContext>));

				if (descriptor is not null)
				{
					services.Remove(descriptor);
				}

				services.AddDbContext<CryptoPriceAPI.Data.CryptoPriceAPIContext>(options =>
				{
					options.UseNpgsql(dbContainer.GetConnectionString());
				});
			});
		}

		public Task InitializeAsync()
		{
			return dbContainer.StartAsync();
		}

		public new Task DisposeAsync()
		{
			return dbContainer.StopAsync();
		}
	}
}
