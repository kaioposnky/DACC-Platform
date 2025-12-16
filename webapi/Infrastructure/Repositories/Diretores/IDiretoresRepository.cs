using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Diretores
{
    /// <summary>
    /// Define a interface para o repositório de diretores.
    /// </summary>
    public interface IDiretoresRepository
    {

        /// <summary>
        /// Obtém todos os diretores.
        /// </summary>
        public Task<List<Diretor>> GetAllDiretores();

        /// <summary>
        /// Obtém um diretor específico pelo seu ID.
        /// </summary>
        public Task<Diretor?> GetDiretorById(Guid id);
        /// <summary>
        /// Cria um novo diretor.
        /// </summary>
        public Task CreateDiretor(Diretor diretor);

        /// <summary>
        /// Deleta um diretor existente.
        /// </summary>
        public Task DeleteDiretor(Guid id);
        /// <summary>
        /// Atualiza um diretor existente.
        /// </summary>
        public Task UpdateDiretor(Guid id, Diretor diretor);




    }
}
