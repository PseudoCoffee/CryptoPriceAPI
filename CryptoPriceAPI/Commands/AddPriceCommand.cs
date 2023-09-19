using CryptoPriceAPI.Data;
using CryptoPriceAPI.Data.Entities;

namespace CryptoPriceAPI.Commands
{
	public class AddPriceCommand : MediatR.IRequest
	{
		public System.Guid SourceId { get; set; }

		public System.DateOnly DateOnly { get; set; }

		public System.Int32 Hour { get; set; }

		public CryptoPriceAPI.Data.Entities.FinancialInstrument FinancialInstrumentName { get; set; }

		public System.Single ClosePrice { get; set; }

		public AddPriceCommand(System.Guid sourceId, System.DateOnly dateOnly, System.Int32 hour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName, System.Single closePrice)
		{
			SourceId = sourceId;
			DateOnly = dateOnly;
			Hour = hour;
			FinancialInstrumentName = financialInstrumentName;
			ClosePrice = closePrice;
		}
	}

	public class AddPriceCommandHandler : MediatR.IRequestHandler<AddPriceCommand>
	{
		private readonly CryptoPriceAPI.Data.CryptoPriceAPIContext cryptoPriceAPIContext;

		public AddPriceCommandHandler(CryptoPriceAPI.Data.CryptoPriceAPIContext cryptoPriceAPIContext)
		{
			this.cryptoPriceAPIContext = cryptoPriceAPIContext;
		}

		public async Task Handle(AddPriceCommand request, CancellationToken cancellationToken)
		{
			System.DateTime dateAndHour = request.DateOnly.ToDateTime(new System.TimeOnly(request.Hour, 0));

			CryptoPriceAPI.Data.Entities.Price price = new()
			{
				SourceId = request.SourceId,
				DateAndHour = dateAndHour,
				FinancialInstrumentName = request.FinancialInstrumentName,
				ClosePrice = request.ClosePrice
			};

			cryptoPriceAPIContext.Prices.Add(price);

			await cryptoPriceAPIContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}
	}
}
