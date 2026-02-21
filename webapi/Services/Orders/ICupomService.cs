using DaccApi.Model.Objects.Order;
using DaccApi.Model.Requests.Orders;
using DaccApi.Model.Responses.Order;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Orders
{
    public interface ICupomService
    {
        Task<IActionResult> ValidateCupom(string code);
        Task<IActionResult> GetAllCuponsAsync();
        Task<IActionResult> GetByIdAsync(Guid id);
        Task<IActionResult> CreateCupomAsync(RequestCreateCupom request);
        Task<IActionResult> UpdateCupomAsync(Guid id, RequestUpdateCupom request);
        Task<IActionResult> DeleteCupomAsync(Guid id);
    }
}
