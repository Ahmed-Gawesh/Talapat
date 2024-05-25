namespace Talabat.APIs.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection Services)
        {

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen(); //Generate Web For Test EndPoints
            return Services;
        }

        public static WebApplication UseSwaggerMiddlewares(this WebApplication app) 
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }

    }
}
