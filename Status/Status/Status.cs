using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace Status
{
    public static class GetStatus
    {
        [FunctionName("GetStatus")]
        public static async Task<HttpResponseMessage> Run(
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

            Dictionary<string, string> resp = new Dictionary<string, string>();
            resp.Add("status", ratings[rInt]);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(resp), Encoding.UTF8, "application/json")
            }; 
        }
    }
}
