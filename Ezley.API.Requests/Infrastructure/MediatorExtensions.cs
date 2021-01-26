using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ES.API.Requests.Infrastructure
{
   public static class MediatorExtensions
    {
        public static IServiceCollection AddMediatorHandlers(this IServiceCollection services, Assembly assembly)
        {

            // // Requests
            // services.AddTransient<IRequestHandler<GetAccount, AccountView>,
            //     GetAccountHandler>();
            return services;
        }
    }
}