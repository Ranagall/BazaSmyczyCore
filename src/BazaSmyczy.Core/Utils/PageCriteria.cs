namespace BazaSmyczy.Core.Utils
{
    public class PageCriteria
    {
        public string search { get; set; }
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 20;
    }
}
