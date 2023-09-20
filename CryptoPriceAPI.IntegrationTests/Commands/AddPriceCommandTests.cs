using CryptoPriceAPI.Data.Entities;
using CryptoPriceAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using System;

namespace CryptoPriceAPI.IntegrationTests.Commands
{
	public class AddPriceCommandTests : CryptoPriceAPI.IntegrationTests.Mediator.MediatorTests
	{
		public AddPriceCommandTests(CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory factory) : base(factory)
		{
			_cryptoPriceAPIContext.Prices.RemoveRange(_cryptoPriceAPIContext.Prices);

			_cryptoPriceAPIContext.SaveChanges();
		}

		[Fact]
		public async Task AddPriceCommand_AddsOne_SucceedsAsync()
		{
			// Arrange
			CryptoPriceAPI.Data.Entities.Price price = new()
			{
				SourceId = _sources[0].Id,
				DateAndHour = new(new DateOnly(2022, 6, 15), 12),
				FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD,
				ClosePrice = 40652.54f
			};

			// Act
			await _mediator.Send(new CryptoPriceAPI.Commands.AddPriceCommand(price.SourceId, price.DateAndHour, price.FinancialInstrument, price.ClosePrice));

			// Assert
			Assert.True(await _cryptoPriceAPIContext.Prices.AnyAsync(priceDB => priceDB.SourceId == price.SourceId && priceDB.DateAndHourTicks == price.DateAndHourTicks && priceDB.FinancialInstrument == price.FinancialInstrument));
		}

		[Fact]
		public async Task AddPriceCommand_AddsTwoIdentical_Prices_FailsAsync()
		{
			// Arrange
			CryptoPriceAPI.Data.Entities.Price price = new()
			{
				SourceId = _sources[1].Id,
				DateAndHour = new(new DateOnly(2022, 2, 21), 12),
				FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD,
				ClosePrice = 40652.54f
			};

			CryptoPriceAPI.Commands.AddPriceCommand request = new(price.SourceId, price.DateAndHour, price.FinancialInstrument, price.ClosePrice);

			// Act & Assert
			await _mediator.Send(request);
			await Assert.ThrowsAsync<InvalidOperationException>(async () => await _mediator.Send(request));
		}

		[Fact]
		public async Task AddPriceCommand_AddsTwoPrices_WithDifferent_SourceGuid_SucceedsAsync()
		{
			// Arrange
			System.Collections.Generic.List<CryptoPriceAPI.Data.Entities.Price> prices = new()
			{
				new ()
				{
					SourceId = _sources[0].Id,
					DateAndHour = new(new DateOnly(2022, 7, 5), 12),
					FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD,
					ClosePrice = 40652.54f
				},
				new ()
				{
					SourceId = _sources[1].Id,
					DateAndHour = new(new DateOnly(2022, 7, 5), 12),
					FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD,
					ClosePrice = 40652.54f
				},
			};

			// Act 
			foreach (CryptoPriceAPI.Data.Entities.Price price in prices)
			{
				await _mediator.Send(new CryptoPriceAPI.Commands.AddPriceCommand(price.SourceId, price.DateAndHour, price.FinancialInstrument, price.ClosePrice));
			}

			// Assert
			foreach (CryptoPriceAPI.Data.Entities.Price price in prices)
			{
				Assert.True(await _cryptoPriceAPIContext.Prices.AnyAsync(priceDB => priceDB.SourceId == price.SourceId && priceDB.DateAndHourTicks == price.DateAndHourTicks && priceDB.FinancialInstrument == price.FinancialInstrument));
			}
		}

		[Fact]
		public async Task AddPriceCommand_AddsTwoPrices_WithDifferent_DateAndHour_SucceedsAsync()
		{
			// Arrange
			System.Collections.Generic.List<CryptoPriceAPI.Data.Entities.Price> prices = new()
			{
				new ()
				{
					SourceId = _sources[0].Id,
					DateAndHour = new(new DateOnly(2022, 5, 21), 12),
					FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD,
					ClosePrice = 40652.54f
				},
				new ()
				{
					SourceId = _sources[1].Id,
					DateAndHour = new(new DateOnly(2022, 11, 11), 12),
					FinancialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD,
					ClosePrice = 40652.54f
				},
			};

			// Act 
			foreach (CryptoPriceAPI.Data.Entities.Price price in prices)
			{
				await _mediator.Send(new CryptoPriceAPI.Commands.AddPriceCommand(price.SourceId, price.DateAndHour, price.FinancialInstrument, price.ClosePrice));
			}

			// Assert
			foreach (CryptoPriceAPI.Data.Entities.Price price in prices)
			{
				Assert.True(await _cryptoPriceAPIContext.Prices.AnyAsync(priceDB => price.SourceId == priceDB.SourceId && price.DateAndHourTicks == priceDB.DateAndHourTicks && price.FinancialInstrument == priceDB.FinancialInstrument));
			}
		}
	}
}
