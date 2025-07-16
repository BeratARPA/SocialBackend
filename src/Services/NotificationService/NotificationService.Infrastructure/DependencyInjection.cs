using NotificationService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NotificationService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NotificationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
            });

            //services.AddScoped<IUserRepository, UserRepository>();

            var optionsBuilder = new DbContextOptionsBuilder<NotificationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            using var context = new NotificationDbContext(optionsBuilder.Options, null);
            context.Database.EnsureCreated();
            context.Database.Migrate();

            return services;
        }
    }
}
