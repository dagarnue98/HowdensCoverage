using Microsoft.AspNetCore.Mvc;
using PublishSystem.Domain.SeedWork;

namespace PublishSystem.Domain.Extensions
{
    public static class LayerResponseExtensions
    {
        public static IActionResult GetHttpResponse<T>(this LayerResponse<T> response)
        {
            if (response.HasError())
                return new BadRequestObjectResult(response.ErrorList);
            if (response.Content != null)
                return new OkObjectResult(response.Content);
            return new OkResult();
        }
    }
}
