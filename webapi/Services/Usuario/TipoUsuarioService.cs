using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Model;
using DaccApi.Model.Responses.Usuario;
using DaccApi.Responses;
using DaccApi.Helpers;

namespace DaccApi.Services.User
{
    public class TipoUsuarioService : ITipoUsuarioService
    {
        private readonly ITipoUsuarioRepository _tipoUsuarioRepository;

        public TipoUsuarioService(ITipoUsuarioRepository tipoUsuarioRepository)
        {
            _tipoUsuarioRepository = tipoUsuarioRepository;
        }

        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _tipoUsuarioRepository.GetAllAsync();
                var response = roles.Select(r => new ResponseRole(r));
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { roles = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
