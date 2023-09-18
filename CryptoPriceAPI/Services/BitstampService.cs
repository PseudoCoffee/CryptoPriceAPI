﻿namespace CryptoPriceAPI.Services
{
	public class BitstampService : CryptoPriceAPI.Services.Interfaces.ICryptoService<CryptoPriceAPI.DTOs.BitstampDTO>
	{
		private readonly MediatR.IMediator _mediator;
		private readonly Microsoft.Extensions.Logging.ILogger<BitstampService> _logger;
		private readonly System.String _sourceName;
		private readonly CryptoPriceAPI.Services.Configuration.CryptoConfiguration _cryptoConfiguration;

		public BitstampService(MediatR.IMediator mediator, Microsoft.Extensions.Logging.ILogger<BitstampService> logger, System.String sourceName, Microsoft.Extensions.Options.IOptions<CryptoPriceAPI.Services.Configuration.PriceSources> options)
		{
			_mediator = mediator;
			_logger = logger;
			_sourceName = sourceName;
			_cryptoConfiguration = options.Value[sourceName];

			if (null == _cryptoConfiguration.StepFormat && null == _cryptoConfiguration.EndFormat)
			{
				throw new System.InvalidOperationException("StepFormat and EndFormat cannot be both null.");
			}
		}

		/// <summary>
		/// Get the close price of the specified <paramref name="financialInstrumentName"/> at given <paramref name="date"/> and <paramref name="hour"/> during a 1hr windows
		/// </summary>
		/// <param name="date"></param>
		/// <param name="hour"></param>
		/// <param name="financialInstrumentName"></param>
		/// <returns></returns>
		public async override Task<CryptoPriceAPI.DTOs.PriceDTO> GetPrice(System.DateOnly date, System.Int32 hour, CryptoPriceAPI.Data.Entities.FinancialInstrumentName financialInstrumentName)
		{
			_logger.LogInformation("GetPrice({@0}, {@1})", date, hour);

			CryptoPriceAPI.Data.Entities.Price? price = await _mediator.Send(new CryptoPriceAPI.Queries.GetPriceQuery(_sourceName, date, hour, financialInstrumentName));

			if (null == price )
			{

			}


			return new CryptoPriceAPI.DTOs.PriceDTO();
		}

		public async override Task<CryptoPriceAPI.DTOs.BitstampDTO> CallExternalAPI(System.DateOnly date, System.Int32 hour, CryptoPriceAPI.Data.Entities.FinancialInstrumentName financialInstrumentName, System.Int32 interval = 3600, System.Int32 limit = 1)
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

			System.Net.Http.HttpClient client = new();			

			System.Net.Http.HttpResponseMessage reply = await client.GetAsync(url);

			var content = await reply.Content.ReadAsStringAsync();

			return new CryptoPriceAPI.DTOs.BitstampDTO();
		}
	}
}
