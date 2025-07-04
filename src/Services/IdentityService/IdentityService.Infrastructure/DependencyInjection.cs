using IdentityService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
            });

            //services.AddScoped<IUserRepository, UserRepository>();

            var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            using var context = new IdentityDbContext(optionsBuilder.Options, null);
            context.Database.EnsureCreated();
            context.Database.Migrate();

            return services;
        }
    }
}
