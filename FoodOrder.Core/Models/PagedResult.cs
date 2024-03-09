namespace FoodOrder.Core.Models
{
    public class PagedResult<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; }
    }

    public static class PagedResult
    {
        public static PagedResult<T> Create<T>(this List<T> items, int totalItems, int pageNumber, int pageSize)
        {
            return new PagedResult<T>
            {
                Items = items,
                TotalItems = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public static void CheckParameters(ref int pageNumber, ref int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 1 : pageSize;
            pageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}
