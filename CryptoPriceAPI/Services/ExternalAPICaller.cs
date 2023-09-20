namespace CryptoPriceAPI.Services
{
	public class ExternalAPICaller : CryptoPriceAPI.Services.Interfaces.IExternalAPICaller
	{
		protected readonly Microsoft.Extensions.Logging.ILogger<ExternalAPICaller> _logger;

		public ExternalAPICaller(Microsoft.Extensions.Logging.ILogger<ExternalAPICaller> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<System.String> GetJSONResponseFrom(System.String url)
		{
			_logger.LogInformation("CallExternalAPI({@0})", url);

			System.String jsonResponse;
			using System.Net.Http.HttpClient client = new();
			{
				System.Net.Http.HttpResponseMessage reply = await client.GetAsync(url);

				jsonResponse = await reply.Content.ReadAsStringAsync();
			}

			return jsonResponse;
		}

		public System.String PrepareURL(
			CryptoPriceAPI.Services.Configuration.CryptoConfiguration cryptoConfiguration,
			CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour,
			CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD,
			System.Int32 limit = 1)
		{
			_logger.LogInformation("PrepareURL({@0}, {@1}, {@2})", dateAndHour, financialInstrumentName, limit);

			System.DateTime dateHour = dateAndHour.DateTime;

			System.TimeSpan differenceToUTC = dateHour - dateHour.ToUniversalTime();

			System.Int64 startTime = ((DateTimeOffset)dateHour.Add(differenceToUTC)).ToUnixTimeSeconds();
			System.Int64? endTime = null == cryptoConfiguration.EndFormat ? null : startTime + 3600;

			if (cryptoConfiguration.TimeFormat == CryptoPriceAPI.Services.Configuration.TimeFormat.Milliseconds)
			{
				startTime *= 1000;
				endTime *= 1000;
			}

			System.String financialInstrumentNameString = financialInstrumentName.ToString();
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
			return System.String.Concat(
				System.String.Format(cryptoConfiguration.CandleUrlFormat, financialInstrumentNameString),
				System.String.Join('&', queryParams));
		}
	}
}
