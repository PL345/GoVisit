using Amazon.Lambda.AspNetCoreServer.Hosting;

namespace GoVisitApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Add Lambda hosting
        builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
        
        var startup = new Startup(builder.Configuration);
        startup.ConfigureServices(builder.Services);
        
        var app = builder.Build();
        startup.Configure(app, app.Environment);
        
        app.Run();
    }
}