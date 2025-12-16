using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Projetos
{
    /// <summary>
    /// Define a interface para o repositório de projetos.
    /// </summary>
    public interface IProjetosRepository 
    {
        /// <summary>
        /// Obtém todos os projetos.
        /// </summary>
        public Task<List<Projeto>> GetAllProjetos();
        
        /// <summary>
        /// Obtém um projeto específico pelo seu ID.
        /// </summary>
        public Task<Projeto?> GetProjetoById(Guid id);

        /// <summary>
        /// Cria um novo projeto.
        /// </summary>
        public Task CreateProjeto(Projeto projeto);

        /// <summary>
        /// Deleta um projeto existente.
        /// </summary>
        public Task DeleteProjeto(Guid id);
        
        /// <summary>
        /// Atualiza um projeto existente.
        /// </summary>
        public Task  UpdateProjeto(Guid id, Projeto projeto);

    }
}
