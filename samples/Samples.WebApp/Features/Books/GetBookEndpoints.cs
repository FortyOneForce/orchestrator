using FortyOne.OrchestratR;

namespace Samples.WebApp.Features.Books
{
    public static class GetBookEndpoints
    {
        public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder endpoint)
        {
            ArgumentNullException.ThrowIfNull(endpoint);

            endpoint.MapGet("/api/books", async (IOrchestrator orchestrator, CancellationToken cancellationToken) =>
            {
                
                var books = await orchestrator.ExecuteAsync(new GetBookRequest(), cancellationToken);
                return books;
            }).Produces<List<GetBookResponse>>();

            endpoint.MapGet("/api/books/{id}", async (int id,IOrchestrator orchestrator, CancellationToken cancellationToken) =>
            {
                var books = await orchestrator.ExecuteAsync(new GetBookRequest() { Id = id }, cancellationToken);
                return books.FirstOrDefault();
            }).Produces<GetBookResponse?>();

            return endpoint;
        }
    }
}
