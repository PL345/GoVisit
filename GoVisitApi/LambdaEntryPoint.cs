using Amazon.Lambda.AspNetCoreServer;

namespace GoVisitApi;

public class LambdaEntryPoint : APIGatewayProxyFunction
{
    protected override void Init(IWebHostBuilder builder)
    {
        builder.UseStartup<Startup>();
    }
}