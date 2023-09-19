using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CryptoPriceAPI.IntegrationTests
{
	public abstract class BaseIntegrationTest : Xunit.IClassFixture<CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory>
	{
		private readonly Microsoft.Extensions.DependencyInjection.IServiceScope scope;
		protected readonly CryptoPriceAPI.Controller.OHLCPriceController priceController;

		public BaseIntegrationTest(CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory factory)
		{
			scope = factory.Services.CreateAsyncScope();

			var aggregationService = scope.ServiceProvider.GetRequiredService<CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO>>();
			var logger = scope.ServiceProvider.GetRequiredService<ILogger<CryptoPriceAPI.Controller.OHLCPriceController>>();
			var externalServices = scope.ServiceProvider.GetRequiredService<System.Collections.Generic.IEnumerable<CryptoPriceAPI.Services.Interfaces.ICryptoService>>();

			priceController = new Controller.OHLCPriceController(aggregationService, logger, externalServices);
		}
	}
}
