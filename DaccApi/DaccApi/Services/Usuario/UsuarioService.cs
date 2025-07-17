using DaccApi.Enum.UserEnum;
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
                if (string.IsNullOrWhiteSpace(request.Nome) ||
                    string.IsNullOrWhiteSpace(request.Sobrenome) ||
                    string.IsNullOrWhiteSpace(request.Email) ||
                    string.IsNullOrWhiteSpace(request.Senha) ||
                    string.IsNullOrWhiteSpace(request.Telefone)
                    )
                {
                    return ResponseHelper.CreateBadRequestResponse("Os campos Nome, Email e Senha são obrigatórios.");
                }
                
                var usuario = new Usuario
                {
                    Nome = request.Nome,
                    Sobrenome = request.Sobrenome,
                    Email = request.Email,
                    SenhaHash = request.Senha,
                    Telefone = request.Telefone,
                    // Se não for especificado, criar Visitante por padrão
                    TipoUsuario = request.TipoUsuario ??= TipoUsuarioEnum.Visitante
                };

                _usuarioRepository.CreateUser(usuario);

                return ResponseHelper.CreateSuccessResponse("", "Usuário adicionado com sucesso.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    "Ocorreu um erro ao tentar cadastrar o usuário, favor relatar ao suporte pelo: contato.daccfei@gmail.com " +
                    ex.StackTrace);
            }
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
