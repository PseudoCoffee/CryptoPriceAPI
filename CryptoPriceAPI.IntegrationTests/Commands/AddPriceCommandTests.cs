namespace CryptoPriceAPI.IntegrationTests.Commands
{
	public class AddPriceCommandTests : CryptoPriceAPI.IntegrationTests.Mediator.MediatorTests
	{
		public AddPriceCommandTests(CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory factory) : base(factory)
		{
		}

		[Fact]
		public async Task AddPriceCommand_AddsOne_SucceedsAsync()
		{
			// Arrange
			System.Guid guid = _sources[0].Id;
			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(new DateOnly(2022, 6, 15), 12);
			CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD;
			System.Single closePrice = 40652.54f;

			CryptoPriceAPI.Commands.AddPriceCommand request = new(guid, dateAndHour, financialInstrument, closePrice);

			// Act
			await _mediator.Send(request);

			// Assert
		}

		[Fact]
		public async Task AddPriceCommand_AddsTwoIdentical_Prices_FailsAsync()
		{
			// Arrange
			System.Guid guid = _sources[1].Id;
			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(new DateOnly(2022, 2, 21), 12);
			CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD;
			System.Single closePrice = 40652.54f;

			CryptoPriceAPI.Commands.AddPriceCommand request = new(guid, dateAndHour, financialInstrument, closePrice);

			// Act & Assert
			await _mediator.Send(request);
			await Assert.ThrowsAsync<InvalidOperationException>(async () => await _mediator.Send(request));
		}

		[Fact]
		public async Task AddPriceCommand_AddsTwoPrices_WithDifferent_SourceGuid_SucceedsAsync()
		{
			// Arrange
			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour = new(new DateOnly(2022, 7, 5), 12);
			CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD;
			System.Single closePrice = 40652.54f;

			CryptoPriceAPI.Commands.AddPriceCommand request0 = new(_sources[0].Id, dateAndHour, financialInstrument, closePrice);
			CryptoPriceAPI.Commands.AddPriceCommand request1 = new(_sources[1].Id, dateAndHour, financialInstrument, closePrice);

			// Act 
			await _mediator.Send(request0);
			await _mediator.Send(request1);

			// Assert
		}

		[Fact]
		public async Task AddPriceCommand_AddsTwoPrices_WithDifferent_DateAndHour_SucceedsAsync()
		{
			// Arrange
			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour0 = new(new DateOnly(2022, 5, 21), 12);
			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour1 = new(new DateOnly(2022, 11, 11), 12);
			CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD;
			System.Single closePrice = 40652.54f;

			CryptoPriceAPI.Commands.AddPriceCommand request0 = new(_sources[0].Id, dateAndHour0, financialInstrument, closePrice);
			CryptoPriceAPI.Commands.AddPriceCommand request1 = new(_sources[0].Id, dateAndHour1, financialInstrument, closePrice);

			// Act 
			await _mediator.Send(request0);
			await _mediator.Send(request1);

			// Assert
		}
	}
}
