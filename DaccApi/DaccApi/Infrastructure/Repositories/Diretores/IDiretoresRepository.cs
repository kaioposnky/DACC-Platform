using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Diretores
{
    public interface IDiretoresRepository
    {

        public Task<List<Diretor>> GetAllDiretores();

        public Task <Diretor?> GetDiretorById(int id);
        public Task CreateDiretor(RequestDiretor diretor);

        public Task DeleteDiretor(int id);
        public Task UpdateDiretor(int id, RequestDiretor diretor);




    }
}
