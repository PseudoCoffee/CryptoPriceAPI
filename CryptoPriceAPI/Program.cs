using Microsoft.EntityFrameworkCore;

namespace CryptoPriceAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Microsoft.AspNetCore.Builder.WebApplicationBuilder builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

			builder.Logging.ClearProviders().AddConsole();

			builder.Services.Configure<CryptoPriceAPI.Services.Configuration.PriceSources>((settings) =>
			{
				builder.Configuration.GetSection("PriceSources").Bind(settings);
			});

			// Add services to the container.
			System.String connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new System.InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<CryptoPriceAPI.Data.CryptoPriceAPIContext>(options => options.UseNpgsql(connectionString));
			builder.Services.AddScoped<CryptoPriceAPI.Data.CryptoPriceAPIQueryContext>();

			builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(Program).Assembly));

			builder.Services.AddScoped<CryptoPriceAPI.Services.Interfaces.ICryptoService<CryptoPriceAPI.DTOs.BitstampDTO>, CryptoPriceAPI.Services.BitstampService>((serviceProvider) =>
			{
				return ActivatorUtilities.CreateInstance<CryptoPriceAPI.Services.BitstampService>(serviceProvider, "bitstamp");
			});

			builder.Services.AddControllers();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			Microsoft.AspNetCore.Builder.WebApplication app = builder.Build();

			using (Microsoft.Extensions.DependencyInjection.IServiceScope scope = app.Services.CreateScope())
			{
				using CryptoPriceAPI.Data.CryptoPriceAPIContext context = scope.ServiceProvider.GetService<CryptoPriceAPI.Data.CryptoPriceAPIContext>()!;

				context.Database.EnsureCreated();

				foreach (System.String source in builder.Configuration.GetSection("PriceSources").GetChildren().Select(source => source.Key))
				{
					if (!context.Sources.Any(s => s.Name == source))
					{
						context.Sources.Add(new CryptoPriceAPI.Data.Entities.Source { Id = System.Guid.NewGuid(), Name = source });
					}
				}

				context.SaveChanges();
			}

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}