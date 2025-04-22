using DaccApi.Responses;
using DaccApi.Responses.UserResponse;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Helpers
{
    public static class ResponseHelper
    {
        public static IActionResult CreateSuccessResponse(object data, string message)
        {
            var response = new UserResponseRequest(message, data);
            return new ObjectResult(response) { StatusCode = 200 };
        }

        public static IActionResult CreateBadRequestResponse(string error)
        {
            var response = new BadRequest(error);
            return new ObjectResult(response) { StatusCode = 400 };
        }

        public static IActionResult CreateErrorResponse(string error)
        {
            var response = new BadRequest(error);
            return new ObjectResult(response) { StatusCode = 500 };
        }
    }
}
