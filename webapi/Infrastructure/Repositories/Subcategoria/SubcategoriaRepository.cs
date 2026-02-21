using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Data.Orm.Subcategoria;

public class SubcategoriaRepository : BaseRepository<ProdutoSubcategoria>, ISubcategoriaRepository
{
    public SubcategoriaRepository(IRepositoryDapper dapper) : base(dapper)
    {
    }

    /// <inheritdoc />
    public new async Task<bool> CreateAsync(ProdutoSubcategoria produtoSubcategoria)
    {
        var query = _dapper.GetQueryNamed("CreateSubcategoria");
        var param = new
        {
            Nome = produtoSubcategoria.Nome
        };
        var result = await _dapper.ExecuteAsync(query, param);

        return result > 0;
    }
}