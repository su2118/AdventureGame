using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdventureGame.AdventureGame.Business;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace AdventureGame.AdventureGame.UI
{
    public partial class GameForm : Form
    {
        private GameManger gameManger;

        private int currentStoryIndex = 1;
        public GameForm()
        {
            InitializeComponent();
            gameManger = new GameManger();
            LoadChapter(currentStoryIndex);
        }
        private void LoadChapter(int chapterID)
        {
            gameManger.LoadChapter(chapterID); 
            lblChapterName.Text = gameManger.CurrentChapter.ChapterName;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            LoadChapter(currentStoryIndex+1);
        }
    }
}
