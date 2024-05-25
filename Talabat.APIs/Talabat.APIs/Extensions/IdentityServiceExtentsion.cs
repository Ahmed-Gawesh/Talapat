using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class IdentityServiceExtentsion
    {
        public static IServiceCollection AppIdentityServices(this IServiceCollection Services,IConfiguration configuration)
        {

            Services.AddScoped<ITokenService, TokenService>();
            Services.AddIdentity<AppUser, IdentityRole>(options =>
            { 
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

            }).AddEntityFrameworkStores<AppIdentityDbContext>();

            Services.AddAuthentication(options =>               // For Authentcation Services
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //For Authorize
            })             
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters() // Token بيتكون من اي 
                    {
                        // Registerd Claims  بساويهم ببعض

                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:ValidIssuer"],

                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:ValidAudience"],

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                    };
                });                                                 

            return Services;
        }
    }
}
