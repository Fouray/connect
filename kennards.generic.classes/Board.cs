using System;
using System.Collections.Generic;
using System.Text;

namespace kennards.generic.classes
{
    [Flags]
    public enum BoardResult
    {
        inital,

        resultVictory,
        resultDraw,


        moveSuccessful,

        invalidMoveOutOfTurn,
        invalidMoveOccupied,
        invlaidMoveOutofBounds,
        invalidMoveGameOver,
        invalidPlayer,
        invalidNoMove,

        Succesfull = inital | resultVictory | resultDraw | moveSuccessful,
        UnSuccessful = invalidMoveOutOfTurn | invalidMoveOccupied | invlaidMoveOutofBounds | invalidMoveGameOver | invalidPlayer | invalidNoMove,
        Over = invalidMoveGameOver | resultDraw | resultVictory

    }
    public class Intersection
    {
        public Player OccupiedBy { get; set; } = null;
        public int Row { get; set; }
        public int Column { get; set; }
        public Intersection()
        {
            OccupiedBy = new Player();
            Row = -1;
            Column = -1;
        }

        public Intersection(Player P, int R, int C)
        {
            OccupiedBy = P;
            Row = R;
            Column = C;
        }
    }

    public class Board
    {
        public List<Intersection> Current { get; set; } = new List<Intersection>();
        public BoardResult State { get; set; } = BoardResult.inital;
        public GameStyle Style { get; set; } = GameStyles.Get(null);
        public Board(GameStyle style)
        {
            Style = style;
        }
        public Board()
        {

        }

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

        public bool Move(Intersection move)
        {

            Intersection lastMove = null;
            if (Current.Count > 0)
            {
                lastMove = Current[Current.Count - 1];
            }
            if (move == null || String.IsNullOrEmpty(move.OccupiedBy.Name))
            {
                State = BoardResult.invalidNoMove;
            }
            else if (State == BoardResult.Over)
            {
                State = BoardResult.invalidMoveGameOver;
            }
            else if (move.Row >= Style.Size || move.Column >= Style.Size || move.Row < 0 || move.Column < 0)
            {
                State = BoardResult.invlaidMoveOutofBounds;
            }
            else if (Current.Exists(i => i.Row == move.Row && i.Column == move.Column))
            {
                State = BoardResult.invalidMoveOccupied;
            }
            else if (lastMove != null && lastMove.OccupiedBy.Name == move.OccupiedBy.Name)
            {
                State = BoardResult.invalidMoveOutOfTurn;
            }

            else if (State != BoardResult.Succesfull || State != BoardResult.Over)
            {

                Current.Add(move);
                FindResult();

            }
            return State == BoardResult.Succesfull;
        }

        private void FindResult()
        {
            //set successful unless win or draw
            State = BoardResult.moveSuccessful;

            //get length of pattern
            var currentPatternLength = Style.Pattern.Split("X").Length;
            var oponentPatternLength = Style.Pattern.Split(".").Length;

            Intersection lastMove = null;
            if (Current.Count > 0)
            {
                lastMove = Current[Current.Count - 1];
            }

            //check at least a move in the game.
            if (lastMove != null)
            {
                //Check play has played enough tiles to meet pattern length
                if (Current.FindAll(f => f.OccupiedBy.Name == lastMove.OccupiedBy.Name).Count < (currentPatternLength) &&
                     //If pattern includes openents tiles then ensure enough moves to includes
                     Current.Count >= currentPatternLength + oponentPatternLength)
                {
                    //only need to check the last tile has won the game 
                    if (lastMove != null)
                    {
                        //get pattern string of horizontal 
                        string testString = BuildPattern(lastMove.OccupiedBy.Name, lastMove.Column, lastMove.Row, currentPatternLength + oponentPatternLength);
                        if (testString.IndexOf(Style.Pattern) >= 0)
                        {
                            State = BoardResult.resultVictory;
                            return;
                        }
                        testString = BuildPattern(lastMove.OccupiedBy.Name, lastMove.Row, lastMove.Column, currentPatternLength + oponentPatternLength);
                        if (testString.IndexOf(Style.Pattern) >= 0)
                        {
                            State = BoardResult.resultVictory;
                            return;
                        }
                        testString = BuildDiagonalPattern(lastMove.OccupiedBy.Name, lastMove.Row, lastMove.Column, currentPatternLength + oponentPatternLength);
                        if (testString.IndexOf(Style.Pattern) >= 0)
                        {
                            State = BoardResult.resultVictory;
                            return;
                        }
                        testString = BuildDiagonalPattern(lastMove.OccupiedBy.Name, lastMove.Row - (currentPatternLength + oponentPatternLength), lastMove.Column - (currentPatternLength + oponentPatternLength), currentPatternLength + oponentPatternLength, false) ;
                        if (testString.IndexOf(Style.Pattern) >= 0)
                        {
                            State = BoardResult.resultVictory;
                            return;
                        }

                        //check if all points are full
                        if (Current.Count == Math.Pow(Style.Size, 2))
                        {
                            State = BoardResult.resultDraw;
                            return;
                        }
                    }
                }
            }
        }

        private string BuildPattern(string name, int r, int c, int length)
        {
            string result = "";
            for (var i = r - length; i <= r + length; i++)
            {
                if (i >= 0 && i < Style.Size)
                {
                    Intersection tile = Current.Find(f => f.Column == i && f.Row == c);
                    if (tile != null)
                    {
                        if (name == tile.OccupiedBy.Name)
                        {
                            result += "X";
                        }
                        else
                        {
                            result += ".";
                        }
                    }
                    else
                    {
                        result += " ";
                    }
                }
            }
            return result;
        }
        private string BuildDiagonalPattern(string name, int r, int c, int length,bool forward =true)
        {
            string result = ""; 
            
            var newR = (r - length);
            var newC = (c - length);
            if (!forward)
            {
                 newR = (r + length);
                 newC = (c + length);
            }
            for (var i2=0; i2 <= (length*2);i2++)
            {
                var i = i2;
                if (!forward)
                {
                    i = ((length * 2) - 1) - i2;
                }
                 r += i;
                 c += i;
                if (r >= 0 && r < Style.Size && c >= 0 && c < Style.Size)
                {
                    Intersection tile = Current.Find(f => f.Column == c && f.Row == r);
                    if (tile != null)
                    {
                        if (name == tile.OccupiedBy.Name)
                        {
                            result += "X";
                        }
                        else
                        {
                            result += ".";
                        }
                    }
                    else
                    {
                        result += " ";
                    }
                }
            }
            return result;
        }

    }
}
