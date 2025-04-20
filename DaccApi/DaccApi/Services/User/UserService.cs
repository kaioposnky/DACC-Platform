using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Model;
using DaccApi.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UserService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult CreateUser(RequestUsuario request)
        {
            try
            {
                if (request == null)
                    return ResponseHelper.CreateBadRequestResponse("Requisição inválida. O corpo da solicitação não pode ser nulo.");

                if (string.IsNullOrWhiteSpace(request.Name) ||
                    string.IsNullOrWhiteSpace(request.Email) ||
                    string.IsNullOrWhiteSpace(request.Password))
                {
                    return ResponseHelper.CreateBadRequestResponse("Os campos Nome, Email e Senha são obrigatórios.");
                }

                var usuario = new Usuario
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = request.Password,
                    RegistrationDate = request.RegistrationDate
                    
                };

                _usuarioRepository.Add(request);

                return ResponseHelper.CreateSuccessResponse("", "Usuário adicionado com sucesso.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse("Ocorreu um erro ao tentar cadastrar o usuário, favor relatar ao suporte pelo: contato.daccfei@gmail.com ");
            }
        }

        public IActionResult GetUserById(RequestUsuario request)
        {
            try
            {
                if (request.UserId == null)
                {
                    return ResponseHelper.CreateBadRequestResponse("Requisição inválida. O UserId não pode ser nulo!");
                }

                var usuario = _usuarioRepository.GetUserById(request.UserId).Result;

                if (usuario == null)
                {
                    return ResponseHelper.CreateBadRequestResponse("Usuário não encontrado!");
                }

                return ResponseHelper.CreateSuccessResponse(new { user = usuario }, "Usuário obtido com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse("Erro ao obter usuário pelo Id! " + ex);
            }
        }

        public IActionResult GetUserByEmail(RequestUsuario request)
        {
            try
            {
                if (request.Email == null)
                {
                    return ResponseHelper.CreateBadRequestResponse("Requisição inválida. O Email não pode ser nulo!");
                }

                var usuario = _usuarioRepository.GetUserByEmail(request.Email).Result;

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
