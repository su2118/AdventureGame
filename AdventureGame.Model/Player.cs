using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame.AdventureGame.Model
{
    public class Player
    {
        public int UserID { get; set; }

        public int EventID { get; set; }

        public GameStatus GameStatus { get; set; }

        public bool HasArm {  get; set; }

        public DateTime LastSaved { get; set; }
    }
}
