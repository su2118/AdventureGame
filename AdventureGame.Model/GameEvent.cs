using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame.AdventureGame.Model
{
    public class GameEvent
    {
        public int EventID { get; set; }

        public string EventName { get; set; }


        public string EventText { get; set; }
                    
        public bool IsYesNoQuestion { get; set; }


        public int? NextEventYes {  get; set; }

        public int? NextEventNo { get; set; }

        public int? MoveToChapter { get; set; }

        public int? RedirectToEventID { get; set; }

        public int ChapterID { get; set; }

        public string RequiredItem { get; set; }

        public string GrantsItem { get; set; }

        public bool IsRandomEvent { get; set; }

        public GameStatus GameStatus { get; set; }
    }
}
