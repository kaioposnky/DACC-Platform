using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Diretorias
{
    public interface IDiretoriasRepository
    {

        public Task<List<Diretoria>> GetAllDiretoriasAsync();

    }
}
