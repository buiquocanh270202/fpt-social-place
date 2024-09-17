using Domain.ApiModels;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BaseController : Controller
    {
        [NonAction]
        public IActionResult Success(object? data = null)
        {
            return Result(new ApiResponseModel(data));
        }

        [NonAction]
        public IActionResult Result(StatusCodeEnum code)
        {
            return Result(new ApiResponseModel(code));
        }
        [NonAction]
        public IActionResult Result(ApiResponseModel resp)
        {
            return Ok(resp);
        }

        [NonAction]
        public IActionResult Success<T>(T? data = default)
        {
            return Ok(new ApiResponseModel<T>(data));
        }

        [NonAction]
        public IActionResult BadRequest(bool withModelState)
        {
            IActionResult result = BadRequest();
            if (withModelState)
            {
                result = BadRequest(ModelState);
            }
            return result;
        }

        [NonAction]
        public IActionResult BadResult(ApiResponseModel message)
        {
            return BadRequest(message);
        }
    }
}
