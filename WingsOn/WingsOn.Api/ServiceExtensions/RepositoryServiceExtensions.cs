using WingsOn.Dal;
using WingsOn.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace WingsOn.Api.ServiceExtensions
{
    public static class RepositoryServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IRepository<Person>, PersonRepository>();
            services.AddSingleton<IRepository<Booking>, BookingRepository>();
            services.AddSingleton<IRepository<Flight>, FlightRepository>();

            return services;
        }
    }
}
