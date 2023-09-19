namespace CryptoPriceAPI.Services.Interfaces
{
	public abstract class ACryptoService<ExternalDTO> : ICryptoService where ExternalDTO : CryptoPriceAPI.DTOs.Interfaces.IExternalDTO
	{
		protected readonly MediatR.IMediator _mediator;
		protected readonly Microsoft.Extensions.Logging.ILogger<ACryptoService<ExternalDTO>> _logger;
		protected readonly System.String _sourceName;
		protected readonly CryptoPriceAPI.Services.Configuration.CryptoConfiguration _cryptoConfiguration;

		public ACryptoService(MediatR.IMediator mediator, Microsoft.Extensions.Logging.ILogger<ACryptoService<ExternalDTO>> logger, System.String sourceName, Microsoft.Extensions.Options.IOptions<CryptoPriceAPI.Services.Configuration.PriceSources> options)
		{
			if (string.IsNullOrEmpty(sourceName))
			{
				throw new ArgumentException($"'{nameof(sourceName)}' cannot be null or empty.", nameof(sourceName));
			}

			if (options is null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_sourceName = sourceName;
			_cryptoConfiguration = options.Value[sourceName];
		}

		public async Task<CryptoPriceAPI.DTOs.PriceDTO> GetPrice(System.DateOnly date, System.Int32 hour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD)
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
				ExternalDTO externalDTO = await CallExternalAPI(date, hour, financialInstrumentName);

				_logger.LogInformation("Price fetched from external API.");
				priceDTO = ConvertDTO(externalDTO, date, hour, financialInstrumentName);

				await _mediator.Send(new CryptoPriceAPI.Commands.AddPriceCommand(source.Id, System.DateOnly.FromDateTime(priceDTO.DateAndHour), priceDTO.DateAndHour.Hour, priceDTO.FinancialInstrument, priceDTO.ClosePrice));
			}
			else
			{
				_logger.LogInformation("Price found in database.");
				priceDTO = new CryptoPriceAPI.DTOs.PriceDTO
				{
					DateAndHour = price.DateAndHour,
					FinancialInstrument = financialInstrumentName,
					ClosePrice = price.ClosePrice,
				};
			}

			return priceDTO;
		}

		public async Task<ExternalDTO> CallExternalAPI(System.DateOnly date, System.Int32 hour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, System.Int32 limit = 1)
		{
			_logger.LogInformation("CallExternalAPI({@0}, {@1}, {@2})", date, hour, financialInstrumentName);

			System.DateTime dateHour = date.ToDateTime(new TimeOnly(hour, 0));

			System.Int64 startTime = ((DateTimeOffset)dateHour).ToUnixTimeSeconds();
			System.Int64? endTime = null == _cryptoConfiguration.EndFormat ? null : startTime + 3600;

			if (_cryptoConfiguration.TimeFormat == CryptoPriceAPI.Services.Configuration.TimeFormat.Milliseconds)
			{
				startTime *= 1000;
				endTime *= 1000;
			}

			System.String financialInstrumentNameString = financialInstrumentName.ToString();

			financialInstrumentNameString =
				_cryptoConfiguration.UppercaseFinancialInstrument ?
					financialInstrumentNameString.ToUpperInvariant() :
					financialInstrumentNameString.ToLowerInvariant();

			System.Collections.Generic.List<System.String> queryParams = new()
			{
				System.String.Format(_cryptoConfiguration.StartFormat, startTime)
			};

			if (null != _cryptoConfiguration.EndFormat)
			{
				queryParams.Add(System.String.Format(_cryptoConfiguration.EndFormat, endTime!.Value));
			}
			queryParams.Add(System.String.Format(_cryptoConfiguration.LimitFormat, limit));

			System.String url = System.String.Concat(
				System.String.Format(_cryptoConfiguration.CandleUrlFormat, financialInstrumentNameString),
				System.String.Join('&', queryParams));

			System.String jsonReply;
			using System.Net.Http.HttpClient client = new();
			{
				System.Net.Http.HttpResponseMessage reply = await client.GetAsync(url);

				jsonReply = await reply.Content.ReadAsStringAsync();
			}

			System.Text.Json.JsonSerializerOptions options = new()
			{
				NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString | System.Text.Json.Serialization.JsonNumberHandling.WriteAsString,
				PropertyNameCaseInsensitive = true
			};

			ExternalDTO externalDTO = System.Text.Json.JsonSerializer.Deserialize<ExternalDTO>(jsonReply, options)!;

			return externalDTO;
		}

		public CryptoPriceAPI.DTOs.PriceDTO ConvertDTO(ExternalDTO externalDTO, DateOnly dateOnly, System.Int32 hour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD)
		{
			System.DateTime dateAndHour = dateOnly.ToDateTime(new TimeOnly(hour, 0));

			return new CryptoPriceAPI.DTOs.PriceDTO
			{
				DateAndHour = dateAndHour,
				FinancialInstrument = financialInstrumentName,
				ClosePrice = externalDTO.GetCloseOHCL()
			};
		}
	}
}
