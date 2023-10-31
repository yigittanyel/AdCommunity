using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;
using AdCommunity.Repository.Repositories;
using AdSocial.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AdCommunity.Repository;

public static class RepositoryServiceRegistration
{
    public static void AddRepositoryRegistration(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<ApplicationDbContext>
            (opt => opt.UseNpgsql(configuration.GetConnectionString("CnnStr")));

        serviceCollection.AddScoped<ICommunityEventRepository, CommunityEventRepository>();
        serviceCollection.AddScoped<IUserCommunityRepository, UserCommunityRepository>();
        serviceCollection.AddScoped<IUserEventRepository, UserEventRepository>();
        serviceCollection.AddScoped<ICommunityRepository, CommunityRepository>();
        serviceCollection.AddScoped<ITicketRepository, TicketRepository>();
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<IUserTicketRepository, UserTicketRepository>();
        serviceCollection.AddScoped<ISocialRepository, SocialRepository>();

        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
    }
}
