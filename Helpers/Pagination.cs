namespace DatingApp.API.Helpers
{
    public class Pagination
    {
        public int Page { get; set; }
        
        public int Limit { get; set; }
        
        public int Total { get; set; }
        
        public int TotalPages { get; set; }

        public Pagination(int page, int limit, int total, int totalPages)
        {
            this.Page = page;
            this.Limit = limit;
            this.Total = total;
            this.TotalPages = totalPages;
        }
    }
}