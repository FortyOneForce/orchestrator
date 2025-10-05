using FortyOne.OrchestratR;
using Samples.WebApp.Entities;

namespace Samples.WebApp.Features.Books
{
    public class GetBookHandler : IActionHandler<GetBookRequest, List<GetBookResponse>>
    {
        private readonly List<Book> _books;

        public GetBookHandler()
        {
            _books = new List<Book>
            {
                new Book { Id = 1, Author = "Dave Mustaine", Title = "Cryptic Writings", YearPublished = 1997 },
                new Book { Id = 2, Author = "Dave Mustaine", Title = "The System Has Failed", YearPublished = 2004 },
                new Book { Id = 3, Author = "Lemmy Kilmister", Title = "Born To Lose, Live To Win", YearPublished = 2016 }
            };
        }

        public Task<List<GetBookResponse>> HandleAsync(GetBookRequest request, CancellationToken cancellationToken)
        {
            var query = _books.AsQueryable();

            if (request.Id.HasValue)
            {
                query = query.Where(x => x.Id == request.Id);
            }

            var result = query
                .Select(x => new GetBookResponse
                {
                    Id = x.Id,
                    Author = x.Author,
                    Title = x.Title,
                    YearPublished = x.YearPublished
                })
                .ToList();

            return Task.FromResult(result);
        }
    }
}
