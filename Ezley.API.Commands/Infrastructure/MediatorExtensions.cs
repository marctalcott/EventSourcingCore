using System.Reflection;
using Ezley.CQRS.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Ezley.API.Commands.Infrastructure
{
   public static class MediatorExtensions
    {
        public static IServiceCollection AddMediatorHandlers(this IServiceCollection services, Assembly assembly)
        {
           
            services
                // ServiceSubscriber
                .AddCommand<RegisterServiceSubscriber, RegisterServiceSubscriberHandler>()
                // Tenant
                .AddCommand<RegisterTenant, RegisterTenantHandler>();
          
            return services;
        }
        private static IServiceCollection AddCommand<TRequest, THandler>(this IServiceCollection services)
            where TRequest : IRequest
            where THandler : class, IRequestHandler<TRequest>
        {
            services.AddTransient<IRequestHandler<TRequest, Unit>, THandler>();
            return services;
        }
        
        private static IServiceCollection AddRequest<TRequest, THandler,TResponse>(this IServiceCollection services)
            where TRequest : IRequest<TResponse>
            where THandler : class, IRequestHandler<TRequest,TResponse>
            where TResponse: class
        {
            services.AddTransient<IRequestHandler<TRequest, TResponse>, THandler>();
            return services;
        }
    }
}