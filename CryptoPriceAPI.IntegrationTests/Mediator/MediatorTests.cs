using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoPriceAPI.IntegrationTests.Mediator
{
	public abstract class MediatorTests : Xunit.IClassFixture<CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory>
	{
		private readonly Microsoft.Extensions.DependencyInjection.IServiceScope scope;

		protected readonly MediatR.IMediator _mediator;
		protected readonly CryptoPriceAPI.Data.CryptoPriceAPIContext _cryptoPriceAPIContext;
		protected readonly System.Collections.Generic.IReadOnlyList<CryptoPriceAPI.Data.Entities.Source> _sources;

		public MediatorTests(CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory factory)
		{
			scope = factory.Services.CreateAsyncScope();

			_mediator = scope.ServiceProvider.GetRequiredService<MediatR.IMediator>();

			_cryptoPriceAPIContext = scope.ServiceProvider.GetRequiredService<CryptoPriceAPI.Data.CryptoPriceAPIContext>();

			_sources = new System.Collections.Generic.List<CryptoPriceAPI.Data.Entities.Source>(_cryptoPriceAPIContext.Sources.AsNoTracking());
		}

		protected System.Collections.Generic.IEnumerable<CryptoPriceAPI.Data.Entities.Price> GetRandomPrices(System.Int32 number = System.Int32.MaxValue)
		{
			System.Collections.Generic.List<CryptoPriceAPI.Data.Entities.Price> list = new()
			{
				new CryptoPriceAPI.Data.Entities.Price { SourceId = _sources[0].Id, DateAndHour = new (new (2022,  1,  1), 0), FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, ClosePrice = 47039.03f },
				new CryptoPriceAPI.Data.Entities.Price { SourceId = _sources[1].Id, DateAndHour = new (new (2022,  3, 16), 0), FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, ClosePrice = 40652.54f },
				new CryptoPriceAPI.Data.Entities.Price { SourceId = _sources[0].Id, DateAndHour = new (new (2022,  7,  1), 0), FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, ClosePrice = 19460.875f },
				new CryptoPriceAPI.Data.Entities.Price { SourceId = _sources[1].Id, DateAndHour = new (new (2022, 11, 16), 0), FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, ClosePrice = 16484f },
			};

			return list.Take(number);
		}
	}
}
