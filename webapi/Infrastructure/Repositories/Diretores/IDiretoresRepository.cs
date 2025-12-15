using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Diretores
{
    public interface IDiretoresRepository
    {

        public Task<List<Diretor>> GetAllDiretores();

        public Task<Diretor?> GetDiretorById(Guid id);
        public Task CreateDiretor(Diretor diretor);

        public Task DeleteDiretor(Guid id);
        public Task UpdateDiretor(Guid id, Diretor diretor);




    }
}
