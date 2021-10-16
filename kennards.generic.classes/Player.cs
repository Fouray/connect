using System;
using System.Collections.Generic;
using System.Text;

namespace kennards.generic.classes
{
    public class Player
    {
        public string Name { get; set; }

        //Can add more properties to player as needed.

        public Player()
        {
            Name = "";
        }
        public Player(string name)
        {

            Name = name;

        }
       

        public string PlayerName(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                Name = name;
            }
            return name;
        }
    }

    public class Players
    {
        public List<Player> PlayerList { get; set; } = new List<Player>();

        public Players()
        {
            PlayerList = new List<Player>();
        }
        public Players(Player player)
        {
            PlayerList.Add(player);
        }
        public Players(List<Player> players)
        {
            PlayerList=players;
        }

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

        public bool Find(Player p)
        {
            return PlayerList.FindAll(f => f.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase))!=null;
        }


    }
}
