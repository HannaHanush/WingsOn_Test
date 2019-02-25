using WingsOn.Dal;
using Microsoft.Extensions.DependencyInjection;
using WingsOn.Domain;

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
