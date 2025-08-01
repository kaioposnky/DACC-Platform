using DaccApi.Helpers;
using DaccApi.Infrastructure.Cryptography;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Model;
using Helpers.Response;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.User
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IArgon2Utility _argon2Utility;

        public UsuarioService(IUsuarioRepository usuarioRepository, IArgon2Utility argon2Utility)
        {
            _usuarioRepository = usuarioRepository;
            _argon2Utility = argon2Utility;
        }

        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _usuarioRepository.GetAll();
                if (users.Count == 0)
                {
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);
                }

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { users = Usuario.ToListResponse(users) }), 
                    "Usuários obtidos com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> UpdateUser(Guid id, RequestUpdateUsuario newUserData)
        {
            try
            {
                var userData = await _usuarioRepository.GetUserById(id);
                if (userData == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
                }
                
                // Para cada atributo, tenta pegar um valor, se for nulo, usa o valor já setado do usuário
                var user = new Usuario()
                {
                    Id = id,
                    Nome = newUserData.Nome ?? userData.Nome,
                    Ra = userData.Ra,
                    Sobrenome = newUserData.Sobrenome ?? userData.Sobrenome,
                    Curso = newUserData.Curso ?? userData.Curso,
                    Telefone = newUserData.Telefone ?? userData.Telefone,
                    NewsLetterSubscriber = newUserData.NewsLetterSubscriber ?? userData.NewsLetterSubscriber,
                };
                
                await _usuarioRepository.UpdateUser(user);
                
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { users = user.ToResponse() }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, 
                    "Erro ao autalizar usuário!" + ex.Message);
            }
        }

        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var user = await _usuarioRepository.GetUserById(id);
                if (user == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, 
                        "Usuário não encontrado!");
                }

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, $"Usuário {user.Nome} {user.Sobrenome} deletado com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,
                    "Erro ao deletar usuário!" + ex.Message);
            }
        }
        
        public IActionResult GetUserById(Guid id)
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
                return ResponseHelper.CreateErrorResponse("Erro ao obter usuário pelo Id! " + ex.Message);
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

                return ResponseHelper.CreateSuccessResponse(new { user = usuario.ToResponse() }, "Usuário obtido com sucesso!");
            } catch(Exception ex)
            {
                return ResponseHelper.CreateErrorResponse("Erro ao obter usuário pelo Email! " + ex.Message);
            }
            
        }
    }
}
