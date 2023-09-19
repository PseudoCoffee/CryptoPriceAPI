namespace CryptoPriceAPI.Services
{
	public class BitstampService : CryptoPriceAPI.Services.Interfaces.ICryptoService<CryptoPriceAPI.DTOs.BitstampDTO>
	{
		private readonly MediatR.IMediator _mediator;

		public BitstampService(
			MediatR.IMediator mediator,
			Microsoft.Extensions.Logging.ILogger<BitstampService> logger,
			System.String sourceName,
			Microsoft.Extensions.Options.IOptions<CryptoPriceAPI.Services.Configuration.PriceSources> options) : base(logger, sourceName, options)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Get the close price of the specified <paramref name="financialInstrumentName"/> at given <paramref name="date"/> and <paramref name="hour"/> during a 1hr windows
		/// </summary>
		/// <param name="date"></param>
		/// <param name="hour"></param>
		/// <param name="financialInstrumentName"></param>
		/// <returns></returns>
		public async override Task<CryptoPriceAPI.DTOs.PriceDTO> GetPrice(System.DateOnly date, System.Int32 hour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName)
		{
			_logger.LogInformation("GetPrice({@0}, {@1})", date, hour);

			CryptoPriceAPI.Data.Entities.Source? source = await _mediator.Send(new CryptoPriceAPI.Queries.GetSourceByNameQuery(_sourceName));

			if (source == null)
			{
				_logger.LogError($"{nameof(source)} was null.");
				throw new NullReferenceException(nameof(source));
			}

			CryptoPriceAPI.Data.Entities.Price? price = await _mediator.Send(new CryptoPriceAPI.Queries.GetPriceQuery(source.Id, date, hour, financialInstrumentName));
			CryptoPriceAPI.DTOs.PriceDTO priceDTO;

			if (null == price)
			{
				_logger.LogInformation("No price found in database.");
				CryptoPriceAPI.DTOs.BitstampDTO bitstampDTO = await CallExternalAPI(date, hour, financialInstrumentName);

				_logger.LogInformation("Price fetched from external API.");
				priceDTO = ConvertDTO(bitstampDTO, financialInstrumentName);

				await _mediator.Send(new CryptoPriceAPI.Commands.AddPriceCommand(source.Id, System.DateOnly.FromDateTime(priceDTO.DateAndHour), priceDTO.DateAndHour.Hour, priceDTO.FinancialInstrumentName, priceDTO.ClosePrice));
			}
			else
			{
				_logger.LogInformation("Price found in database.");
				priceDTO = new CryptoPriceAPI.DTOs.PriceDTO
				{
					DateAndHour = price.DateAndHour,
					FinancialInstrumentName = financialInstrumentName,
					ClosePrice = price.ClosePrice,
				};
			}

			return priceDTO;
		}

		protected override CryptoPriceAPI.DTOs.PriceDTO ConvertDTO(CryptoPriceAPI.DTOs.BitstampDTO bitstampDTO, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName)
		{
			System.DateTime dateAndHour;

			if (_cryptoConfiguration.TimeFormat == CryptoPriceAPI.Services.Configuration.TimeFormat.Seconds)
			{
				dateAndHour = System.DateTimeOffset.FromUnixTimeSeconds(bitstampDTO.Data.Ohlc[0].Timestamp).UtcDateTime;
			}
			else
			{
				dateAndHour = System.DateTimeOffset.FromUnixTimeMilliseconds(bitstampDTO.Data.Ohlc[0].Timestamp).UtcDateTime;
			}

			return new CryptoPriceAPI.DTOs.PriceDTO
			{
				DateAndHour = dateAndHour,
				FinancialInstrumentName = financialInstrumentName,
				ClosePrice = bitstampDTO.Data.Ohlc[0].Close
			};
		}
	}
}
