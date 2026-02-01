using System.Data;
using Dapper;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model.Responses.Statistics;

namespace DaccApi.Infrastructure.Repositories.Statistics
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly IRepositoryDapper _dapper;
        private readonly IDbConnection _connection;

        public StatisticsRepository(IRepositoryDapper dapper, IDbConnection connection)
        {
            _dapper = dapper;
            _connection = connection;
        }

        public async Task<ResponseDashboardStats> GetDashboardStatsAsync()
        {
            var sql = _dapper.GetQueryNamed("GetDashboardStats");
            var response = new ResponseDashboardStats();
            
            // Initialize sub-objects because DTO properties are uninitialized
            response.Users = new UserStats();
            response.Orders = new OrderStats();
            response.Products = new ProductStats();
            response.Reviews = new ReviewStats();
            response.Events = new EventStats();
            response.News = new NewsStats();
            response.Ads = new AdStats();
            response.Faculty = new FacultyStats();
            response.Permissions = new PermissionStats();

            using (var multi = await _connection.QueryMultipleAsync(sql))
            {
                // 1. Users Basic
                var userBasic = await multi.ReadFirstAsync<dynamic>();
                response.Users.Total = Convert.ToInt32(userBasic.Total);
                response.Users.Active = Convert.ToInt32(userBasic.Active);
                response.Users.Subscribers = Convert.ToInt32(userBasic.Subscribers);
                response.Users.NewThisMonth = Convert.ToInt32(userBasic.NewThisMonth);

                // 2. Users By Role
                var usersByRole = await multi.ReadAsync<dynamic>();
                response.Users.ByRole = usersByRole.Select(r => new { K = (string)r.Key, V = (int)Convert.ToInt32(r.Value) }).Where(x => x.K != null).ToDictionary(x => x.K, x => x.V);

                // 3. Orders Basic
                var orderBasic = await multi.ReadFirstAsync<dynamic>();
                response.Orders.Total = Convert.ToInt32(orderBasic.Total);
                response.Orders.TotalRevenue = Convert.ToDecimal(orderBasic.TotalRevenue);
                response.Orders.Pending = Convert.ToInt32(orderBasic.Pending);
                response.Orders.SalesLast30Days = Convert.ToInt32(orderBasic.SalesLast30Days);

                // 4. Orders By Status
                var ordersByStatus = await multi.ReadAsync<dynamic>();
                response.Orders.ByStatus = ordersByStatus.Select(r => new { K = (string)r.Key, V = (int)Convert.ToInt32(r.Value) }).Where(x => x.K != null).ToDictionary(x => x.K, x => x.V);

                // 5. Products Basic
                var productBasic = await multi.ReadFirstAsync<dynamic>();
                response.Products.TotalActive = Convert.ToInt32(productBasic.TotalActive);
                response.Products.LowStockCount = Convert.ToInt32(productBasic.LowStockCount);

                // 6. Products By Category
                var productsByCat = await multi.ReadAsync<dynamic>();
                response.Products.ByCategory = productsByCat.Select(r => new { K = (string)r.Key, V = (int)Convert.ToInt32(r.Value) }).Where(x => x.K != null).ToDictionary(x => x.K, x => x.V);

                // 7. Reviews Basic
                var reviewBasic = await multi.ReadFirstAsync<dynamic>();
                response.Reviews.Total = Convert.ToInt32(reviewBasic.Total);
                response.Reviews.AverageRating = Convert.ToDouble(reviewBasic.AverageRating);

                // 8. Reviews Distribution
                var reviewsDist = await multi.ReadAsync<dynamic>();
                response.Reviews.RatingDistribution = reviewsDist.Select(r => new { K = (int)Convert.ToInt32(r.Key), V = (int)Convert.ToInt32(r.Value) }).ToDictionary(x => x.K, x => x.V);

                // 9. Events Basic
                var eventBasic = await multi.ReadFirstAsync<dynamic>();
                response.Events.Total = Convert.ToInt32(eventBasic.Total);
                response.Events.Upcoming = Convert.ToInt32(eventBasic.Upcoming);

                // 10. Events By Type
                var eventsByType = await multi.ReadAsync<dynamic>();
                response.Events.ByType = eventsByType.Select(r => new { K = (string)r.Key, V = (int)Convert.ToInt32(r.Value) }).Where(x => x.K != null).ToDictionary(x => x.K, x => x.V);

                // 11. News Basic
                var newsBasic = await multi.ReadFirstAsync<dynamic>();
                response.News.Total = Convert.ToInt32(newsBasic.Total);

                // 12. News By Category
                var newsByCat = await multi.ReadAsync<dynamic>();
                response.News.ByCategory = newsByCat.Select(r => new { K = (string)r.Key, V = (int)Convert.ToInt32(r.Value) }).Where(x => x.K != null).ToDictionary(x => x.K, x => x.V);

                // 13. Ads Basic
                var adsBasic = await multi.ReadFirstAsync<dynamic>();
                response.Ads.TotalActive = Convert.ToInt32(adsBasic.TotalActive);

                // 14. Ads By Type
                var adsByType = await multi.ReadAsync<dynamic>();
                response.Ads.ByType = adsByType.Select(r => new { K = (string)r.Key, V = (int)Convert.ToInt32(r.Value) }).Where(x => x.K != null).ToDictionary(x => x.K, x => x.V);

                // 15. Faculty Basic
                var facultyBasic = await multi.ReadFirstAsync<dynamic>();
                response.Faculty.Total = Convert.ToInt32(facultyBasic.Total);

                // 16. Faculty By Title
                var facultyByTitle = await multi.ReadAsync<dynamic>();
                response.Faculty.ByTitle = facultyByTitle.Select(r => new { K = (string)r.Key, V = (int)Convert.ToInt32(r.Value) }).Where(x => x.K != null).ToDictionary(x => x.K, x => x.V);

                // 17. Permissions
                var permBasic = await multi.ReadFirstAsync<dynamic>();
                response.Permissions.TotalDefinitions = Convert.ToInt32(permBasic.TotalDefinitions);
            }

            return response;
        }
    }
}
