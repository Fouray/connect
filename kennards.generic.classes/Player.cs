using System;
using System.Collections.Generic;
using System.Text;

namespace kennards.generic.classes
{
 /// <summary>
  /// Player - A object for player ***Only storing name but could be expanded as needed without affecting the game. 
  ///     <list type="bullet">
  ///         <item>
  ///             <term>Name String</term>
  ///             <description>
  ///                The name of the player
  ///             </description>
  ///         </item>
  /// </summary>
  /// 
    public class Player
    {
        public string Name { get; set; }

        //Can add more properties to player as needed.

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Player()
        {
            Name = "";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name" type="string">The name of the player</param>
        public Player(string name)
        {

            Name = name;

        }
       
        /// <summary>
        /// Get or set the player name
        /// </summary>
        /// <param name="name" type="string" optional>If null does nothign but return current name. If has a value sets name to new value and returns new name</param>
        /// <returns></returns>
        public string PlayerName(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                Name = name;
            }
            return name;
        }
    }
    /// <summary>
    /// A List of <see cref="Player">Player</see>"/>
    /// </summary>
    public class Players
    {
        public List<Player> PlayerList { get; set; } = new List<Player>();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Players()
        {
            PlayerList = new List<Player>();
        }

        /// <summary>
        /// Constructor with <see cref="Player">Player</see>"/>
        /// </summary>
        /// <param name="player" type="Player">the Player to be added to the list</param>
        public Players(Player player)
        {
            PlayerList.Add(player);
        }

        /// <summary>
        /// Replace existing players with a list of <see cref="Player">Player</see>
        /// </summary>
        /// <param name="players">The list of players</param>
        public Players(List<Player> players)
        {
            PlayerList=players;
        }

        /// <summary>
        /// Override list add with a name
        /// </summary>
        /// <param name="name" optional> If the name is empty sets the name to a value</param>
        /// <exception cref="ArgumentException">If name already exists in player list raise exception</exception>
        public void Add(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                name = $"No Name {PlayerList.FindAll(f => f.Name.Contains("No Name", StringComparison.OrdinalIgnoreCase)).Count}";
            }
            Player found = PlayerList.Find(f => f.Name.Equals(name,StringComparison.OrdinalIgnoreCase));
            if (found == null)
            {
                PlayerList.Add(new Player(name));
            }
            else
            {
                throw new ArgumentException($"{name} is already a player in this game. Players must have unique names per game.");
            }
        }

        /// <summary>
        /// Add a <see cref="Player">Player</see>>Player to a Players
        /// </summary>
        /// <param name="p" type="Player">The new Player to be added</param>
        /// <exception cref="ArgumentException">If player already exists in list</exception>
        public void Add(Player p)
        {
            if (p==null)
            {
                throw new ArgumentException("Object Player was not valid.");
            }
            Player found = PlayerList.Find(f => f.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase));
            if (found == null)
            {
                PlayerList.Add(p);
            }
            else
            {
                throw new ArgumentException($"{p.Name} Player already a player in this game. Players must have unique names per game.");
            }
        }

        /// <summary>
        /// Find a player in the list
        /// </summary>
        /// <param name="p" type="Player">The player being searched for</param>
        /// <returns type="bool">True if found false if not</returns>
        public bool Find(Player p)
        {
            return PlayerList.FindAll(f => f.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase))!=null;
        }


    }
}
