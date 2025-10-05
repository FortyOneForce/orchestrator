using FortyOne.OrchestratR;
using Samples.WebApp.RequestMarkers;

namespace Samples.WebApp.Features.Books
{
    public class GetBookRequest : IActionRequest<List<GetBookResponse>>, ILoggableRequest
    {
        public int? Id { get; set; }
    }
}
