namespace api.Helpers
{
    public class QueryObject
    {
        public string? CompanyName { get; set; } = null;
        
        public string? Industry { get; set; } = null;

        public string? SortBy { get; set; } = null;

        public bool IsDescending { get; set; } = false;
    }    
}
