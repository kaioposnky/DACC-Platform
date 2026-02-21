using DaccApi.Helpers.Attributes;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Model.Requests.Orders;
using DaccApi.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Orders
{
    [Authorize]
    [ApiController]
    [Route("v1/api/coupons")]
    public class CuponsController : ControllerBase
    {
        private readonly ICupomService _cupomService;

        public CuponsController(ICupomService cupomService)
        {
            _cupomService = cupomService;
        }

        [AuthenticatedGetResponses]
        [HttpGet("")]
        [HasPermission(AppPermissions.Cupons.View)]
        public async Task<IActionResult> GetAll()
        {
            return await _cupomService.GetAllCuponsAsync();
        }

        [AuthenticatedGetResponses]
        [HttpGet("{id:guid}")]
        [HasPermission(AppPermissions.Cupons.View)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            return await _cupomService.GetByIdAsync(id);
        }

        [HasPermission(AppPermissions.Cupons.Create)]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] RequestCreateCupom request)
        {
            return await _cupomService.CreateCupomAsync(request);
        }

        [HasPermission(AppPermissions.Cupons.Update)]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RequestUpdateCupom request)
        {
            return await _cupomService.UpdateCupomAsync(id, request);
        }

        [HasPermission(AppPermissions.Cupons.Delete)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return await _cupomService.DeleteCupomAsync(id);
        }
    }
}
