using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Status
{
    public static class GetStatus
    {
        [FunctionName("GetStatus")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Getting status");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            log.LogInformation("Determining " + name + " status");

            // Fictional scale of ratings indicating excellent risk down to poor
            string[] ratings = { "AAA", "AA", "A", "BBB", "BB", "B", "CCC", "CC", "C", "D" };

            Random r = new Random();
            int rInt = r.Next(ratings.Length);

            return name != null
                ? (ActionResult)new OkObjectResult(ratings[rInt])
                : new BadRequestObjectResult("Please pass a name to rate on the query string or in the request body");
        }
    }
}
