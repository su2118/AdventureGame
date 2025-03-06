using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventureGame.AdventureGame.Data;
using AdventureGame.AdventureGame.Model;

namespace AdventureGame.AdventureGame.Business
{
    public class GameManger
    {
        private GameRepository repository;

        public GameChapter CurrentChapter { get; private set; }

        public GameManger()
        {
            repository = new GameRepository();

        }
        public void LoadChapter(int chapterID)
        {
            CurrentChapter = repository.GetChapter(chapterID);
        }
    }
}
