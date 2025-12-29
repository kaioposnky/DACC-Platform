using DaccApi.Model.Objects.Order;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Orders
{
    public interface ICupomService
    {
        Task<IActionResult> ValidateCupom(string code);
        Task<IActionResult> GetAllCupons();
        Task<IActionResult> CreateCupom(Cupom cupom);
        Task<IActionResult> DeleteCupom(Guid id);
    }
}
