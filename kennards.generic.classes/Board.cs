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

        Succesfull = inital | resultVictory | resultDraw | moveSuccessful,
        UnSuccessful = invalidMoveOutOfTurn | invalidMoveOccupied | invlaidMoveOutofBounds | invalidMoveGameOver | invalidPlayer ,
        Over = invalidMoveGameOver | resultDraw | resultVictory

    }
    

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
            if (State == BoardResult.Over)
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
            var currentPatternLength = Style.Pattern.Split("X").Length-1;
            var oponentPatternLength = Style.Pattern.Split(".").Length-1;

            Intersection lastMove = null;
            if (Current.Count > 0)
            {
                lastMove = Current[Current.Count - 1];
            }

            //check at least a move in the game.
            if (lastMove != null)
            {
                //Check play has played enough tiles to meet pattern length
                if (Current.FindAll(f => f.OccupiedBy.Name == lastMove.OccupiedBy.Name).Count >= (currentPatternLength) &&
                     //If pattern includes openents tiles then ensure enough moves to includes
                     Current.Count >= currentPatternLength + oponentPatternLength)
                {
                    //only need to check the last tile has won the game 
                    if (lastMove != null)
                    {
                        State = CheckWin(lastMove);
                        
                        if (State != BoardResult.resultVictory && Current.Count == Math.Pow(Style.Size, 2))
                        {
                            State = BoardResult.resultDraw;
                            return;
                        }
                    }
                }
            }
        }

        private BoardResult CheckWin(Intersection lastMove)
        {
            BoardResult result = BoardResult.moveSuccessful;
            string horizontal = new string(' ', Style.Size);
            string vertical = new string(' ', Style.Size);
            string northWest = new string(' ', Style.Size);
            string northEast = new string(' ', Style.Size);
            for (var i = 0; i <= Style.Size; i++)
            {
                if (i >= 0 && i < Style.Size)
                {
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
                    //build up horizontal string of X and . for the  row dependent on the player
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
                    if(horizontal.IndexOf(Style.Pattern)>=0 ||
                        vertical.IndexOf(Style.Pattern) >= 0 ||
                        northEast.IndexOf(Style.Pattern) >= 0 || 
                        northWest.IndexOf(Style.Pattern) >=0)
                    {
                        return BoardResult.resultVictory;
                    }
                }
            }
            return result;
        }

    }
}
