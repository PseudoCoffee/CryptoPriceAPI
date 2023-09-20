namespace CryptoPriceAPI.Services
{
	public class ExternalAPICaller : CryptoPriceAPI.Services.Interfaces.IExternalAPICaller
	{
		private readonly Microsoft.Extensions.Logging.ILogger<ExternalAPICaller> _logger;
		private readonly System.Net.Http.HttpMessageHandler _httpMessageHandler;

		public ExternalAPICaller(Microsoft.Extensions.Logging.ILogger<ExternalAPICaller> logger, System.Net.Http.HttpMessageHandler httpMessageHandler)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_httpMessageHandler = httpMessageHandler;
		}

		public System.Uri GenerateUri(
			CryptoPriceAPI.Services.Configuration.CryptoConfiguration cryptoConfiguration,
			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour,
			CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrument = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD,
			System.Int32 limit = 1)
		{
			_logger.LogInformation("PrepareURL({@0}, {@1}, {@2})", dateAndHour, financialInstrument, limit);

			System.DateTime dateHour = dateAndHour.DateTime;

			System.TimeSpan differenceToUTC = dateHour - dateHour.ToUniversalTime();

			System.Int64 startTime = ((DateTimeOffset)dateHour.Add(differenceToUTC)).ToUnixTimeSeconds();
			System.Int64? endTime = null == cryptoConfiguration.EndFormat ? null : startTime + 3600;

			if (cryptoConfiguration.TimeFormat == CryptoPriceAPI.Services.Configuration.TimeFormat.Milliseconds)
			{
				startTime *= 1000;
				endTime *= 1000;
			}

			System.String financialInstrumentNameString = financialInstrument.ToString();
			financialInstrumentNameString =
				cryptoConfiguration.UppercaseFinancialInstrument ?
					financialInstrumentNameString.ToUpperInvariant() :
					financialInstrumentNameString.ToLowerInvariant();

			System.Collections.Generic.List<System.String> queryParams = new()
			{
				System.String.Format(cryptoConfiguration.StartFormat, startTime)
			};

			if (null != cryptoConfiguration.EndFormat)
			{
				queryParams.Add(System.String.Format(cryptoConfiguration.EndFormat, endTime!.Value));
			}

			queryParams.Add(System.String.Format(cryptoConfiguration.LimitFormat, limit));

			System.String urlString = System.String.Concat(
				System.String.Format(cryptoConfiguration.CandleUrlFormat, financialInstrumentNameString),
				System.String.Join('&', queryParams));

			return new System.Uri(urlString);
		}

		public async Task<System.String> GetStringResponseFrom(System.Uri uri)
		{
			_logger.LogInformation("CallExternalAPI({@0})", uri);

			System.String jsonResponse;
			using System.Net.Http.HttpClient client = new(_httpMessageHandler);
			{
				System.Net.Http.HttpResponseMessage reply = await client.GetAsync(uri);

				jsonResponse = await reply.Content.ReadAsStringAsync();
			}

			return jsonResponse;
		}
	}
}
