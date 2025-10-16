using FortyOne.OrchestratR;
using FortyOne.OrchestratR.Extensions;

namespace Samples.WebApp.Features.Books
{
    public class UpdateBookHandler : IRequestHandler<UpdateBookRequest, Result>
    {
        public async Task<Result> HandleAsync(UpdateBookRequest request, CancellationToken cancellationToken)
        {
            // Do nothing, just a sample

            return Result.Success();
        }
    }
}
