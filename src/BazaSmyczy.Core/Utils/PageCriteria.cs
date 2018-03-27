namespace BazaSmyczy.Core.Utils
{
    public class PageCriteria
    {
        public string Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
