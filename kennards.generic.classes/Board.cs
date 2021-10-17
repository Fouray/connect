using System;
using System.Collections.Generic;
using System.Text;

namespace kennards.generic.classes
{
    /// <summary>
    /// BoardResults enumerated type represents the states a board is in.
    /// Each move request is processed and returned with the current state.
    /// The front end is then required to carry out what ever is required based on the board state.
    /// </summary>
    [Flags]
    public enum BoardResult
    {
        inital, //The board is in the initial state no moves have been made. Moves can be made.

        resultVictory, //The board is in a victory state. No further moves can be made.
        resultDraw, //The board is in a drawn state no further moves can be made.


        moveSuccessful, //Last move was successful and has been added to the board. More moves can be made

        invalidMoveOutOfTurn, //The last move was invalid due to not being the users turn. Last move is not added to board.
        invalidMoveOccupied, //The last move was invalid as the intersection was already occupied. The last move was not added to the board
        invlaidMoveOutofBounds,//The last move was invalid as it was outside the bounds of the board. The last move was not added to the board
        invalidMoveGameOver,//The last move is invalid as it  was made on a completed game. The last move was not added to the board
        invalidPlayer, //The last move is invalid as it was made by a player who is not part of the player list. The last move was not made.

        Succesfull = inital | resultVictory | resultDraw | moveSuccessful, //The group of successful moves
        UnSuccessful = invalidMoveOutOfTurn | invalidMoveOccupied | invlaidMoveOutofBounds | invalidMoveGameOver | invalidPlayer ,//The group of unsuccessful moves
        Over = invalidMoveGameOver | resultDraw | resultVictory //the group of game over states

    }

    /// <summary>
    /// Board - The game board. 
    ///     <list type="bullet">
    ///         <item>
    ///             <term>Current</term>
    ///             <description>
    ///                 List of <see cref="Intersection">Inersections</see>. Each intersection represents a playable location on a board.
    ///                 The board stores all played Intersectons.
    ///                 <example>
    ///                     current=[] 
    ///                     <remark>is an game that has just started and no players have made a move</remark>
    ///                 </example>
    ///                 <example>
    ///                     current=[{"occupiedBy":{"name":"No Name 0"},"row":0,"column":0}] 
    ///                     <remark>is an game that has had one move made by Player "No Name 0" at board position (0, 0)</remark> 
    ///                 </example>
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>State</term>
    ///             <description>
    ///                 A <see cref="BoardResult">BoardResult</see>. The current state of the game using the enumerated type BoardResult.    ///                 
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>Style</term>
    ///             <description>
    ///                 A <see cref="GameStyle">GameStyle</see>. The style of game being played.    ///                
    ///             </description>
    ///         </item>
    ///     </list>
    /// </summary>
    /// 

    public class Board
    {
        public List<Intersection> Current { get; set; } = new List<Intersection>();
        public BoardResult State { get; set; } = BoardResult.inital;
        public GameStyle Style { get; set; } 
        public Board(GameStyle style)
        {
            Style = style;
        }
        public Board()
        {

        }
        /// <summary>ChangeStyle Method. To change the style of an un-played game.</summary>
        /// <param name="style" type="GameStyle">The style to change to</param>
        /// <returns type="bool">True if game style changed. False if not.</returns>
        public bool ChangeStyle(GameStyle style)
        {
            if (Current.Count == 0)
            {
                Style = style;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>Handles a new Move.</summary>
        /// <param name="move" type="Intersection">A new Move</param>
        /// <remarks>The state is updated with the result of the move.</remarks>
        /// <remarks>The board has the move added if the move was successful.</remarks>
        /// <returns type="bool">If move successful a true is returned otherwise false</returns>
        public bool Move(Intersection move)
        {

            Intersection lastMove = null;//Gets the last move made in the game to check order of moves.
            if (Current.Count > 0) //Has moves
            {
                //get the last move (If the adding of a list value is found to be not in order then need to add an index)
                lastMove = Current[Current.Count - 1];
            }
            //If game is over then no move can be made
            if (State == BoardResult.Over) 
            {
                State = BoardResult.invalidMoveGameOver;
            }
            //If current move is out side of bounds
            else if (move.Row >= Style.Size || move.Column >= Style.Size || move.Row < 0 || move.Column < 0)
            {
                State = BoardResult.invlaidMoveOutofBounds;
            }
            //Check current move is not already occupied
            else if (Current.Exists(i => i.Row == move.Row && i.Column == move.Column))
            {
                State = BoardResult.invalidMoveOccupied;
            }
            //Check the order of the move and that the previous mover is not the same as the current move
            //***It is assumed that only two players if a GameStyle is added with more than 2 then refactoring is required***
            else if (lastMove != null && lastMove.OccupiedBy.Name == move.OccupiedBy.Name)
            {
                State = BoardResult.invalidMoveOutOfTurn;
            }
            //if the current state of the game is successful or not over then can add the move and check for victory.
            else if (State != BoardResult.Succesfull || State != BoardResult.Over)
            {

                Current.Add(move);

                FindResult();

            }
            //Return true if the current state is successful.
            return State == BoardResult.Succesfull;
        }
        /// <summary>Checks the board for a result</summary>
        /// <remarks>The state is updated with the result of the move.</remarks>
        private void FindResult()
        {
            //set successful unless win or draw
            State = BoardResult.moveSuccessful;

            //get length of pattern - As the uses multiple versions patterns can't be assumed to be only 4 long.
            //A pattern has 'X' where the player has to have a piece and a '.' for the opponent
            // XXX means three in a row for the player
            // X.X.X. means the winning position is three separated by the opponents three
            var currentPatternLength = Style.Pattern.Split("X").Length-1;
            var oponentPatternLength = Style.Pattern.Split(".").Length-1;

            //get the last move
            Intersection lastMove = null;
            if (Current.Count > 0)
            {
                lastMove = Current[Current.Count - 1];
            }

            //check at least a move in the game. If only one move no need to check for a win
            if (lastMove != null)
            {
                //Check players has played enough tiles to meet pattern length. If not no need to check win
                if (Current.FindAll(f => f.OccupiedBy.Name == lastMove.OccupiedBy.Name).Count >= (currentPatternLength) &&
                     //If pattern includes opponents tiles then ensure enough moves to includes
                     Current.Count >= currentPatternLength + oponentPatternLength)
                {
                    //only need to check the last tile has won the game 
                    if (lastMove != null)
                    {
                        //check if a winning  board has been reached
                        State = CheckWin(lastMove);
                        
                        //if board is full then a draw if not already a victory.
                        if (State != BoardResult.resultVictory && Current.Count == Math.Pow(Style.Size, 2))
                        {
                            State = BoardResult.resultDraw;
                            return;
                        }
                    }
                }
            }
        }
        /// <summary>Checks a board for a win</summary>
        /// <param name="lastMoce" type="Intersection">Last Move made</param>
        /// <remarks>We only have to check for a win based on the last position</remarks>
        /// <returns type="BoardResult">If win the state is returned</returns>
        private BoardResult CheckWin(Intersection lastMove)
        {
            //set the default prior to checking as it is more than likely not a winning move so set to successful move.
            BoardResult result = BoardResult.moveSuccessful;

            //The algorithm uses string matching to find the patter.
            //there are four axises to check, Horizontal, Vertical, 2 Diagonal
            //Create empty strings of the length of the length of a row or column
            //****the checking could be reduce to just checking the length of the pattern and should be re factored if this was to go beyond a simple Test of coding***
            string horizontal = new string(' ', Style.Size);
            string vertical = new string(' ', Style.Size);
            string northWest = new string(' ', Style.Size);
            string northEast = new string(' ', Style.Size);

            //Loop for the length of a column and row
            //***It is assumed all boards a square***
            //***could be reduced to only loop for twice the size of the pattern and this would be implemented if this was to be utilised for real***
            for (var i = 0; i <= Style.Size; i++)
            {
                ///<remarks>
                ///In all four cases build up a  string based on the current occupied row column.
                ///If the occupied square is the current player then Replace space with 'X'
                ///if current square is the opponent then replace space with '.'
                ///if neither leave as is
                ///<example>
                /// 'X. ' play in first position opponent in second and neither in third.
                /// </example>
                /// </remarks>

                    //build up horizontal string of X and . for the  row dependent on the player
                    Intersection row = Current.Find(f => f.Column == i && f.Row == lastMove.Row);
                    if (row != null)
                    {
                        if (lastMove.OccupiedBy.Name == row.OccupiedBy.Name)
                        {
                            horizontal=horizontal.Remove(i, 1).Insert(i, "X");
                        }
                        else
                        {
                            horizontal=horizontal.Remove(i, 1).Insert(i, "."); ;
                        }
                    }
                    //build up vertical string of X and . for the  row dependent on the player
                    Intersection col = Current.Find(f => f.Column == lastMove.Column && f.Row == i);
                    if (col != null)
                    {
                        if (lastMove.OccupiedBy.Name == col.OccupiedBy.Name)
                        {
                            vertical=vertical.Remove(i, 1).Insert(i, "X");
                        }
                        else
                        {
                            vertical.Remove(i, 1).Insert(i, "."); ;
                        }
                    }

                    //Find  the north east pointing diagonal
                    int diffToFirst = Math.Min(lastMove.Row, lastMove.Column);
                    var startRow = lastMove.Row - diffToFirst;
                    var startCol = lastMove.Column - diffToFirst;
                    Intersection ne = Current.Find(f => f.Column == startCol+i && f.Row == startRow-i);
                    if (ne != null)
                    {
                        if (lastMove.OccupiedBy.Name == ne.OccupiedBy.Name)
                        {
                            northEast=northEast.Remove(i, 1).Insert(i, "X");
                        }
                        else
                        {
                            northEast=northEast.Remove(i, 1).Insert(i, "."); ;
                        }
                    } 
                    
                    //Find  the north west pointing diagonal

                     diffToFirst = Math.Min(lastMove.Row, lastMove.Column);
                    if (diffToFirst < Style.Size) {//at far end no diagonal
                        startRow = Math.Min(Style.Size-1,lastMove.Row + diffToFirst);
                        startCol = lastMove.Column - diffToFirst;
                        Intersection nw = Current.Find(f => f.Column == startCol - i && f.Row == startRow +i);
                        if (nw != null)
                        {
                            if (lastMove.OccupiedBy.Name == nw.OccupiedBy.Name)
                            {
                                northWest=northWest.Remove(i, 1).Insert(i, "X");
                            }
                            else
                            {
                                northWest= northWest.Remove(i, 1).Insert(i, "."); ;
                            }
                        }
                    }

                    //Finally simple string comparison to see if winning position found
                    if(horizontal.IndexOf(Style.Pattern)>=0 ||
                        vertical.IndexOf(Style.Pattern) >= 0 ||
                        northEast.IndexOf(Style.Pattern) >= 0 || 
                        northWest.IndexOf(Style.Pattern) >=0)
                    {
                    //found winning position
                        return BoardResult.resultVictory;
                    }
                
            }
            //return as is.
            return result;
        }

    }
}
