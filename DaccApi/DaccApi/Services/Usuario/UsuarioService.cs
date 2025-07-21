using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.User;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.User
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult GetUserById(int id)
        {
            try
            {
                var usuario = _usuarioRepository.GetUserById(id).Result;

                if (usuario == null)
                    return ResponseHelper.CreateBadRequestResponse("Usuário não encontrado!");
                
                return ResponseHelper.CreateSuccessResponse(new { user = usuario }, "Usuário obtido com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse("Erro ao obter usuário pelo Id! " + ex);
            }
        }

        public IActionResult GetUserByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return ResponseHelper.CreateBadRequestResponse("Requisição inválida. O Email não pode ser nulo!");
                }

                var usuario = _usuarioRepository.GetUserByEmail(email).Result;

                if (usuario == null)
                {
                    return ResponseHelper.CreateBadRequestResponse("Usuário não encontrado!");
                }

                return ResponseHelper.CreateSuccessResponse(new { user = usuario }, "Usuário obtido com sucesso!");
            } catch(Exception ex)
            {
                return ResponseHelper.CreateErrorResponse("Erro ao obter usuário pelo Email! " + ex);
            }
            
        }
    }
}
