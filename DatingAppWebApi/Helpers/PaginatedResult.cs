using Microsoft.EntityFrameworkCore;

namespace DatingAppWebApi.Helpers
{
    public class PaginatedResult<T>
    {
        public PaginationMetadata Metadata { get; set; } = default!;


        public List<T> Items { get; set; } = [];



    }

    public class PaginationMetadata {

        public int currentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }


        

    }

    public static class PaginationHelper {

        public static async Task<PaginatedResult<T>> CreateAsync<T>(IQueryable<T> query, int pageNumber, int pageSize)
        {

            var count =   await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<T>
            {
                Metadata = new PaginationMetadata
                {

                    currentPage = pageNumber,
                    TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                    PageSize = pageSize,
                    TotalCount = count
                },
                Items = items
            };


        }
    }
}
