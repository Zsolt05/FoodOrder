using FoodOrder.API.Middlewares;
using FoodOrder.Core.Services;
using FoodOrder.Core.Services.Init;
using FoodOrder.Core.WebSocket;
using FoodOrder.Core.WebSocket.Handlers;
using FoodOrder.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace FoodOrder.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSqlite<FoodOrderDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));

            // Add services to the container.
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<FoodInit>();
            builder.Services.AddScoped<RoleInit>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IFoodService, FoodService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddWebSocketManager();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."

                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<FoodOrderDbContext>();
                ctx.Database.Migrate();
                scope.ServiceProvider.GetRequiredService<FoodInit>().Init().Wait();
                scope.ServiceProvider.GetRequiredService<RoleInit>().Init().Wait();
            }
            app.UseCors(x => x
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            var serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;
            app.UseWebSockets();
            app.MapWebSocketManager("/api/food/ws", serviceProvider.GetService<FoodHandler>());

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
