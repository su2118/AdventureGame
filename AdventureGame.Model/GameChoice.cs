using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame.AdventureGame.Model
{
    public class GameChoice
    {
        public int ChoiceID { get; set; }

        public int PlayerID { get; set; }

        public int ChapterID { get; set; }

        public string ChoiceMade { get; set; }
    }
}
