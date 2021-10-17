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
    /// <summary>
    /// AzureFunction for the API of the Gomoku Game.
    /// <remark>I had not used these prior to this exercise but decided it was best way to build something and show my Azure abilities...As I said in phone call the Processes are the same it is often just a little different methods.</remark>
    /// <remark>No Security was implemented</remark>
    /// <remark>GET method was used for simplicity. I would normally use encrypted Post methods</remark>
    /// </summary>
    public static class Game
    {
        /// <summary>
        /// API call to create a new game
        /// </summary>
        /// <param name="req"> the URL Request</param>
        /// <param name="log"> The default logger</param>
        /// <returns>A game object</returns>
        /// <exception cref="Exception">Any exceptions raised are returned as a bad request</exception>
        /// 
        [FunctionName("CreateGame")]
        public static IActionResult  Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "gomoku/v1/create")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"CreateGame (v1)");

            //The following Parameters are optional but can have values to set up a game
            ///<see cref="GomokuTests">GomokuTests</see>
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

        /// <summary>
        /// Porcess a move ona exsiiting Gomoku game
        /// </summary>
        /// <param name="req">The query string must contain a JSO Gomoku Object</param>
        /// <param name="log"></param>
        /// <returns></returns>
        /// <exception cref="Exception">Any exceptions raised are returned as a bad request</exception>
        /// 
        [FunctionName("GameMove")]
        public static IActionResult Move(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "gomoku/v1/move")] HttpRequest req,
          ILogger log)
        {
            log.LogInformation($"Move (v1)");

            //a JSON string of the Gomoku Object must be passed. (If using  a data store I would pass id)
            string gameJson = req.Query["current"];
            try
            {
                //Attempt to convert and then execute the move.,
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
