using Microsoft.EntityFrameworkCore;

namespace CryptoPriceAPI.Queries
{
	public class GetPriceQuery : MediatR.IRequest<CryptoPriceAPI.Data.Entities.Price?>
	{
		public System.Guid SourceId { get; set; }

		public System.DateOnly DateOnly { get; set; }

		public System.Int32 Hour {  get; set; }

		public CryptoPriceAPI.Data.Entities.FinancialInstrument FinancialInstrumentName { get; set; }

		public GetPriceQuery(System.Guid sourceId, System.DateOnly dateOnly, System.Int32 hour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName)
		{
			SourceId = sourceId;
			DateOnly = dateOnly;
			Hour = hour;
			FinancialInstrumentName = financialInstrumentName;
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
			System.DateTime dateHour = request.DateOnly.ToDateTime(new TimeOnly(request.Hour, 0));

			var all = _queryContext.Prices.ToList();

			return await _queryContext.Prices.FirstOrDefaultAsync(price => 
					price.SourceId == request.SourceId && 
					price.DateAndHourTicks == dateHour.Ticks && 
					price.FinancialInstrumentName == request.FinancialInstrumentName,
				cancellationToken: cancellationToken).ConfigureAwait(false);
		}
	}
}
