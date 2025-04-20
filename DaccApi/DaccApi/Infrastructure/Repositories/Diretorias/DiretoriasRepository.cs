using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Diretorias
{
    public class DiretoriasRepository : IDiretoriasRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;

        public List<Diretoria> GetDiretoriasAsync()
        {
            var sql = _repositoryDapper.GetQueryNamed("GetAllDiretorias");
            var param = new { };

            return Task.Run(() =>
            {
                var diretorias = _repositoryDapper.QueryAsync<Diretoria>(sql, param).GetAwaiter().GetResult();

                var diretoriasList = new List<Diretoria>();
                foreach (var diretoria in diretorias)
                {
                    diretoriasList.Add(diretoria);
                }

                return diretoriasList;
            }).GetAwaiter().GetResult();
        }

    }
}
