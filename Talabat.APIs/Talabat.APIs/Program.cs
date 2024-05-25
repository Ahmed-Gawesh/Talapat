using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middleware;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Repository.Repositories;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services Works With DI
            // Add services to the container.

            builder.Services.AddControllers(); // Add Services APIs ASP.NET


            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            #region For create dBcontext In Redis

            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
              {
                  var connected = builder.Configuration.GetConnectionString("Redis");
                  return ConnectionMultiplexer.Connect(connected);
              });

            builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            #endregion

            #region Create DbContext For Security Module 

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            #endregion

            builder.Services.AppIdentityServices(builder.Configuration); //Services For Security Module
            
            
            builder.Services.AddApplicationServices();
            builder.Services.AddSwaggerServices();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", Options =>
                {
                    Options.AllowAnyHeader().AllowAnyMethod()
                    .WithOrigins(builder.Configuration["FrontUrl"]);/*1-بياخد اي ريكوست من اي بروتوكول()
                                                                             * 2-بيستقبل اي ريكويست post delete get .....
                                                                             * 3-الجهة اللي جاي منها الريكوست زي ال frontend
                                                                        */

                });
            });

            #endregion

            var app = builder.Build();

            #region Update DataBase

            var scope=app.Services.CreateScope();  //Create services Of Scope
            var services=scope.ServiceProvider;  // Generate DI OF DbContext
            var loggerFactory=services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbContext = services.GetRequiredService<StoreContext>();
                await dbContext.Database.MigrateAsync(); //Update DataBase
                await StoreContextSeed.SeedAsync(dbContext); //For Seeding Data

                //Update DataBase For Security Module
                var identityDbContext = services.GetRequiredService<AppIdentityDbContext>();
                await identityDbContext.Database.MigrateAsync(); //Update DataBase

                // For Seed User To Test
                var seedUser = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUsersAsync(seedUser);

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Occured During Apply Migration");
               
            }
            #endregion


            #region Configure Request Into Pipelines

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
               app.UseSwaggerMiddlewares();
            }
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection(); //Convert Request From HTTP To HTTPs Encryption 
            app.UseStatusCodePagesWithRedirects("/errors/{0}"); // if endpoint not Found
            app.UseCors("MyPolicy");
            app.UseAuthentication(); //For Use Token To Login And Register.

            app.UseAuthorization();
           

            app.UseStaticFiles();
            app.MapControllers(); // Use For Routing ...Name Of EndPoint
            #endregion

            app.Run();
        }
    }
}