namespace Api.ApiConfiguration;

internal static class EndpointExtension
{
    internal static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = typeof(Program).Assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } &&
                        t.IsAssignableTo(typeof(IEndpoint)))
            .Select(Activator.CreateInstance)
            .Cast<IEndpoint>();

        foreach (var endpoint in endpoints)
            endpoint.MapEndpoint(app);
        
        return app;
    }
}