using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class CreateRating
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

 

        public CreateRating(HttpClient httpClient, ILoggerFactory loggerFactory)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ApiURI"));
            _logger = loggerFactory.CreateLogger<CreateRating>();
        }

        [Function("CreateRating")]
        [CosmosDBOutput("%CosmosDb%", "%CosmosCollOut%", ConnectionStringSetting = "CosmosConnection", CreateIfNotExists = true)]
        public async Task<object> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            // _logger.LogInformation("C# HTTP trigger function processed a request.");

            // var q = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            // var productId = q["productId"] ?? "0";

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            RatingItem ratingItem = (RatingItem)Newtonsoft.Json.JsonConvert.DeserializeObject(requestBody, typeof(RatingItem));

            // validate userId and productId using external APIs
            HttpResponseMessage responseProduct = await _httpClient.GetAsync($"GetProduct?productId={ratingItem.productId}");
            HttpResponseMessage responseUser = await _httpClient.GetAsync($"GetUser?userId={ratingItem.userId}");

            if (!responseProduct.IsSuccessStatusCode || !responseUser.IsSuccessStatusCode)
            {

                var invalidUserAndProductResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                invalidUserAndProductResponse.WriteString("Invalid user or product reference");
                return null;

            }

            // validate the value of rating and ensure it is between 0 and 5
            if (ratingItem.rating < 0 || ratingItem.rating > 5) {
                var invalidPayloadResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                invalidPayloadResponse.WriteString("rating must be between 0 and 5");
                return null;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            ratingItem.id = Guid.NewGuid().ToString();
            ratingItem.timestamp = DateTime.Now;

            return ratingItem;
        }
    }
}
