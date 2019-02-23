using WingsOn.Dal;
using Microsoft.Extensions.DependencyInjection;

namespace WingsOn.Api.ServiceExtensions
{
    public static class RepositoryServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<PersonRepository>();
            services.AddSingleton<BookingRepository>();
            services.AddSingleton<FlightRepository>();

            return services;
        }
    }
}
