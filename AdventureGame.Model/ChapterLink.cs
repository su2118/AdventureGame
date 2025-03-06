using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame.AdventureGame.Model
{
    public class ChapterLink
    {
        public int LinkID {  get; set; }

        public int FromChapterID { get; set; }

        public int ToChapterID { get; set; }

        public string ChapterCondition {  get; set; }

    }
}
