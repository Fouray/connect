using System;
using System.Collections.Generic;
using System.Text;

namespace kennards.generic.classes
{
    /// <summary>
    /// The Game style is to allow for multiple types of games to be played in the same environment
    /// In the case of Gomoku it is a 15x15 board with a goal of 4 in a row
    /// If the parameters are changed to a 3x3 board with a s pattern then that game is Tick-Tack-Toe
    /// Many other games could be made using the same engine.
    ///     <list type="bullet">
    ///         <item>
    ///             <term>Size</term>
    ///             <description>The size of the board. All boards are assumed to be square</description>
    ///         </item>
    ///         <item>
    ///             <term>Pattern</term>
    ///             <description>A string to represent the winning result.
    ///                 <list type="bullet">
    ///                     <item>
    ///                         <term>X</term>
    ///                         <description>
    ///                             the players tile
    ///                         </description>
    ///                     </item>
    ///                     <item>
    ///                         <term>.</term>
    ///                         <description>
    ///                             the opponents tile
    ///                         </description>
    ///                     </item><item>
    ///                         <term>space</term>
    ///                         <description>
    ///                             Neither tile
    ///                         </description>
    ///                     </item>
    ///                 </list>
    ///             </description>
    ///             <example>
    ///                     XXXXX Gomoku
    ///                     X.X.X.X LeemoKu :)
    ///                     xxx Tic-tack-toe
    ///             </example>
    ///         </item>
    ///         <item>
    ///             <term>name</term>
    ///             <description>Unique Name for each game</description>
    ///         </item>
    ///     </list>
    /// </summary>
    public class GameStyle
    {
        public int Size { get; set; } = 15;
        public string Pattern { get; set; } = "XXXXX";
        public string Name { get; set; }

        public GameStyle(string name, int boardSize, string pattern)
        {
            Size = boardSize;
            Pattern = pattern;
            Name = name;
        }

    }

    /// <summary>
    /// List of <see cref="GameStyle">GameStyle</see>. It would be expected that these are loaded from a config or data store.
    ///    
    /// </summary>
    public static class GameStyles
    {
        static List<GameStyle> Styles { get; set; } = new List<GameStyle>();
        static GameStyles()
        {

            //The following could be sourced from any form of configuration based data store
            if (Styles.Count == 0)
            {
                Styles.Add(new GameStyle("Standard", 15, "XXXXX"));
                Styles.Add(new GameStyle("Leemoku", 20, "X.X.X.X.X"));
                Styles.Add(new GameStyle("TicTackToe", 3, "XXX"));
            }

        }

        /// <summary>
        /// Get the game style
        /// </summary>
        /// <param name="name" optional>String with the unique name of the game. If null the default Standard is Used</param>
        /// <exception cref="ArgumentException">If GameStyle not found then wrong argument passed.</exception>
        /// <returns><see cref="GameStyle">GameStyler</see></returns>
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
