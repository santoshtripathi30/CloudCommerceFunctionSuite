using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CloudCommerceFunctionSuite.Entities;
using CloudCommerceFunctionSuite.Utilities;

namespace CloudCommerceFunctionSuite.Functions
{
    public static class HttpOrderProcessingFunction
    {
        [FunctionName("OrderProcessing")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            if (req.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                log.LogInformation("Returning API documentation for GET request.");
                return new OkObjectResult(new
                {
                    Message = "Send a POST request with the following JSON format:",
                    SampleRequestBody = OrderHelper.GetExampleOrder()
                })
                {
                    ContentTypes = { "application/json" }
                };
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(requestBody))
            {
                return new BadRequestObjectResult(new
                {
                    Error = "Request body cannot be empty.",
                    ExampleRequest = OrderHelper.GetExampleOrder()
                });
            }

            Order order;
            try
            {
                order = JsonConvert.DeserializeObject<Order>(requestBody, JsonHelper.Settings);
            }
            catch (JsonException ex)
            {
                log.LogError($"JSON Deserialization Error: {ex.Message}");
                return new BadRequestObjectResult(new
                {
                    Error = "Invalid JSON format. Ensure correct key names and data types.",
                    ExampleRequest = OrderHelper.GetExampleOrder()
                });
            }

            // Validate required fields
            if (order == null || order.OrderId <= 0 || string.IsNullOrEmpty(order.CustomerName))
            {
                return new BadRequestObjectResult(new
                {
                    Error = "Invalid order details. Must provide 'OrderId', 'CustomerName', 'TotalAmount', and 'Product'.",
                    ExampleRequest = OrderHelper.GetExampleOrder()
                });
            }

            log.LogInformation($"Order Received - ID: {order.OrderId}, Customer: {order.CustomerName}, Amount: {order.TotalAmount:C}");

            return new OkObjectResult(new
            {
                Message = "Order processed successfully!",
                OrderId = order.OrderId
            });
        }

    }
}
