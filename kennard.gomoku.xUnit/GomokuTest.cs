
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
        public async void AzureFunctionCreate(string description, string gameStyle, string p1, string p2, bool expectedResult)
        {
            Dictionary<string, StringValues> paramaters = new Dictionary<string, StringValues>();
            paramaters.Add("gameStyle", gameStyle);
            paramaters.Add("p1", p1);
            paramaters.Add("p2", p2);

            var request = TestFactory.CreateHttpRequest(paramaters);
            IActionResult response = await Game.Create(request, logger);
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
            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"No Name 1\"},\"row\":1,\"column\":1},\"state\":0}",
                     BoardResult.moveSuccessful
                    }; 
            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"No Name 1\"},\"row\":15,\"column\":15},\"state\":0}",
                     BoardResult.invlaidMoveOutofBounds
                    };
            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"No Name 1\"},\"row\":12,\"column\":-1},\"state\":0}",
                     BoardResult.invlaidMoveOutofBounds
                    };
            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"No Name 0\"},\"row\":12,\"column\":12},\"state\":0}",
                     BoardResult.invalidMoveOutOfTurn
                    };
            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"No Name 1\"},\"row\":0,\"column\":0},\"state\":0}",
                     BoardResult.invalidMoveOccupied
                    };
            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"Invalid Player\"},\"row\":5,\"column\":5},\"state\":0}",
                     BoardResult.invalidPlayer
                    };
            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{},\"state\":0}",
                     BoardResult.invalidNoMove
                    };

            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 1\"},\"row\":5,\"column\":5}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":1}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 1\"},\"row\":6,\"column\":6}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":2}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 1\"},\"row\":7,\"column\":7}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":3}," +
                    "{\"occupiedBy\":{\"name\":\"No Name 1\"},\"row\":8,\"column\":8}" +0
                    "]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"No Name 0\"},\"row\":0,\"column\":4},\"state\":0}",
                     BoardResult.resultVictory
                    };
        }

        [Theory]
        [MemberData(nameof(GetGameData))]
        public async void AzureFunctionMove(string game, BoardResult expectedResult)
        {
            Dictionary<string, StringValues> paramaters = new Dictionary<string, StringValues>();
            paramaters.Add("current", game);

            var request = TestFactory.CreateHttpRequest(paramaters);
            IActionResult response = await Game.Move(request, logger);
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
