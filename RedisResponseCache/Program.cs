using Microsoft.Extensions.Configuration;
using RedisResponseCache.Interfaces;
using RedisResponseCache.Models;
using RedisResponseCache.Services;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RedisResponseCache
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.RegisterServices(builder.Configuration);

            var redisCacheSettings = new RedisCacheSettings();
            builder.Configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
            builder.Services.AddSingleton(redisCacheSettings);

            if (redisCacheSettings.Enabled)
                builder.Services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);

            builder.Services.AddSingleton<IResponseCacheService, ResponseCacheService>();

            var app = builder.Build();

            var service = app.Services.GetRequiredService<IResponseCacheService>();

            var redis = app.Services.GetRequiredService<RedisCacheSettings>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}