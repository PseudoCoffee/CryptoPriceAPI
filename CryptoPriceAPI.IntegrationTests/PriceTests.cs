using System.Text;

namespace CryptoPriceAPI.IntegrationTests
{
	public class PriceTests : CryptoPriceAPI.IntegrationTests.BaseIntegrationTest
	{
		private readonly System.Collections.Generic.IReadOnlyDictionary<System.Tuple<System.DateOnly, System.Int32>, System.Single> datePrices = new System.Collections.Generic.Dictionary<System.Tuple<System.DateOnly, System.Int32>, System.Single>
		{
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  1,  1), 12), 46767.54f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  1, 16), 12), 42989.11f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  2,  1), 12), 38478.168f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  2, 16), 12), 44220.77f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  3,  1), 12), 43641.32f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  3, 16), 12), 40561.453f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  4,  1), 12), 45250.727f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  4, 16), 12), 40435.39f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  5,  1), 12), 38049.33f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  5, 16), 12), 30161.191f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  6,  1), 12), 31555.695f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  6, 16), 12), 21243.209f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  7,  1), 12), 19054.496f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  7, 16), 12), 20634.955f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  8,  1), 12), 23040.816f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  8, 16), 12), 24010.5f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  9,  1), 12), 20024.5f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022,  9, 16), 12), 19797.023f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 10,  1), 12), 19331f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 10, 16), 12), 19125f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 11,  1), 12), 20525.5f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 11, 16), 12), 16691.5f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 12,  1), 12), 17105.5f },
			{ new System.Tuple<System.DateOnly, System.Int32>(new(2022, 12, 16), 12), 17030.5f }
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
		public async Task GetCandleClosePrice_ReturnsPrice(System.Int32 year, System.Int32 month, System.Int32 day, System.Int32 hour)
		{
			// Arrange
			System.DateOnly dateOnly = new(year, month, day);
			System.Single price = GetPrice(dateOnly, hour);

			// Act
			DTOs.PriceDTO response = await this.priceController.GetCandleClosePrice(dateOnly, hour);

			// Assert
			Assert.Equal(price, response.ClosePrice);
		}

		private System.Single GetPrice(System.DateOnly dateOnly, System.Int32 hour) => datePrices[new System.Tuple<System.DateOnly, System.Int32>(dateOnly, hour)];
	}
}
