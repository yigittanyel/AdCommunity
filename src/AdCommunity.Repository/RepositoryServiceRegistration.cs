using AdCommunity.Core.UnitOfWork;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Context;
using AdCommunity.Repository.Repositories;
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

        serviceCollection.AddScoped<IEventRepository, EventRepository>();
        serviceCollection.AddScoped<IUserCommunityRepository, UserCommunityRepository>();
        serviceCollection.AddScoped<IUserEventRepository, UserEventRepository>();
        serviceCollection.AddScoped<ICommunityRepository, CommunityRepository>();
        serviceCollection.AddScoped<ITicketRepository, TicketRepository>();
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<IUserTicketRepository, UserTicketRepository>();

        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

    }
}
