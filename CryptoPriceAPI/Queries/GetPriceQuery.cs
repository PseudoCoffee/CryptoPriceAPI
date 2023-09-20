using Microsoft.EntityFrameworkCore;

namespace CryptoPriceAPI.Queries
{
	public class GetPriceQuery : MediatR.IRequest<CryptoPriceAPI.Data.Entities.Price?>
	{
		public System.Guid SourceId { get; set; }

		public CryptoPriceAPI.Data.Entities.DateAndHour DateAndHour { get; set; }

		public CryptoPriceAPI.Data.Entities.FinancialInstrument FinancialInstrument { get; set; }

		public GetPriceQuery(System.Guid sourceId, CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrument)
		{
			SourceId = sourceId;
			DateAndHour = dateAndHour;
			FinancialInstrument = financialInstrument;
		}
	}

	public class GetPriceQueryHandler : MediatR.IRequestHandler<GetPriceQuery, CryptoPriceAPI.Data.Entities.Price?>
	{
		private readonly CryptoPriceAPI.Data.CryptoPriceAPIQueryContext _queryContext;

		public GetPriceQueryHandler(CryptoPriceAPI.Data.CryptoPriceAPIQueryContext queryContext)
		{
			_queryContext = queryContext;
		}

		public async Task<CryptoPriceAPI.Data.Entities.Price?> Handle(GetPriceQuery request, CancellationToken cancellationToken)
		{
			return await _queryContext.Prices.FirstOrDefaultAsync(price =>
					price.SourceId == request.SourceId &&
					price.DateAndHourTicks == request.DateAndHour.DateTime.Ticks &&
					price.FinancialInstrument == request.FinancialInstrument,
				cancellationToken: cancellationToken).ConfigureAwait(false);
		}
	}
}
