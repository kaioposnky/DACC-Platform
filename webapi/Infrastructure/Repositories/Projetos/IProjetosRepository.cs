using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Projetos
{
    public interface IProjetosRepository 
    {
        public Task<List<Projeto>> GetAllProjetos();
        
        public Task<Projeto?> GetProjetoById(Guid id);

        public Task CreateProjeto(Projeto projeto);

        public Task DeleteProjeto(Guid id);
        
        public Task  UpdateProjeto(Guid id, Projeto projeto);

    }
}
