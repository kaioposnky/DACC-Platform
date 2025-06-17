using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Model;
using DaccApi.Services.Auth;
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

        public IActionResult CreateUser(RequestUsuario request)
        {
            try
            {
                if (request == null)
                    return ResponseHelper.CreateBadRequestResponse("Requisição inválida. O corpo da solicitação não pode ser nulo.");

                if (string.IsNullOrWhiteSpace(request.Nome) ||
                    string.IsNullOrWhiteSpace(request.Email) ||
                    string.IsNullOrWhiteSpace(request.Senha))
                {
                    return ResponseHelper.CreateBadRequestResponse("Os campos Nome, Email e Senha são obrigatórios.");
                }

                var usuario = new Model.Usuario
                {
                    Nome = request.Nome,
                    Email = request.Email,
                    Senha = request.Senha,
                };

                _usuarioRepository.CreateUser(request);

                return ResponseHelper.CreateSuccessResponse("", "Usuário adicionado com sucesso.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse("Ocorreu um erro ao tentar cadastrar o usuário, favor relatar ao suporte pelo: contato.daccfei@gmail.com ");
            }
        }

        public IActionResult GetUserById(Guid? id)
        {
            try
            {
                if (id == null ||Guid.Empty == id)
                {
                    return ResponseHelper.CreateBadRequestResponse("Requisição inválida. O UserId não pode ser nulo!");
                }

                var usuario = _usuarioRepository.GetUserById(id);

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

                var usuario = _usuarioRepository.GetUserByEmail(email);

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
