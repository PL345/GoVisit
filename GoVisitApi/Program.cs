using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Amazon.Lambda.AspNetCoreServer.Hosting;
using System.Text.Json;

namespace GoVisitApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // If running in AWS and a secret ARN is provided, try to fetch the MongoDB connection string
        var secretArn = Environment.GetEnvironmentVariable("MONGO_SECRET_ARN");
        if (!string.IsNullOrEmpty(secretArn))
        {
            try
            {
                var awsRegion = Environment.GetEnvironmentVariable("AWS_REGION");
                var client = string.IsNullOrEmpty(awsRegion)
                    ? new AmazonSecretsManagerClient()
                    : new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(awsRegion));

                var secretResp = await client.GetSecretValueAsync(new GetSecretValueRequest { SecretId = secretArn });
                var secretString = secretResp.SecretString;
                if (!string.IsNullOrEmpty(secretString))
                {
                    try
                    {
                        using var doc = JsonDocument.Parse(secretString);
                        if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("MONGO_URI", out var uriProp))
                        {
                            builder.Configuration["ConnectionStrings:MongoDB"] = uriProp.GetString();
                        }
                        else if (doc.RootElement.ValueKind == JsonValueKind.String)
                        {
                            builder.Configuration["ConnectionStrings:MongoDB"] = doc.RootElement.GetString();
                        }
                        else
                        {
                            if (doc.RootElement.TryGetProperty("MongoDB", out var m1)) builder.Configuration["ConnectionStrings:MongoDB"] = m1.GetString();
                            else if (doc.RootElement.TryGetProperty("MongoUri", out var m2)) builder.Configuration["ConnectionStrings:MongoDB"] = m2.GetString();
                            else if (doc.RootElement.TryGetProperty("ConnectionString", out var m3)) builder.Configuration["ConnectionStrings:MongoDB"] = m3.GetString();
                        }
                    }
                    catch (JsonException)
                    {
                        // Secret is not JSON — treat it as raw connection string
                        builder.Configuration["ConnectionStrings:MongoDB"] = secretString;
                    }
                }
            }
            catch
            {
                // Intentionally swallow — missing connection will be surfaced later in logs
            }
        }

        // Add Lambda hosting
        builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

        var startup = new Startup(builder.Configuration);
        startup.ConfigureServices(builder.Services);

        var app = builder.Build();
        startup.Configure(app, app.Environment);

        app.Run();
    }
}