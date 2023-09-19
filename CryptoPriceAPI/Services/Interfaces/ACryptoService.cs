using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System;

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

		public async Task<CryptoPriceAPI.DTOs.PriceDTO> GetPriceAsync(CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD)
		{
			_logger.LogInformation("GetPriceAsync({@0})", dateAndHour);

			CryptoPriceAPI.Data.Entities.Source? source = await _mediator.Send(new CryptoPriceAPI.Queries.GetSourceByNameQuery(_sourceName));

			if (source == null)
			{
				_logger.LogError($"{nameof(source)} was null.");
				throw new NullReferenceException(nameof(source));
			}

			CryptoPriceAPI.Data.Entities.Price? price = await _mediator.Send(new CryptoPriceAPI.Queries.GetPriceQuery(source.Id, dateAndHour, financialInstrumentName));
			CryptoPriceAPI.DTOs.PriceDTO priceDTO;

			if (null == price)
			{
				_logger.LogInformation("No price found in database.");
				System.String jsonReply = await CallExternalAPI(dateAndHour, financialInstrumentName);

				ExternalDTO externalDTO = ConvertJsonToDTO(jsonReply);

				_logger.LogInformation("Price fetched from external API.");
				priceDTO = ConvertExternalDTOToPriceDTO(externalDTO, dateAndHour, financialInstrumentName);

				await _mediator.Send(new CryptoPriceAPI.Commands.AddPriceCommand(source.Id, priceDTO.DateAndHour, priceDTO.FinancialInstrument, priceDTO.ClosePrice));
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

		protected async Task<System.String> CallExternalAPI(CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, System.Int32 limit = 1)
		{
			_logger.LogInformation("CallExternalAPI({@0}, {@1})", dateAndHour, financialInstrumentName);

			System.String url = PrepareURL(dateAndHour, financialInstrumentName, limit);

			System.String jsonReply;
			using System.Net.Http.HttpClient client = new();
			{
				System.Net.Http.HttpResponseMessage reply = await client.GetAsync(url);

				jsonReply = await reply.Content.ReadAsStringAsync();
			}

			return jsonReply;
		}

		protected System.String PrepareURL(CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD, System.Int32 limit = 1)
		{
			_logger.LogInformation("PrepareURL({@0}, {@1})", dateAndHour, financialInstrumentName);

			System.DateTime dateHour = dateAndHour.DateTime;

			System.TimeSpan differenceToUTC = dateHour - dateHour.ToUniversalTime();

			System.Int64 startTime = ((DateTimeOffset)dateHour.Add(differenceToUTC)).ToUnixTimeSeconds();
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

			return System.String.Concat(
				System.String.Format(_cryptoConfiguration.CandleUrlFormat, financialInstrumentNameString),
				System.String.Join('&', queryParams));
		}

		protected ExternalDTO ConvertJsonToDTO(System.String jsonDTO)
		{
			_logger.LogInformation("ConvertJsonToDTO({@0})", jsonDTO);

			System.Text.Json.JsonSerializerOptions options = new()
			{
				NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString | System.Text.Json.Serialization.JsonNumberHandling.WriteAsString,
				PropertyNameCaseInsensitive = true
			};

			ExternalDTO externalDTO;
			try
			{
				externalDTO = System.Text.Json.JsonSerializer.Deserialize<ExternalDTO>(jsonDTO, options)!;
			}
			catch (Exception ex)
			{
				_logger.LogError("{@0}", ex.Message);
				throw;
			}

			return externalDTO;
		}

		protected CryptoPriceAPI.DTOs.PriceDTO ConvertExternalDTOToPriceDTO(ExternalDTO externalDTO, CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD)
		{
			_logger.LogInformation("ConvertExternalDTOToPriceDTO({@0}, {@1})", dateAndHour, financialInstrumentName);
			return new CryptoPriceAPI.DTOs.PriceDTO
			{
				DateAndHour = dateAndHour,
				FinancialInstrument = financialInstrumentName,
				ClosePrice = externalDTO.GetCloseOHCL()
			};
		}
	}
}
