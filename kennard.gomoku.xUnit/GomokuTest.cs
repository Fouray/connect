
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using kennards.gomoku.api;
using kennards.generic.classes;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace kennard.gomoku.xUnit
{
    public class GomokuTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();



        [Theory]
        [InlineData("Create Standard Game with Two Unique Players", "standard", "p1", "p2", true)]
        [InlineData("Fail To Create Game with non Unique players", "standard", "p1", "p1", false)]
        [InlineData("Create a standard game with no arguments passed", "standard", null, null, true)]
        [InlineData("Fail to Create and unknown game style", "NonStandard", null, null, false)]
        [InlineData("Create Alternate game", "Alternate", null, null, true)]
        public  void Create(string description, string gameStyle, string p1, string p2, bool expectedResult)
        {
            Dictionary<string, StringValues> paramaters = new Dictionary<string, StringValues>();
            paramaters.Add("gameStyle", gameStyle);
            paramaters.Add("p1", p1);
            paramaters.Add("p2", p2);

            var request = TestFactory.CreateHttpRequest(paramaters);
            IActionResult response =  Game.Create(request, logger);
            var ok = response as OkObjectResult;
            Gomoku game = null;
            if (ok != null)
            {
                game = ok.Value as Gomoku;
            }
            Assert.True((game != null) == expectedResult, description);
        }
        public static IEnumerable<object[]> GetGameData()
        {

            //Test a successful move
            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"No Name 1\"},\"row\":1,\"column\":1}}",
                     BoardResult.moveSuccessful
                    }; 

            //Test an uppoer bound out of bounds move 
            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"No Name 1\"},\"row\":15,\"column\":15}}",
                     BoardResult.invlaidMoveOutofBounds
                    };

            //test lower bound out of bounds
            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"No Name 1\"},\"row\":12,\"column\":-1}}",
                     BoardResult.invlaidMoveOutofBounds
                    };

            //test move out of turn
            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"No Name 0\"},\"row\":12,\"column\":12}}",
                     BoardResult.invalidMoveOutOfTurn
                    };

            //test invlaid Move with already occupied intersection
            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"No Name 1\"},\"row\":0,\"column\":0}}",
                     BoardResult.invalidMoveOccupied
                    };

            //test move by invlaid player
            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"Invalid Player\"},\"row\":5,\"column\":5}}",
                     BoardResult.invalidPlayer
                    };

            //Test no move passed
            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{}}",
                     BoardResult.invalidPlayer
                    };

            //Test victory Gomoku
            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 1\"},\"row\":5,\"column\":5}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":1}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 1\"},\"row\":6,\"column\":6}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":2}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 1\"},\"row\":7,\"column\":7}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":3}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 1\"},\"row\":8,\"column\":8}" +
                    "]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"No Name 0\"},\"row\":0,\"column\":4}}",
                     BoardResult.resultVictory
                    };

            //test vistory ticktacktoe
            yield return
           new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 1\"},\"row\":0,\"column\":1}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":2}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 1\"},\"row\":1,\"column\":0}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":1,\"column\":1}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 1\"},\"row\":1,\"column\":2}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":2,\"column\":1}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 1\"},\"row\":2,\"column\":2}" +
                    "]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":3,\"pattern\":\"XXX\",\"name\":\"TickTackToe\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"No Name 0\"},\"row\":2,\"column\":0}}",
                     BoardResult.resultVictory
                   };
            //test draw ticktacktoe
            yield return
          new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 1\"},\"row\":0,\"column\":2}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":1}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 1\"},\"row\":1,\"column\":0}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":1,\"column\":1}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 1\"},\"row\":1,\"column\":2}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":2,\"column\":1}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 1\"},\"row\":2,\"column\":2}" +
                    "]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":3,\"pattern\":\"XXX\",\"name\":\"TickTackToe\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"No Name 0\"},\"row\":2,\"column\":0}}",
                     BoardResult.resultDraw
                  };
        }

        [Theory]
        [MemberData(nameof(GetGameData))]
        
        public  void Move(string game, BoardResult expectedResult)
        {
            Dictionary<string, StringValues> paramaters = new Dictionary<string, StringValues>();
            paramaters.Add("current", game);

            var request = TestFactory.CreateHttpRequest(paramaters);
            IActionResult response =   Game.Move(request, logger);
            var ok = response as OkObjectResult;
            Gomoku returnedGame = null;
            if (ok != null)
            {
                returnedGame = ok.Value as Gomoku;
                Assert.True(returnedGame.CurrentGame.State == expectedResult);
            }
            else
            {
                Assert.True(false, "Game returned unepected erorr ");
            }

        }
    }
}
