using FortyOne.OrchestratR;

namespace Benchmarks.Features
{
    public static class GetBook
    {
        #region [ Request ]

        public class Request : IRequest<Response>, MediatR.IRequest<Response>
        {
            public int Id { get; set; }
        }

        #endregion

        #region [ Response ]

        public class Response
        {
            public int Id { get; set; }
            public string Title { get; set; } = null!;
        }

        #endregion

        #region [ Handler ]

        public class Handler : IRequestHandler<Request, Response>, MediatR.IRequestHandler<Request, Response>
        {
            // MediatR handler implementation
            Task<Response> MediatR.IRequestHandler<Request, Response>.Handle(Request request, CancellationToken cancellationToken)
            {
                return GetResponse(request);
            }

            // OrchestratR handler implementation
            Task<Response> IRequestHandler<Request, Response>.HandleAsync(Request request, CancellationToken cancellationToken)
            {
                return GetResponse(request);
            }

            private Task<Response> GetResponse(Request request)
            {
                return Task.FromResult(new Response
                {
                    Id = request.Id,
                    Title = $"Book {request.Id}"
                });
            }
        }

        #endregion
    }
}
