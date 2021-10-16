using System;
using System.Collections.Generic;

namespace kennards.generic.classes
{
   
    [Serializable]
    public class Gomoku
    {

        public Board CurrentGame { get; set; }
        public Players CurrentPlayers { get; set; } = new Players();

        public Intersection CurrentMove { get; set; }


        public Gomoku()
        {
            CurrentGame = new Board();
            CurrentPlayers = new Players();
            CurrentMove = new Intersection();
        }
        public Gomoku(Board game, Player p1, Player p2, Intersection move, BoardResult state=BoardResult.inital)
        {
            CurrentGame = game;
            CurrentPlayers.Add(p1);
            CurrentPlayers.Add(p2);
            CurrentMove = move;
        }
        public Gomoku(string style,string p1, string p2)
        {
            
                CurrentGame = new Board(GameStyles.Get(style));
                CurrentPlayers.Add(p1);
                CurrentPlayers.Add(p2);
                CurrentMove = new Intersection();
        }

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
