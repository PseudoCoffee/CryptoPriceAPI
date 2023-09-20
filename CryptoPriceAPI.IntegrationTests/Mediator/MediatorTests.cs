using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoPriceAPI.IntegrationTests.Mediator
{
	public abstract class MediatorTests : Xunit.IClassFixture<CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory>
	{
		private readonly Microsoft.Extensions.DependencyInjection.IServiceScope scope;
		protected readonly MediatR.IMediator _mediator;
		protected readonly System.Collections.Generic.IReadOnlyList<CryptoPriceAPI.Data.Entities.Source> _sources;

		public MediatorTests(CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory factory)
		{
			scope = factory.Services.CreateAsyncScope();

			_mediator = scope.ServiceProvider.GetRequiredService<MediatR.IMediator>();

			CryptoPriceAPI.Data.CryptoPriceAPIContext dbContext = scope.ServiceProvider.GetRequiredService<CryptoPriceAPI.Data.CryptoPriceAPIContext>();

			_sources = new System.Collections.Generic.List<CryptoPriceAPI.Data.Entities.Source>(dbContext.Sources.AsNoTracking());
		}
	}
}
