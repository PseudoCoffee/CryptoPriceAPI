using CryptoPriceAPI.Data;
using CryptoPriceAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoPriceAPI.Queries
{
	public class GetSourceByNameQuery : MediatR.IRequest<CryptoPriceAPI.Data.Entities.Source?>
	{
		public System.String Name { get; set; }

		/// <summary>
		/// Get price source by name.
		/// </summary>
		/// <param name="name"> Name of the source. </param>
		public GetSourceByNameQuery(System.String name)
		{
			Name = name;
		}
	}

	public class GetSourceByNameQueryHandler : MediatR.IRequestHandler<GetSourceByNameQuery, CryptoPriceAPI.Data.Entities.Source?>
	{
		private readonly CryptoPriceAPI.Data.CryptoPriceAPIQueryContext _queryContext;

		public GetSourceByNameQueryHandler(CryptoPriceAPI.Data.CryptoPriceAPIQueryContext queryContext)
		{
			_queryContext = queryContext;
		}

		public async Task<Source?> Handle(GetSourceByNameQuery request, CancellationToken cancellationToken)
		{
			return await _queryContext.Sources.SingleOrDefaultAsync(source => source.Name == request.Name, cancellationToken: cancellationToken).ConfigureAwait(false);
		}
	}
}
