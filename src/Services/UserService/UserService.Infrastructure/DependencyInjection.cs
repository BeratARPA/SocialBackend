using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Repositories;

namespace UserService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
            });

            services.AddScoped<IUserRepository, UserRepository>();

            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            using var context = new UserDbContext(optionsBuilder.Options, null);
            context.Database.EnsureCreated();
            context.Database.Migrate();

            return services;
        }
    }
}
