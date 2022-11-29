using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class GetProduct
    {
        private readonly ILogger _logger;

        public GetProduct(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetProduct>();
        }

        [Function("GetProduct")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            var q = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            var productId = q["productId"] ?? "0";

            _logger.LogInformation($"C# HTTP trigger function processed a request.{productId}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString($"The product name for your product id {productId} is Starfruit Explosion");

            return response;
        }
    }
}
