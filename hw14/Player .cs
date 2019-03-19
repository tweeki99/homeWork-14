using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw14
{
    public class Player
    {
        public List<Card> PlayerCards { get; set; }
        public Card ThrownCard { get; set; }
        public ConsoleColor Color { get; set; }
        
        public Player(ConsoleColor color)
        {
            Color = color;
            PlayerCards = new List<Card>();
        }
    }
}
