using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FoodOrder.Core.WebSocket
{
    public static class Extensions
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
                                                        PathString path,
                                                        WebSocketHandler handler)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<WebSocketManagerMiddleware>(handler));
        }
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<ConnectionManager>();

            foreach (var type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()))
            {
                if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }
    }
}
