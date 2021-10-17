using System;
using System.Collections.Generic;

namespace kennards.generic.classes
{
    /// <summary>
    /// Gomoku is the main object of the game.
    ///     <list type="bullet">
    ///         <item>
    ///             <term>CurrentGame</term>
    ///             <description>
    ///             The current <see cref="Board">Board</see> of the game
    ///             </description>
    ///         </item>
    ///         
    ///         <item>
    ///             <term>CurrentPlayers</term>
    ///             <description>
    ///             The current <see cref="Players">Players</see> of the game
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>CurrentMove</term>
    ///             <description>
    ///             The current <see cref="Intersection">Move</see> of the game
    ///             </description>
    ///         </item>
    ///     </list>
    /// </summary>
    [Serializable]

    public class Gomoku
    {

        public Board CurrentGame { get; set; }
        public Players CurrentPlayers { get; set; } = new Players();

        public Intersection CurrentMove { get; set; }

        //***Due to not using a data store the entire game is passed back and forward from the client.***
        // this is done via JSON
        // On calling a game the JSON needs to be converted to the Gomoku Object before proceeding

        /// <summary>
        /// Constructor for a default game
        /// </summary>
        public Gomoku()
        {
            CurrentGame = new Board();
            CurrentPlayers = new Players();
            CurrentMove = new Intersection();
        }

        /// <summary>
        /// Constructor for a default game
        ///     <param name="game" type="GameStyle">A GameStyle object</param>
        ///     <param name="p1" type="Player">First player</param>
        ///     <param name="p2" type="Player">Second player</param>
        ///     <param name="move" type="Intersection">the current move</param>
        /// </summary>
        public Gomoku(Board game, Player p1, Player p2, Intersection move)
        {
            CurrentGame = game;
            CurrentPlayers.Add(p1);
            CurrentPlayers.Add(p2);
            CurrentMove = move;
        }

        /// <summary>
        /// Constructor for a default game
        ///     <param name="game" type="string" optional>The unique identifier for a game</param>
        ///     <param name="p1" type="string" optional>Name of fist player</param>
        ///     <param name="p2" type="string"optional>name of second player</param>
        /// </summary>
        public Gomoku(string style,string p1, string p2)
        {
            
                CurrentGame = new Board(GameStyles.Get(style));
                CurrentPlayers.Add(p1);
                CurrentPlayers.Add(p2);
                CurrentMove = new Intersection();
        }
        /// <summary>
        /// Perform a move on the game
        /// <remark>Checks the Move is valid and returns invalid player if not</remark>
        /// <remark>Calls <see cref="Board.Move(Intersection)">Board Move </see></remark>
        /// </summary>
        public void Move()
        {
          if (CurrentMove.OccupiedBy != null)
            {
                if (CurrentPlayers.PlayerList.Find(f => f.Name == CurrentMove.OccupiedBy.Name)==null)
                {
                   CurrentGame.State= BoardResult.invalidPlayer;
                }
                else if (CurrentGame.Move(CurrentMove))
                {
                    CurrentMove = new Intersection();
                }
            }
            else
            {
                CurrentGame.State = BoardResult.invalidPlayer;
            }
        }


    }
}
