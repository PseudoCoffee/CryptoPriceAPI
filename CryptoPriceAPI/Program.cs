using Microsoft.EntityFrameworkCore;

namespace CryptoPriceAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Microsoft.AspNetCore.Builder.WebApplicationBuilder builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

			builder.Logging.ClearProviders().AddConsole();

			var src = builder.Configuration.GetSection("Sources");


			// Add services to the container.
			System.String connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<CryptoPriceAPI.Data.CryptoPriceAPIContext>(options => options.UseNpgsql(connectionString));
			builder.Services.AddScoped<CryptoPriceAPI.Data.CryptoPriceAPIQueryContext>();

			builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(Program).Assembly));

			builder.Services.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(builder.Configuration);

			builder.Services.AddControllers();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			Microsoft.AspNetCore.Builder.WebApplication app = builder.Build();

			//using (Microsoft.Extensions.DependencyInjection.IServiceScope scope = app.Services.CreateScope())
			//{
			//	using CryptoPriceAPI.Data.CryptoPriceAPIContext? context = scope.ServiceProvider.GetService<CryptoPriceAPI.Data.CryptoPriceAPIContext>();
			//	context!.Database.EnsureCreated();
			//}

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