using DaccApi.Model;
using DaccApi.Infrastructure.Repositories.Products;
using DaccApi.Infrastructure.Repositories.Diretorias;

namespace DaccApi.Services.Diretorias
{
    public class DiretoriasService : IDiretoriasService
    {
        private readonly IDiretoriasRepository _diretoriasRepository;
        public List<Diretoria> GetDiretorias()
        {

            List<Diretoria> diretorias = _diretoriasRepository.GetDiretoriasAsync();

            return diretorias;
        }
    }
}
