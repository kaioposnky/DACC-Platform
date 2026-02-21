using DaccApi.Data.Orm;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model.Objects.Order;

namespace DaccApi.Infrastructure.Repositories.Orders
{
    public class CupomRepository : BaseRepository<Cupom>, ICupomRepository
    {
        public CupomRepository(IRepositoryDapper dapper) : base(dapper)
        {
        }

        public async Task<Cupom?> GetByCodeAsync(string code)
        {
            var sql = _dapper.GetQueryNamed("GetCupomByCodigo");
            var param = new { Codigo = code };
            var result = await _dapper.QueryAsync<Cupom>(sql, param);
            return result.FirstOrDefault();
        }

        public async Task IncrementUsageAsync(Guid id)
        {
            var sql = _dapper.GetQueryNamed("IncrementarUsoCupom");
            var param = new { Id = id };
            await _dapper.ExecuteAsync(sql, param);
        }
    }
}
