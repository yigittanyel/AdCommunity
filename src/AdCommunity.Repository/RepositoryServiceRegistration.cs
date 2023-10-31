using AdCommunity.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdCommunity.Repository;

public static class RepositoryServiceRegistration
{
    public static void AddRepositoryRegistration(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<ApplicationDbContext>
            (opt => opt.UseNpgsql(configuration.GetConnectionString("CnnStr")));
    }
}
