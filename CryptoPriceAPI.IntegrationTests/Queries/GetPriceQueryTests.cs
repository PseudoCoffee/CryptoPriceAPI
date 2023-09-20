using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoPriceAPI.IntegrationTests.Queries
{
	public class GetPriceQueryTests : CryptoPriceAPI.IntegrationTests.Mediator.MediatorTests
	{
		public GetPriceQueryTests(CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory factory) : base(factory)
		{
			lock (this)
			{
				InsertIfMissing(GetRandomPrices());
			}
		}

		[Fact]
		public async Task GetPriceQuery_Returns_PriceAsync()
		{
			// Arrange
			CryptoPriceAPI.Data.Entities.Price price = GetRandomPrices(1).First();

			// Act
			CryptoPriceAPI.Data.Entities.Price? priceDB = await _mediator.Send(new CryptoPriceAPI.Queries.GetPriceQuery(price.SourceId, price.DateAndHour, price.FinancialInstrument));

			// Assert
			Assert.NotNull(priceDB);
			Assert.True(price.SourceId == priceDB.SourceId && price.DateAndHourTicks == priceDB.DateAndHourTicks && price.FinancialInstrument == price.FinancialInstrument);
		}

		[Fact]
		public async Task GetPriceQuery_Returns_NullAsync()
		{
			// Arrange
			CryptoPriceAPI.Data.Entities.Price price = GetRandomPrices(1).First();

			// Act
			CryptoPriceAPI.Data.Entities.Price? priceDB = await _mediator.Send(new CryptoPriceAPI.Queries.GetPriceQuery(Guid.NewGuid(), new(new DateOnly(1999, 2, 21), 0), price.FinancialInstrument));

			// Assert
			Assert.Null(priceDB);
		}

		[Fact]
		public async Task GetPriceQuery_WithWrongSourceId_Returns_NullAsync()
		{
			// Arrange
			CryptoPriceAPI.Data.Entities.Price price = GetRandomPrices(1).First();

			// Act
			CryptoPriceAPI.Data.Entities.Price? priceDB = await _mediator.Send(new CryptoPriceAPI.Queries.GetPriceQuery(Guid.NewGuid(), price.DateAndHour, price.FinancialInstrument));

			// Assert
			Assert.Null(priceDB);
		}

		[Fact]
		public async Task GetPriceQuery_WithWrongDateAndHour_Returns_NullAsync()
		{
			// Arrange
			CryptoPriceAPI.Data.Entities.Price price = GetRandomPrices(1).First();

			// Act
			CryptoPriceAPI.Data.Entities.Price? priceDB = await _mediator.Send(new CryptoPriceAPI.Queries.GetPriceQuery(price.SourceId, new(new DateOnly(1999, 2, 21), 0), price.FinancialInstrument));

			// Assert
			Assert.Null(priceDB);
		}

		private void InsertIfMissing(System.Collections.Generic.IEnumerable<CryptoPriceAPI.Data.Entities.Price> prices)
		{
			System.Boolean saveChanges = false;
			foreach (CryptoPriceAPI.Data.Entities.Price price in prices)
			{
				if (!_cryptoPriceAPIContext.Prices.Any(priceDB => priceDB.SourceId == price.SourceId && priceDB.DateAndHourTicks == price.DateAndHourTicks && priceDB.FinancialInstrument == price.FinancialInstrument))
				{
					_cryptoPriceAPIContext.Prices.Add(price);
					saveChanges = true;
				}
			}
			if(saveChanges)
			{
				_cryptoPriceAPIContext.SaveChanges();
			}
		}
	}
}
