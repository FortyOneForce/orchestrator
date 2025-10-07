using FortyOne.OrchestratR;
using Microsoft.AspNetCore.Mvc;

namespace Samples.WebApp.Features.Books
{
    public static class BookEndpoints
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

            endpoint.MapPost("/api/books/{id}", async (int id, [FromBody] UpdateBookRequest request,IOrchestrator orchestrator, CancellationToken CancellationToken) =>
            {
                request.Id = id;

                await orchestrator.ExecuteAsync(request, CancellationToken);
                await orchestrator.NotifyAsync(new BookUpdatedNotification { Id = id }, CancellationToken);

                return Results.NoContent();
            });

            return endpoint;
        }
    }
}
