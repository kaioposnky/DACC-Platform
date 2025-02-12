using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Diretorias
{
    public interface IDiretoriasService
    {
        public List<Diretoria> GetDiretorias();

    }
}
