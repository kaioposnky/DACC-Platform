using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;

namespace DaccApi.Controllers.Carrinho
{
    [Authorize]
    [NonController]
    [ApiController]
    [Route("api/cart")]
    public class CarrinhoController : ControllerBase
    {
        [HttpGet("")]
        [HasPermission(AppPermissions.Cart.View)]
        public IActionResult GetUserCarrinho()
        {
            throw new NotImplementedException();
        }

        [HttpPost("items")]
        [HasPermission(AppPermissions.Cart.AddItem)]
        public IActionResult AddItemToCarrinho([FromBody] object item)
        {
            throw new NotImplementedException();
        }
        
        [HttpPut("items/{id:int}")]
        [HasPermission(AppPermissions.Cart.UpdateItem)]
        public IActionResult UpdateItemFromCarrinho([FromRoute] int id, [FromBody] object item)
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete("")]
        [HasPermission(AppPermissions.Cart.Clear)]
        public IActionResult ClearCarrinho()
        {
            throw new NotImplementedException();
        }
    }
}