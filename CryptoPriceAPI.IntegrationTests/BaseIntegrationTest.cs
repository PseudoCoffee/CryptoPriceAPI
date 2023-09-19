using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CryptoPriceAPI.IntegrationTests
{
	public abstract class BaseIntegrationTest : Xunit.IClassFixture<CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory>
	{
		private readonly Microsoft.Extensions.DependencyInjection.IServiceScope scope;
		protected readonly CryptoPriceAPI.Controller.OHLCPriceController aggregatedPriceController;
		protected readonly CryptoPriceAPI.Controller.OHLCPriceController bitfinexPriceController;
		protected readonly CryptoPriceAPI.Controller.OHLCPriceController bitstampPriceController;

		public BaseIntegrationTest(CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory factory)
		{
			scope = factory.Services.CreateAsyncScope();

			var aggregationService = scope.ServiceProvider.GetRequiredService<CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO>>();
			var logger = scope.ServiceProvider.GetRequiredService<ILogger<CryptoPriceAPI.Controller.OHLCPriceController>>();
			var externalServices = scope.ServiceProvider.GetRequiredService<System.Collections.Generic.IEnumerable<CryptoPriceAPI.Services.Interfaces.ICryptoService>>();
			aggregatedPriceController = new Controller.OHLCPriceController(aggregationService, logger, externalServices);

			var bfService = externalServices.First(service => service is CryptoPriceAPI.Services.BitfinexService);
			var bfAggregationService = scope.ServiceProvider.GetRequiredService<CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO>>();
			var bfLogger = scope.ServiceProvider.GetRequiredService<ILogger<CryptoPriceAPI.Controller.OHLCPriceController>>();
			bitfinexPriceController = new CryptoPriceAPI.Controller.OHLCPriceController(bfAggregationService, bfLogger, new System.Collections.Generic.List<CryptoPriceAPI.Services.Interfaces.ICryptoService> { bfService });

			var bsService = externalServices.First(service => service is CryptoPriceAPI.Services.BitstampService);
			var bsAggregationService = scope.ServiceProvider.GetRequiredService<CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO>>();
			var bsLogger = scope.ServiceProvider.GetRequiredService<ILogger<CryptoPriceAPI.Controller.OHLCPriceController>>();
			bitstampPriceController = new CryptoPriceAPI.Controller.OHLCPriceController(bsAggregationService, bsLogger, new System.Collections.Generic.List<CryptoPriceAPI.Services.Interfaces.ICryptoService> { bsService });
		}
	}
}
