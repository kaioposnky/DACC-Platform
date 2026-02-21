using Microsoft.AspNetCore.Mvc;
using DaccApi.Model.Requests.Curso;

namespace DaccApi.Services.User
{
    public interface ICursoService
    {
        Task<IActionResult> GetAllCursos();
        Task<IActionResult> GetCursoById(Guid id);
        Task<IActionResult> CreateCurso(RequestCreateCurso request);
        Task<IActionResult> UpdateCurso(Guid id, RequestUpdateCurso request);
        Task<IActionResult> DeleteCurso(Guid id);
    }
}
