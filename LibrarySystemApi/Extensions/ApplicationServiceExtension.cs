using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LibrarySystemApi.Data;
using LibrarySystemApi.Helper;

namespace LibrarySystemApi.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenApi();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultSqlConnection"))
            );

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<LibraryProfile>();
            });

            return services;
        }
    }
}
