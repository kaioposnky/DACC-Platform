namespace DaccApi.Model.Responses.Statistics
{
    public class ResponseDashboardStats
    {
        public UserStats Users { get; set; } 
        public OrderStats Orders { get; set; } 
        public ProductStats Products { get; set; } 
        public ReviewStats Reviews { get; set; } 
        public EventStats Events { get; set; } 
        public NewsStats News { get; set; } 
        public AdStats Ads { get; set; } 
        public FacultyStats Faculty { get; set; } 
        public PermissionStats Permissions { get; set; } 
    }

    public class UserStats
    {
        public int Total { get; set; }
        public int Active { get; set; }
        public int Subscribers { get; set; }
        public int NewThisMonth { get; set; }
        public Dictionary<string, int> ByRole { get; set; } 
    }

    public class OrderStats
    {
        public int Total { get; set; }
        public decimal TotalRevenue { get; set; }
        public int Pending { get; set; }
        public int SalesLast30Days { get; set; }
        public Dictionary<string, int> ByStatus { get; set; } 
    }

    public class ProductStats
    {
        public int TotalActive { get; set; }
        public int LowStockCount { get; set; }
        public Dictionary<string, int> ByCategory { get; set; } 
    }

    public class ReviewStats
    {
        public int Total { get; set; }
        public double AverageRating { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; }  // 5 stars: 10, 4 stars: 5...
    }

    public class EventStats
    {
        public int Total { get; set; }
        public int Upcoming { get; set; }
        public Dictionary<string, int> ByType { get; set; } 
    }

    public class NewsStats
    {
        public int Total { get; set; }
        public Dictionary<string, int> ByCategory { get; set; } 
    }

    public class AdStats
    {
        public int TotalActive { get; set; }
        public Dictionary<string, int> ByType { get; set; } 
    }

    public class FacultyStats
    {
        public int Total { get; set; }
        public Dictionary<string, int> ByTitle { get; set; } 
    }

    public class PermissionStats
    {
        public int TotalDefinitions { get; set; }
    }
}
