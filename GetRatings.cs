using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class GetRatings
    {
        private readonly ILogger _logger;

        public GetRatings(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetRatings>();
        }

        [Function("GetRatings")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            

            _logger.LogInformation($"C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);

            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString($"The ratings");

            return response;
        }
    }
}
