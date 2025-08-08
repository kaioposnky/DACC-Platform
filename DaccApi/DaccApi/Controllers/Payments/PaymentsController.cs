using DaccApi.Helpers;
using DaccApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Payments
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        [HttpGet("success")]
        [AllowAnonymous]
        public IActionResult Success([FromQuery] string external_reference)
        {
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(external_reference),
                "Pagamento realizado com sucesso!");
        }

        [HttpGet("failure")]
        [AllowAnonymous]
        public IActionResult Failure([FromQuery] string external_reference)
        {
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(external_reference),
                "Pagamento falhou. Tente novamente.");
        }

        [HttpGet("pending")]
        [AllowAnonymous]
        public IActionResult Pending([FromQuery] string external_reference)
        {
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(external_reference),
                "Pagamento pendente. Aguarde a confirmação.");
        }
    }
}