using FortyOne.OrchestratR;

namespace Samples.WebApp.Features.Books
{
    public class BookUpdatedNotificationHandler : INotificationHandler<BookUpdatedNotification>
    {
        public async Task HandleAsync(BookUpdatedNotification notification, CancellationToken cancellationToken)
        {
            // Do nothing, just a sample
        }
    }
}
