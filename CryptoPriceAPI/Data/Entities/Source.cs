namespace CryptoPriceAPI.Data.Entities
{
	[System.ComponentModel.DataAnnotations.Schema.Table($"{nameof(Source)}s")]
	[Microsoft.EntityFrameworkCore.PrimaryKey(nameof(Name))]
	public class Source
	{
		public required System.String Name { get; set; }

		public required System.String URL { get; set; }
	}
}
