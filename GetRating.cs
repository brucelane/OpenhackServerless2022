using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class GetRating
    {
        private readonly ILogger _logger;

        public GetRating(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetRating>();
        }

        [Function("GetRating")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            var q = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            var ratingId = q["ratingId"] ?? "0";

            _logger.LogInformation($"C# HTTP trigger function processed a request.{ratingId}");

            var response = req.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString($"The rating for id {ratingId} is 42");

            return response;
        }
    }
}
