namespace ETICARET.WebUI.Models
{
    public class PageInfo
    {
        public int TotalItems { get; set; }
        public int PageSize { get; set; } = 12;
        
        public int CurrentPage
        {
            get;
            set => field = Math.Max(1, value);
        }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

    }
}
