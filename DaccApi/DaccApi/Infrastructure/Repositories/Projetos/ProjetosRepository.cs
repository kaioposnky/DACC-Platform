using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Projetos
{
    public class ProjetosRepository : IProjetosRepository
    {
        private readonly RepositoryDapper _repositoryDapper;
        public List<Projeto> GetProjetosAsync()
        {
            var sql = _repositoryDapper.GetQueryNamed("GetProjetos");
            var param = new { };

            return new List<Projeto>();
            //return Task.Run(() => {
                
            //    var projetos = _repositoryDapper.QueryAsync<Projeto>(sql, param).GetAwaiter().GetResult();

            //    var projetosList = new List<Projeto>();
            //    foreach(var projeto in projetos)
            //    {
            //        projetosList.Add(projeto);
            //    }

            //    return projetosList;
            //}).GetAwaiter().GetResult();
        }

    }
}
