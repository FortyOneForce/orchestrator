using FortyOne.OrchestratR;

namespace Samples.WebApp.Features.Books
{
    public class UpdateBookRequest : IRequest
    {
        internal int Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
