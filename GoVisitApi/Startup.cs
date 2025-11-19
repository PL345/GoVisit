using MongoDB.Driver;
using Microsoft.OpenApi.Models;
using GoVisit.Core.Models;
using GoVisit.Core.Interfaces;
using GoVisit.Application.Services;
using GoVisit.Infrastructure.Repositories;
using GoVisitApi.Middleware;

namespace GoVisitApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo 
            { 
                Title = "GoVisit - Government Appointment Scheduler API", 
                Version = "v1",
                Description = "מערכת זימון תורים עבור משרדי הממשלה",
                Contact = new OpenApiContact
                {
                    Name = "Government IT Department"
                }
            });
        });

        var connectionString = Configuration.GetConnectionString("MongoDB") 
            ?? Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
        
        services.AddSingleton<IMongoClient>(s => new MongoClient(connectionString));
        services.AddScoped(s => s.GetRequiredService<IMongoClient>().GetDatabase("GoVisitDB"));
        
        // Register repositories
        services.AddScoped<IRepository<Office>>(provider => 
            new MongoRepository<Office>(provider.GetRequiredService<IMongoDatabase>(), "offices"));
        services.AddScoped<IRepository<Service>>(provider => 
            new MongoRepository<Service>(provider.GetRequiredService<IMongoDatabase>(), "services"));
        services.AddScoped<IRepository<Citizen>>(provider => 
            new MongoRepository<Citizen>(provider.GetRequiredService<IMongoDatabase>(), "citizens"));
        services.AddScoped<IRepository<OfficeSchedule>>(provider => 
            new MongoRepository<OfficeSchedule>(provider.GetRequiredService<IMongoDatabase>(), "office_schedules"));
        services.AddScoped<IRepository<Appointment>>(provider => 
            new MongoRepository<Appointment>(provider.GetRequiredService<IMongoDatabase>(), "appointments"));
        
        // Register services
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IOfficeService, OfficeService>();
        services.AddScoped<DataSeederService>();
        
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoVisit API v1");
            c.RoutePrefix = "swagger";
            c.DocumentTitle = "GoVisit - Government Appointment Scheduler";
        });

        app.UseRouting();
        app.UseCors();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}