using Microsoft.Extensions.DependencyInjection;

namespace CryptoPriceAPI.IntegrationTests.Controllers.Interfaces
{
	public abstract class AControllerTests : Xunit.IClassFixture<CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory>
	{
		private readonly Microsoft.Extensions.DependencyInjection.IServiceScope scope;
		protected readonly CryptoPriceAPI.Controllers.OHLCPriceController _aggregatedPriceController;
		protected readonly CryptoPriceAPI.Controllers.OHLCPriceController _bitfinexPriceController;
		protected readonly CryptoPriceAPI.Controllers.OHLCPriceController _bitstampPriceController;

		public AControllerTests(CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory factory)
		{
			scope = factory.Services.CreateAsyncScope();

			var logger = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<CryptoPriceAPI.Controllers.OHLCPriceController>>();
			var aggregationService = scope.ServiceProvider.GetRequiredService<CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO>>();
			var externalServices = scope.ServiceProvider.GetRequiredService<System.Collections.Generic.IEnumerable<CryptoPriceAPI.Services.Interfaces.ICryptoService>>();
			_aggregatedPriceController = new CryptoPriceAPI.Controllers.OHLCPriceController(logger, aggregationService, externalServices);

			var bfLogger = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<CryptoPriceAPI.Controllers.OHLCPriceController>>();
			var bfAggregationService = scope.ServiceProvider.GetRequiredService<CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO>>();
			var bfService = externalServices.First(service => service is CryptoPriceAPI.Services.BitfinexService);
			_bitfinexPriceController = new CryptoPriceAPI.Controllers.OHLCPriceController(bfLogger, bfAggregationService, new System.Collections.Generic.List<CryptoPriceAPI.Services.Interfaces.ICryptoService> { bfService });

			var bsLogger = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<CryptoPriceAPI.Controllers.OHLCPriceController>>();
			var bsAggregationService = scope.ServiceProvider.GetRequiredService<CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO>>();
			var bsService = externalServices.First(service => service is CryptoPriceAPI.Services.BitstampService);
			_bitstampPriceController = new CryptoPriceAPI.Controllers.OHLCPriceController(bsLogger, bsAggregationService, new System.Collections.Generic.List<CryptoPriceAPI.Services.Interfaces.ICryptoService> { bsService });
		}
	}
}
