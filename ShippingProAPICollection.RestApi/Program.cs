using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ShippingProAPICollection.RestApi.Entities;
using System.Reflection;

namespace ShippingProAPICollection.RestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
            .AddNewtonsoftJson(options => 
            { 
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; 
                options.SerializerSettings.Converters.Add(new StringEnumConverter()); 
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc; 
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                foreach (var filePath in Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ""), "*.xml"))
                {
                    try
                    {
                        c.IncludeXmlComments(filePath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShippingProApiCollection", Version = "1.0.0" });
              
                c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
                c.UseOneOfForPolymorphism();
      
            });

            builder.Services.AddSwaggerGenNewtonsoftSupport();

            builder.Services.AddMemoryCache();


            ApplicationSettingService applicationSettingService = new ApplicationSettingService();
            builder.Services.AddSingleton(applicationSettingService);
            builder.Services.AddSingleton(applicationSettingService.BuildCollectionSettings());

            builder.Services.AddScoped<ShippingProAPICollectionService>();


            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShippApiCollection V1");
                c.DisplayRequestDuration();
                c.EnableDeepLinking();
                c.ShowExtensions();
            });

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
