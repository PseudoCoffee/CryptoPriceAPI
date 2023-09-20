using CryptoPriceAPI.Data;
using CryptoPriceAPI.Data.Entities;

namespace CryptoPriceAPI.Commands
{
	public class AddPriceCommand : MediatR.IRequest
	{
		public System.Guid SourceId { get; set; }

		public CryptoPriceAPI.Data.Entities.DateAndHour DateAndHour { get; set; }

		public CryptoPriceAPI.Data.Entities.FinancialInstrument FinancialInstrument { get; set; }

		public System.Single ClosePrice { get; set; }

		/// <summary>
		/// Inserts OHLC candle price to database.
		/// </summary>
		/// <param name="sourceId"> Id of the price source. </param>
		/// <param name="dateAndHour"> Starting date and hour of the price. </param>
		/// <param name="financialInstrument"> Financial instrument name of the price (e.g.: BTCUSD, BTCEUR). </param>
		/// <param name="closePrice"> Close price of the OHLC candle. </param>
		public AddPriceCommand(System.Guid sourceId, CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrument, System.Single closePrice)
		{
			SourceId = sourceId;
			DateAndHour = dateAndHour;
			FinancialInstrument = financialInstrument;
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
				FinancialInstrument = request.FinancialInstrument,
				ClosePrice = request.ClosePrice
			};

			cryptoPriceAPIContext.Prices.Add(price);

			await cryptoPriceAPIContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}
	}
}
