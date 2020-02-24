namespace CodeBuddy.Api.Helpers
{
    public class PaginationHeader
    {
        public int CurrentPage { get; }
        public int TotalItems { get; }
        public int ItemsPerPage { get; }
        public int TotalPages { get; }


        public PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            CurrentPage = currentPage;
            TotalItems = totalItems;
            ItemsPerPage = itemsPerPage;
            TotalPages = totalPages;
        }        
    }
}
