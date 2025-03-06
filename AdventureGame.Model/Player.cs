using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame.AdventureGame.Model
{
    public class Player
    {
        public int PlayerID { get; set; }

        public string PlayerName { get; set; }

        public int LivesRemaining { get; set; }

        public string GameStatus { get; set; }
    }
}
