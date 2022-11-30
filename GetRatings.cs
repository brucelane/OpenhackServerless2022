using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

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
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ratingsByUser/{userId}")] HttpRequestData req, 
            [CosmosDBInput(databaseName: "%CosmosDb%",
                       collectionName: "%CosmosCollOut%",
                       ConnectionStringSetting = "CosmosConnection",
                       SqlQuery ="select * from %CosmosCollOut% r where r.userId = {userId}")] IEnumerable<RatingItem> ratingItems
        )
        {
            
            _logger.LogInformation($"C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);

            response.WriteAsJsonAsync(ratingItems);

            return response;
        }
    }
}
