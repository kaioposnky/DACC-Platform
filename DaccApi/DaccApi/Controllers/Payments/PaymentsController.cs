using DaccApi.Helpers;
using DaccApi.Helpers.Attributes;
using DaccApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Payments
{
    [Authorize]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        [WebhookResponses]
        [HttpGet("success")]
        [AllowAnonymous]
        public IActionResult Success([FromQuery] string external_reference)
        {
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(external_reference),
                "Pagamento realizado com sucesso!");
        }

        [WebhookResponses]
        [HttpGet("failure")]
        [AllowAnonymous]
        public IActionResult Failure([FromQuery] string external_reference)
        {
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(external_reference),
                "Pagamento falhou. Tente novamente.");
        }

        [WebhookResponses]
        [HttpGet("pending")]
        [AllowAnonymous]
        public IActionResult Pending([FromQuery] string external_reference)
        {
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(external_reference),
                "Pagamento pendente. Aguarde a confirmação.");
        }
    }
}