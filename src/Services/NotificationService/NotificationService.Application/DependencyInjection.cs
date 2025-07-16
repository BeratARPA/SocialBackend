using NotificationService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace NotificationService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
           
            return services;
        }
    }
}
