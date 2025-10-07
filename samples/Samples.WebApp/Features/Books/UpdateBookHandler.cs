using FortyOne.OrchestratR;

namespace Samples.WebApp.Features.Books
{
    public class UpdateBookHandler : IRequestHandler<UpdateBookRequest>
    {
        public async Task HandleAsync(UpdateBookRequest request, CancellationToken cancellationToken)
        {
            // Do nothing, just a sample
        }
    }
}
