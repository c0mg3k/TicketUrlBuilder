using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Common.RequestHelpers;

namespace TicketUrlBuilder
{
    public static class GetTicketUrl
    {
        [FunctionName("GetTicketUrl")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, FunctionRequest.Get, FunctionRequest.Post, Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            req.ContentType = $"application/x-www-form-urlencoded";
            string text = req.Query["text"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            text = text ?? data?.text;

            return text != null
                ? (ActionResult)new OkObjectResult(new { response_type = "in_channel", text = $"https://itt.atlas-fin.com/ticket/detail/{text}" })
                : new BadRequestObjectResult(new { response_type = "in_channel", text = "Whoops - something seems to have gone wrong.  Hang tight while we work on that for you..." });
        }
    }
}
