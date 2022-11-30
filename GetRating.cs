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
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rating/{id}")] HttpRequestData req,
        [CosmosDBInput("%CosmosDb%", 
            "%CosmosCollOut%", 
            ConnectionStringSetting = "CosmosConnection",
            SqlQuery = "select * from %CosmosCollOut% r where r.id = {id}")] IEnumerable<RatingItem> ratingItems)
        {
            if (ratingItems == null || ratingItems.ToList().Count == 0) {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                return notFoundResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(ratingItems.FirstOrDefault());
            return response;
        }
        
    }
}
