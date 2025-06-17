using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Noticias;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Noticias;

public class NoticiasServices : INoticiasServices
{
    private readonly INoticiasRepository _noticiasRepository;

    public NoticiasServices(INoticiasRepository noticiasRepository)
    {
        _noticiasRepository = noticiasRepository;
    }

    public IActionResult GetAllNoticias()
    {
        try
        {
            var noticias = _noticiasRepository.GetAllNoticias().Result;

            if (noticias.Count == 0) return ResponseHelper.CreateBadRequestResponse();

            return ResponseHelper.CreateSuccessResponse( new { Noticias = noticias }, null);

        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse();
        }
        
    }
    
}