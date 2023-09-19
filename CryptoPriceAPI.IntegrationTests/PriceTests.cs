using System;
using System.Text;

namespace CryptoPriceAPI.IntegrationTests
{
	public class PriceTests : CryptoPriceAPI.IntegrationTests.BaseIntegrationTest
	{
		private enum PriceSet
		{
			Aggregated,
			Bitfinex,
			Bitstamp
		}

		private readonly System.Collections.Generic.IReadOnlyDictionary<System.Tuple<System.DateOnly, System.Int32>, System.Single> aggregatedDatePrices = new System.Collections.Generic.Dictionary<System.Tuple<System.DateOnly, System.Int32>, System.Single>
		{
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 1, 1), 12), 47039.03f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 1, 16), 12), 43173.43f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 2, 1), 12), 38899.164f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 2, 16), 12), 43659.25f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 3, 1), 12), 44450.992f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 3, 16), 12), 40652.54f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 4, 1), 12), 45460.242f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 4, 16), 12), 40465.477f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 5, 1), 12), 37948.047f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 5, 16), 12), 29498.434f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 6, 1), 12), 31735.7f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 6, 16), 12), 21031.04f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 7, 1), 12), 19460.875f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 7, 16), 12), 20847.21f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 8, 1), 12), 23246.04f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 8, 16), 12), 23907.902f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 9, 1), 12), 19974f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 9, 16), 12), 19528.5f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 10, 1), 12), 19335.99f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 10, 16), 12), 19129.5f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 11, 1), 12), 20426.5f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 11, 16), 12), 16484f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 12, 1), 12), 17092.5f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 12, 16), 12), 16949f}
		};

		private readonly System.Collections.Generic.IReadOnlyDictionary<System.Tuple<System.DateOnly, System.Int32>, System.Single> bitfinexDatePrices = new System.Collections.Generic.Dictionary<System.Tuple<System.DateOnly, System.Int32>, System.Single>
		{
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 1, 1), 12), 47049f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 1, 16), 12), 43159f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 2, 1), 12), 38900f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 2, 16), 12), 43655f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 3, 1), 12), 44447f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 3, 16), 12), 40663.24f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 4, 1), 12), 45450f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 4, 16), 12), 40469f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 5, 1), 12), 37956f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 5, 16), 12), 29506f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 6, 1), 12), 31749f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 6, 16), 12), 21046f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 7, 1), 12), 19468f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 7, 16), 12), 20852f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 8, 1), 12), 23249f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 8, 16), 12), 23898.805f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 9, 1), 12), 19983f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 9, 16), 12), 19538f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 10, 1), 12), 19337.98f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 10, 16), 12), 19130f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 11, 1), 12), 20433f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 11, 16), 12), 16483f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 12, 1), 12), 17094f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 12, 16), 12), 16951f}
		};

		private readonly System.Collections.Generic.IReadOnlyDictionary<System.Tuple<System.DateOnly, System.Int32>, System.Single> bitstampDatePrices = new System.Collections.Generic.Dictionary<System.Tuple<System.DateOnly, System.Int32>, System.Single>
		{
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 1, 1), 12), 47029.06f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 1, 16), 12), 43187.86f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 2, 1), 12), 38898.33f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 2, 16), 12), 43663.5f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 3, 1), 12), 44454.99f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 3, 16), 12), 40641.84f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 4, 1), 12), 45470.49f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 4, 16), 12), 40461.95f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 5, 1), 12), 37940.09f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 5, 16), 12), 29490.87f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 6, 1), 12), 31722.4f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 6, 16), 12), 21016.08f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 7, 1), 12), 19453.75f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 7, 16), 12), 20842.42f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 8, 1), 12), 23243.08f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 8, 16), 12), 23917f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 9, 1), 12), 19965f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 9, 16), 12), 19519f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 10, 1), 12), 19334f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 10, 16), 12), 19129f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 11, 1), 12), 20420f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 11, 16), 12), 16485f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 12, 1), 12), 17091f},
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 12, 16), 12), 16947f}
		};

		public PriceTests(CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory factory) : base(factory)
		{

		}

		[Theory]
		[InlineData(2022, 1, 1, 12)]
		[InlineData(2022, 4, 16, 12)]
		[InlineData(2022, 6, 16, 12)]
		[InlineData(2022, 10, 1, 12)]
		[InlineData(2022, 11, 16, 12)]
		public async Task GetCandleClosePrice_ReturnsAggregatedPrice(System.Int32 year, System.Int32 month, System.Int32 day, System.Int32 hour)
		{
			// Arrange
			System.DateOnly dateOnly = new(year, month, day);
			System.Single price = GetPrice(PriceSet.Aggregated, dateOnly, hour);

			// Act
			DTOs.PriceDTO response = await this.aggregatedPriceController.GetCandleClosePrice(dateOnly, hour);

			// Assert
			Assert.Equal(price, response.ClosePrice);
		}

		[Theory]
		[InlineData(2022, 1, 1, 12)]
		[InlineData(2022, 4, 16, 12)]
		[InlineData(2022, 6, 16, 12)]
		[InlineData(2022, 10, 1, 12)]
		[InlineData(2022, 11, 16, 12)]
		public async Task GetCandleClosePrice_ReturnsBitfinexPrice(System.Int32 year, System.Int32 month, System.Int32 day, System.Int32 hour)
		{
			// Arrange
			System.DateOnly dateOnly = new(year, month, day);
			System.Single price = GetPrice(PriceSet.Bitfinex, dateOnly, hour);

			// Act
			DTOs.PriceDTO response = await bitfinexPriceController.GetCandleClosePrice(dateOnly, hour);

			// Assert
			Assert.Equal(price, response.ClosePrice);
		}

		[Theory]
		[InlineData(2022, 1, 1, 12)]
		[InlineData(2022, 4, 16, 12)]
		[InlineData(2022, 6, 16, 12)]
		[InlineData(2022, 10, 1, 12)]
		[InlineData(2022, 11, 16, 12)]
		public async Task GetCandleClosePrice_ReturnsBitstampPrice(System.Int32 year, System.Int32 month, System.Int32 day, System.Int32 hour)
		{
			// Arrange
			System.DateOnly dateOnly = new(year, month, day);
			System.Single price = GetPrice(PriceSet.Bitstamp, dateOnly, hour);

			// Act
			DTOs.PriceDTO response = await bitstampPriceController.GetCandleClosePrice(dateOnly, hour);

			// Assert
			Assert.Equal(price, response.ClosePrice);
		}

		private System.Single GetPrice(PriceSet priceSet, System.DateOnly dateOnly, System.Int32 hour) => priceSet switch
		{
			PriceSet.Aggregated => aggregatedDatePrices[new System.Tuple<System.DateOnly, System.Int32>(dateOnly, hour)],
			PriceSet.Bitfinex => bitfinexDatePrices[new System.Tuple<System.DateOnly, System.Int32>(dateOnly, hour)],
			PriceSet.Bitstamp => bitstampDatePrices[new System.Tuple<System.DateOnly, System.Int32>(dateOnly, hour)],
			_ => throw new ArgumentException(null, nameof(priceSet)),
		};
	}
}
