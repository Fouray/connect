
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
    /// <summary>
    /// Testing using xUnit
    /// <remark>
    /// I had never used xUnit prior to this mainly concentrating on nUNIT.
    /// I found the Fact and Theory styles enjoyable
    /// </remark>
    /// <remark>
    /// Most things have been tested here via the API calling a with a HTTP get request.
    /// The Use of a Tick-Tack-Toe game was used in some testing to reduce the need of building up games with 225 intersections to test completed games
    /// The tick-tack-toe is identical to Gomoku except it uses a smaller board and has a smaller success pattern.
    /// </remark>
    /// </summary>
    public class GomokuTests
    {
        //Mock logger
        private readonly ILogger logger = TestFactory.CreateLogger();


        //Theory to check creation of various games and fails when expected.
        [Theory]
        [InlineData("Create Standard Game with Two Unique Players", "standard", "p1", "p2", true)]
        [InlineData("Fail To Create Game with non Unique players", "standard", "p1", "p1", false)]
        [InlineData("Create a standard game with no arguments passed", "standard", null, null, true)]
        [InlineData("Fail to Create and unknown game style", "NonStandard", null, null, false)]
        [InlineData("Create Leemoku game", "Leemoku", null, null, true)]
        [InlineData("Create Tick-Tack-Toe game", "TicTackToe", null, null, true)]
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

        /// <summary>
        /// Build up the test data for the Move tests.
        /// </summary>
        /// <returns>object[]
        ///     <list type="bullet">
        ///         <item>
        ///             <term>
        ///                 Current Game <see cref="String">Gomoku JSON</see>
        ///             </term>
        ///             <description>
        ///                 A Gomoku Object in current play as a Json String
        ///             </description>
        ///         </item> 
        ///         <item>
        ///             <term>
        ///                 Expected Result <see cref="BoardResult">BoardResult</see>
        ///             </term>
        ///             <description>
        ///                 What the expected result for the current game is
        ///             </description>
        ///         </item> 
        ///     </list>
        /// </returns>
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

            //Test an upper bound out of bounds move 
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

            //test invalid Move with already occupied intersection
            yield return
            new object[] {
                    "{\"currentGame\":{\"current\":[{\"occupiedBy\":{\"name\":\"No Name 0\"},\"row\":0,\"column\":0}]," +
                    "\"state\":3," +
                    "\"style\":{ \"size\":15,\"pattern\":\"XXXXX\",\"name\":\"Standard\"}}," +
                    "\"currentPlayers\":{ \"playerList\":[{ \"name\":\"No Name 0\"},{ \"name\":\"No Name 1\"}]}," +
                    "\"currentMove\":{ \"occupiedBy\":{ \"name\":\"No Name 1\"},\"row\":0,\"column\":0}}",
                     BoardResult.invalidMoveOccupied
                    };

            //test move by invalid player
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

            //test victory ticktacktoe
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

        /// <summary>
        /// The theory test of move utilising the data from <see cref="GetGameData">GetGameData</see>
        /// </summary>
        /// <param name="game" type="string">A json string of a current Gomoku</param>
        /// <param name="expectedResult" type="BoardResult">The expected result</param>
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
                Assert.True(false, "Game returned unexpected error ");
            }

        }
    }
}
