using FortyOne.OrchestratR;

namespace Samples.WebApp.Features.Books
{
    public class UpdateBookHandler : IRequestHandler<UpdateBookRequest>
    {
        public async Task HandleAsync(UpdateBookRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

            // Do nothing, just a sample
        }
    }
}
