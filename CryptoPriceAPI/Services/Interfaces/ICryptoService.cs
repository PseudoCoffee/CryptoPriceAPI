using System.Text.Json;
using System.Text.Json.Serialization;

namespace CryptoPriceAPI.Services.Interfaces
{
	public abstract class ICryptoService<ExternalDTO> where ExternalDTO : class
	{

		protected readonly Microsoft.Extensions.Logging.ILogger<BitstampService> _logger;
		protected readonly System.String _sourceName;
		protected readonly CryptoPriceAPI.Services.Configuration.CryptoConfiguration _cryptoConfiguration;

		public ICryptoService(Microsoft.Extensions.Logging.ILogger<BitstampService> logger, System.String sourceName, Microsoft.Extensions.Options.IOptions<CryptoPriceAPI.Services.Configuration.PriceSources> options)
		{
			if (string.IsNullOrEmpty(sourceName))
			{
				throw new ArgumentException($"'{nameof(sourceName)}' cannot be null or empty.", nameof(sourceName));
			}

			if (options is null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_sourceName = sourceName;
			_cryptoConfiguration = options.Value[sourceName];

			if (null == _cryptoConfiguration.StepFormat && null == _cryptoConfiguration.EndFormat)
			{
				throw new System.InvalidOperationException("StepFormat and EndFormat cannot be both null.");
			}
		}

		public abstract Task<CryptoPriceAPI.DTOs.PriceDTO> GetPrice(System.DateOnly date, System.Int32 hour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName);

		protected async Task<ExternalDTO> CallExternalAPI(System.DateOnly date, System.Int32 hour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName, System.Int32 interval = 3600, System.Int32 limit = 1)
		{
			_logger.LogInformation("CallExternalAPI({@0}, {@1}, {@2})", date, hour, financialInstrumentName);

			System.DateTime dateHour = date.ToDateTime(new TimeOnly(hour, 0));

			System.Int64 startTime = ((DateTimeOffset)dateHour).ToUnixTimeSeconds();
			System.Int64? step = null == _cryptoConfiguration.StepFormat ? null : interval;
			System.Int64? endTime = null == _cryptoConfiguration.EndFormat ? null : startTime + interval;

			if (_cryptoConfiguration.TimeFormat == CryptoPriceAPI.Services.Configuration.TimeFormat.Milliseconds)
			{
				startTime *= 1000;
				step *= 1000;
				endTime *= 1000;
			}

			System.String financialInstrumentNameString = financialInstrumentName.ToString().ToLowerInvariant();

			System.Collections.Generic.List<System.String> queryParams = new()
			{
				System.String.Format(_cryptoConfiguration.StartFormat, startTime)
			};

			if (null != _cryptoConfiguration.EndFormat)
			{
				queryParams.Add(System.String.Format(_cryptoConfiguration.EndFormat, endTime!.Value));
			}
			else
			{
				queryParams.Add(System.String.Format(_cryptoConfiguration.StepFormat!, step!.Value));
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
			var options = new JsonSerializerOptions
			{
				NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString,
				PropertyNameCaseInsensitive = true
			};
			ExternalDTO bitstampDto = System.Text.Json.JsonSerializer.Deserialize<ExternalDTO>(jsonReply, options)!;

			return bitstampDto;
		}

		protected abstract CryptoPriceAPI.DTOs.PriceDTO ConvertDTO(ExternalDTO bitstampDTO, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName);
	}
}
