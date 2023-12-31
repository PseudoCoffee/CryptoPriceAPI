using Microsoft.EntityFrameworkCore;
using System.Reflection;
using CryptoPriceAPI.Services.Helper;

namespace CryptoPriceAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Microsoft.AspNetCore.Builder.WebApplicationBuilder builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

			builder.Logging.ClearProviders().AddConsole();

			builder.Configuration.AddJsonFile("priceSources.json");

			builder.Services.Configure<CryptoPriceAPI.Services.Configuration.PriceSources>((settings) =>
			{
				builder.Configuration.GetSection("PriceSources").Bind(settings);
			});

			// Add services to the container.
			System.String connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new System.InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<CryptoPriceAPI.Data.CryptoPriceAPIContext>(options => options.UseNpgsql(connectionString));
			builder.Services.AddScoped<CryptoPriceAPI.Data.CryptoPriceAPIQueryContext>();

			builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(Program).Assembly));

			System.Collections.Generic.IEnumerable<System.Type> types = Assembly.GetExecutingAssembly().GetTypes()
				.Where(type => type.IsDefined(typeof(PriceSourceNameAttribute)));

			foreach (System.Type type in types)
			{
				PriceSourceNameAttribute priceSourceNameAttribute = (PriceSourceNameAttribute)Attribute.GetCustomAttribute(type, typeof(PriceSourceNameAttribute))!;

				builder.Services.AddScoped((serviceProvider) =>
				{
					return (Services.Interfaces.ICryptoService)ActivatorUtilities.CreateInstance(serviceProvider, type, priceSourceNameAttribute.PriceSourceKey);
				});
			}

			builder.Services.AddScoped<CryptoPriceAPI.Services.Interfaces.IAggregationService<CryptoPriceAPI.DTOs.PriceDTO>, CryptoPriceAPI.Services.AverageService>();
			builder.Services.AddTransient<System.Net.Http.HttpMessageHandler, System.Net.Http.HttpClientHandler>();
			builder.Services.AddTransient<CryptoPriceAPI.Services.Interfaces.IExternalAPICaller, CryptoPriceAPI.Services.ExternalAPICaller>();

			builder.Services.AddControllers().AddJsonOptions(config =>
			{
				config.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
			});

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options => {
				var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
			});


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
				app.UseSwagger(options =>
				{
					options.SerializeAsV2 = true;
				});
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
				});
			}

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}