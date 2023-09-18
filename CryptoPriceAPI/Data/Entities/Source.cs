namespace CryptoPriceAPI.Data.Entities
{
	[System.ComponentModel.DataAnnotations.Schema.Table($"{nameof(Source)}s")]
	[Microsoft.EntityFrameworkCore.PrimaryKey(nameof(Id))]
	[Microsoft.EntityFrameworkCore.Index(nameof(Source.Name), IsUnique = true)]
	public class Source
	{
		[System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
		public required System.Guid Id { get; set; }

		public required System.String Name { get; set; }
	}
}