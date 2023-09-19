using CryptoPriceAPI.Data;
using CryptoPriceAPI.Data.Entities;

namespace CryptoPriceAPI.Commands
{
	public class AddPriceCommand : MediatR.IRequest
	{
		public System.Guid SourceId { get; set; }

		public CryptoPriceAPI.Data.Entities.DateAndHour DateAndHour { get; set; }

		public CryptoPriceAPI.Data.Entities.FinancialInstrument FinancialInstrumentName { get; set; }

		public System.Single ClosePrice { get; set; }

		public AddPriceCommand(System.Guid sourceId, CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName, System.Single closePrice)
		{
			SourceId = sourceId;
			DateAndHour = dateAndHour;
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
			CryptoPriceAPI.Data.Entities.Price price = new()
			{
				SourceId = request.SourceId,
				DateAndHour = request.DateAndHour,
				FinancialInstrumentName = request.FinancialInstrumentName,
				ClosePrice = request.ClosePrice
			};

			cryptoPriceAPIContext.Prices.Add(price);

			await cryptoPriceAPIContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}
	}
}
