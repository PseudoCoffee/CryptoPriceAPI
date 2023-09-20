﻿using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System;

namespace CryptoPriceAPI.Services.Interfaces
{
	public abstract class ACryptoService<ExternalDTO> : ICryptoService where ExternalDTO : CryptoPriceAPI.DTOs.Interfaces.IExternalDTO
	{
		protected readonly Microsoft.Extensions.Logging.ILogger<ACryptoService<ExternalDTO>> _logger;
		protected readonly MediatR.IMediator _mediator;
		protected readonly CryptoPriceAPI.Services.Interfaces.IExternalAPICaller _externalAPICaller;
		protected readonly System.String _sourceName;
		protected readonly CryptoPriceAPI.Services.Configuration.CryptoConfiguration _cryptoConfiguration;

		public ACryptoService(
			Microsoft.Extensions.Logging.ILogger<ACryptoService<ExternalDTO>> logger,
			MediatR.IMediator mediator,
			CryptoPriceAPI.Services.Interfaces.IExternalAPICaller externalAPICaller,
			System.String? sourceName,
			Microsoft.Extensions.Options.IOptions<CryptoPriceAPI.Services.Configuration.PriceSources> options)
		{
			if (System.String.IsNullOrEmpty(sourceName))
			{
				throw new ArgumentException($"'{nameof(sourceName)}' cannot be null or empty.", nameof(sourceName));
			}

			if (options is null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_externalAPICaller = externalAPICaller ?? throw new ArgumentNullException(nameof(externalAPICaller));
			_sourceName = sourceName;
			_cryptoConfiguration = options.Value[sourceName];
		}

		public async Task<CryptoPriceAPI.DTOs.PriceDTO> GetPriceAsync(CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD)
		{
			_logger.LogInformation("GetPriceAsync({@0})", dateAndHour);

			CryptoPriceAPI.Data.Entities.Source? source = await _mediator.Send(new CryptoPriceAPI.Queries.GetSourceByNameQuery(_sourceName));

			if (source == null)
			{
				NullReferenceException exception = new(nameof(source));
				_logger.LogError(exception, "{@0}", exception.Message);

				throw exception;
			}

			CryptoPriceAPI.Data.Entities.Price? price = await _mediator.Send(new CryptoPriceAPI.Queries.GetPriceQuery(source.Id, dateAndHour, financialInstrumentName));
			CryptoPriceAPI.DTOs.PriceDTO priceDTO;

			if (null == price)
			{
				_logger.LogInformation("No price found in database.{@0}Build url for external api.", System.Environment.NewLine);
				System.Uri uri = _externalAPICaller.GenerateUri(_cryptoConfiguration, dateAndHour, financialInstrumentName);

				_logger.LogInformation("Call external API for price.");
				System.String jsonResponse = await _externalAPICaller.GetStringResponseFrom(uri);

				_logger.LogInformation("Convert json to externalDTO.");
				ExternalDTO externalDTO = ConvertJsonToExternalDTO(jsonResponse);

				_logger.LogInformation("Convert externalDTO to priceDTO.");
				priceDTO = ConvertExternalDTOToPriceDTO(externalDTO, dateAndHour, financialInstrumentName);

				_logger.LogInformation("Cache the price in the DB.");
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

		private ExternalDTO ConvertJsonToExternalDTO(System.String jsonDTO)
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

		private CryptoPriceAPI.DTOs.PriceDTO ConvertExternalDTOToPriceDTO(ExternalDTO externalDTO, CryptoPriceAPI.Data.Entities.DateAndHour dateAndHour, CryptoPriceAPI.Data.Entities.FinancialInstrument financialInstrumentName = CryptoPriceAPI.Data.Entities.FinancialInstrument.BTCUSD)
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
