 using DaccApi.Helpers;
using DaccApi.Helpers.Attributes;
using DaccApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Payments
{
    /// <summary>
    /// Controlador para gerenciar os retornos de pagamento.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        /// <summary>
        /// Endpoint de retorno para pagamentos bem-sucedidos.
        /// </summary>
        [WebhookResponses]
        [HttpGet("success")]
        [AllowAnonymous]
        public IActionResult Success([FromQuery] string external_reference)
        {
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(external_reference),
                "Pagamento realizado com sucesso!");
        }

        /// <summary>
        /// Endpoint de retorno para pagamentos que falharam.
        /// </summary>
        [WebhookResponses]
        [HttpGet("failure")]
        [AllowAnonymous]
        public IActionResult Failure([FromQuery] string external_reference)
        {
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(external_reference),
                "Pagamento falhou. Tente novamente.");
        }

        /// <summary>
        /// Endpoint de retorno para pagamentos pendentes.
        /// </summary>
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