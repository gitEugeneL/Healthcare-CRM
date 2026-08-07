namespace Api.ApiConfiguration;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}