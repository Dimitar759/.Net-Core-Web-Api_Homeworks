
using Helpers;

namespace MovieApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure Database Settings
            var appSettings = builder.Configuration.GetSection("DbSettings");
            builder.Services.Configure<DatabaseSettings>(appSettings);
            var dbSettings = appSettings.Get<DatabaseSettings>();
            var connectionString = dbSettings.ConnectionString;

            // Dependency Injection Configuration
            DependencyInjectionHelper.InjectDbContext(builder.Services, connectionString);
            DependencyInjectionHelper.InjectRepositories(builder.Services);
            DependencyInjectionHelper.InjectServices(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
