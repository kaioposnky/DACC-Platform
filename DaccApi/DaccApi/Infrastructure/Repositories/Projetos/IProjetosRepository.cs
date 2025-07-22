using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Projetos
{
    public interface IProjetosRepository 
    {
        public Task<List<Projeto>> GetAllProjetos();
        
        public Task<Projeto?> GetProjetoById(int id);

        public Task CreateProjeto(RequestProjeto projeto);

        public Task DeleteProjeto(int id);
        
        public Task  UpdateProjeto(int id, RequestProjeto projeto);

    }
}
