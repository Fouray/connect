using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using kennards.generic.classes;

namespace kennards.gomoku.api
{
    public static class Game
    {
        [FunctionName("CreateGame")]
        public static IActionResult  Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "gomoku/v1/create")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"CreateGame (v1)");
           
            string style = req.Query["gameStyle"];
            string p1 = req.Query["p1"];
            string p2 = req.Query["p2"];
            try
            {
                Gomoku game = new Gomoku(style,p1,p2);


                return  new OkObjectResult(game);
            }
            catch(Exception e)
            {
                log.LogError(e.Message);
                 return new BadRequestObjectResult($"Exception Returned the following erorr {e.Message}");
            }
        }
        [FunctionName("GameMove")]
        public static IActionResult Move(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "gomoku/v1/move")] HttpRequest req,
          ILogger log)
        {
            log.LogInformation($"Move (v1)");

            string gameJson = req.Query["current"];
            try
            {
                Gomoku game = JsonConvert.DeserializeObject<Gomoku>(gameJson);

                game.Move();

                return new OkObjectResult(game);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new BadRequestObjectResult($"Exception Returned the following erorr {e.Message}");
            }
        }
    }
}
