using System;
using System.Collections.Generic;

namespace kennards.generic.classes
{
    public class GameStyle
    {
        public int Size { get; set; } = 15;
        public string Pattern { get; set; } = "xxxxx";
        public string Name { get; set; }

        public GameStyle(string name, int boardSize, string pattern)
        {
            Size = boardSize;
            Pattern = pattern;
            Name = name;
        }

    }

    public static class GameStyles
    {
        static List<GameStyle> Styles { get; set; } = new List<GameStyle>();
        static GameStyles()
        {

            //The following could be sourced from any form of configuration based data store
            Styles.Add(new GameStyle("Standard", 15, "XXXXX"));
            Styles.Add(new GameStyle("Alternate", 20, "X.X.X.X.X"));

        }
        public static GameStyle Get(string name)
        {
            if (Styles.Count < 1)
            {
                throw new InvalidOperationException("Contructor for GameStyles Failed to run");
            }
            if (String.IsNullOrEmpty(name))
            {
                name = "standard";
            }
            GameStyle game = Styles.Find(f => f.Name.Equals(name,StringComparison.OrdinalIgnoreCase));
            if(game == null)
            {
                throw new ArgumentException($"{name} Style is not defined as a known game.");
            }
            return game;
        }
    }
    [Serializable]
    public class Gomoku
    {

        public Board CurrentGame { get; set; }
        public Players CurrentPlayers { get; set; } = new Players();

        public Intersection CurrentMove { get; set; }

        public BoardResult State { get; set; }

        public Gomoku()
        {
            CurrentGame = new Board();
            CurrentPlayers = new Players();
            CurrentMove = new Intersection();
            State = BoardResult.inital;
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
                if (CurrentGame.Move(CurrentMove))
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
