using WingsOn.Infrastructure.Services;
using WingsOn.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace WingsOn.Api.ServiceExtensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IPassengerService, PassengerService>();
            services.AddSingleton<IBookingService, BookingService>();

            return services;
        }
    }
}
