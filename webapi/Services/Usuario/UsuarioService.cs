using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Model;
using DaccApi.Model.Responses;
using DaccApi.Responses;
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

        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _usuarioRepository.GetAllAsync();
                if (users.Count == 0)
                {
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);
                }

                var response = users.Select(user => new ResponseUsuario(user));
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { users = response }),
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
                var userData = await _usuarioRepository.GetByIdAsync(id);
                if (userData == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
                }
                
                // Para cada atributo, tenta pegar um valor, se for nulo, usa o valor já setado do usuário
                userData.Nome = newUserData.Nome ?? userData.Nome;
                userData.Sobrenome = newUserData.Sobrenome ?? userData.Sobrenome;
                userData.Curso = newUserData.Curso ?? userData.Curso;
                userData.Telefone = newUserData.Telefone ?? userData.Telefone;
                userData.InscritoNoticia = newUserData.InscritoNoticia ?? userData.InscritoNoticia;
                userData.DataAtualizacao = DateTime.UtcNow;
                
                await _usuarioRepository.UpdateAsync(id, userData);
                
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
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
                var user = await _usuarioRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, 
                        "Usuário não encontrado!");
                }

                await _usuarioRepository.DeleteAsync(id);

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
                var usuario = _usuarioRepository.GetByIdAsync(id).Result;

                if (usuario == null)
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Usuário não encontrado!");
                
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK,"Usuário obtido com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, "Erro ao obter usuário pelo Id! " + ex.Message);
            }
        }

        public IActionResult GetUserByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "O Email não pode estar vazio!");
                }

                var usuario = _usuarioRepository.GetUserByEmail(email).Result;

                if (usuario == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Usuário não encontrado!");
                }

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { user = usuario.ToResponse() }), "Usuário obtido com sucesso!");
            } catch(Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, "Erro ao obter usuário pelo Email! " + ex.Message);
            }
        }
    }
}
