using DaccApi.Model;
using DaccApi.Model.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Professores
{
    public interface IProfessoresService
    {
        Task<IActionResult> GetAllProfessores();
        Task<IActionResult> GetProfessorById(Guid id);
        Task<IActionResult> CreateProfessor(RequestProfessor request);
        Task<IActionResult> UpdateProfessor(Guid id, RequestProfessor request);
        Task<IActionResult> DeleteProfessor(Guid id);
        Task<IActionResult> CreateProfessorJson(RequestProfessorJson request);
        Task<IActionResult> UpdateProfessorJson(Guid id, RequestProfessorJson request);
        Task<IActionResult> SearchProfessores(RequestQueryProfessor query);
    }
}
