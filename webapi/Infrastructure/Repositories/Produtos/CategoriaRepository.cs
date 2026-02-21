using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Produtos
{
    /// <summary>
    /// Implementação do repositório de categorias de produtos.
    /// </summary>
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly IRepositoryDapper _dapper;

        public CategoriaRepository(IRepositoryDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<List<ProdutoCategoria>> GetAllCategoriasAsync()
        {
            var sql = _dapper.GetQueryNamed("GetAllCategorias");
            var result = await _dapper.QueryAsync<ProdutoCategoria>(sql);
            return result.ToList();
        }

        public async Task<List<ProdutoSubcategoria>> GetAllSubcategoriasAsync()
        {
            var sql = _dapper.GetQueryNamed("GetAllSubcategorias");
            var result = await _dapper.QueryAsync<ProdutoSubcategoria>(sql);
            return result.ToList();
        }

        public async Task<ProdutoCategoria?> GetCategoriaByIdAsync(Guid id)
        {
            var sql = _dapper.GetQueryNamed("GetCategoriaById");
            var result = await _dapper.QueryAsync<ProdutoCategoria>(sql, new { id });
            return result.FirstOrDefault();
        }

        public async Task<ProdutoSubcategoria?> GetSubcategoriaByIdAsync(Guid id)
        {
            var sql = _dapper.GetQueryNamed("GetSubcategoriaById");
            var result = await _dapper.QueryAsync<ProdutoSubcategoria>(sql, new { id });
            return result.FirstOrDefault();
        }
    }
}
