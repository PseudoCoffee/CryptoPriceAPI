namespace CryptoPriceAPI.IntegrationTests.Queries
{
	public class GetSourceByNameQueryTests : CryptoPriceAPI.IntegrationTests.Mediator.MediatorTests
	{
		public GetSourceByNameQueryTests(CryptoPriceAPI.IntegrationTests.IntegrationTestWebAppFactory factory) : base(factory)
		{

		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		public async Task GetSourceQuery_Returns_SourceAsync(System.Int32 sourceIndex)
		{
			// Arrange
			CryptoPriceAPI.Data.Entities.Source source = _sources[sourceIndex];

			// Act
			CryptoPriceAPI.Data.Entities.Source? sourceDB = await _mediator.Send(new CryptoPriceAPI.Queries.GetSourceByNameQuery(source.Name));

			// Assert
			Assert.NotNull(sourceDB);
			Assert.True(source.Id == sourceDB.Id && source.Name == sourceDB.Name);
		}

		[Fact]
		public async Task GetSourceQuery_Returns_NullAsync()
		{
			// Arrange

			// Act
			CryptoPriceAPI.Data.Entities.Source? sourceDB = await _mediator.Send(new CryptoPriceAPI.Queries.GetSourceByNameQuery("random name"));

			// Assert
			Assert.Null(sourceDB);
		}
	}
}
