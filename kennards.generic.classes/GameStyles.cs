using System;
using System.Collections.Generic;
using System.Text;

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
            if (Styles.Count == 0)
            {
                Styles.Add(new GameStyle("Standard", 15, "XXXXX"));
                Styles.Add(new GameStyle("Alternate", 20, "X.X.X.X.X"));
                Styles.Add(new GameStyle("TicTackToe", 3, "XXX"));
            }

        }
        public static GameStyle Get(string name)
        {
            if (Styles.Count == 0)
            {
                throw new InvalidOperationException("Contructor for GameStyles Failed to run");
            }
            if (String.IsNullOrEmpty(name))
            {
                name = "standard";
            }
            GameStyle game = Styles.Find(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (game == null)
            {
                throw new ArgumentException($"{name} Style is not defined as a known game.");
            }
            return game;
        }
    }
}
