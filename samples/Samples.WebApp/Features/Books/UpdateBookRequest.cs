using FortyOne.OrchestratR;
using FortyOne.OrchestratR.Extensions;

namespace Samples.WebApp.Features.Books
{
    public class UpdateBookRequest : IRequest<Result>
    {
        internal int Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
