using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdventureGame.AdventureGame.Business;
using AdventureGame.AdventureGame.Data;
using AdventureGame.AdventureGame.Model;
using DevExpress.Export.Xl;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace AdventureGame.AdventureGame.UI
{
    public partial class GameForm : Form
    {
        private GameManager gameManager;

        private readonly GameRepository gameRepository;

        private int currentEventId;

        public GameForm()
        {
            InitializeComponent();
            gameManager = new GameManager();
            gameRepository = new GameRepository();
            LoadEvent(1);
        }

        // Load and display a game event
        private async void LoadEvent(int eventId)
        {
            HideYesNoButtons(); // Hide buttons when loading new event
            currentEventId = eventId;
            Console.WriteLine($"Current EventID : {eventId}");
            GameEvent gameEvent = gameManager.GetGameEvent(eventId);
            if (gameEvent == null)
            {
                MessageBox.Show("Error loading event. Please check the database.");
                return;
            }
            lblStoryText.Text = gameEvent.EventText;
            pnlChoices.Controls.Clear(); // Clear previous choices

            //ChapterName 
            string chapterName = gameManager.GetChapterNameForEvent(eventId);
            lblChapterName.Text = $"Chapter: {chapterName}";
            
            if(gameEvent.RedirectToEventID.HasValue)
            {
                Console.WriteLine($"Redirecting to Menu: {gameEvent.RedirectToEventID.Value}(Chapater Menu)");
                await Task.Delay(1500);

                LoadEvent(gameEvent.RedirectToEventID.Value);
                return;
            }
            if (gameEvent.IsYesNoQuestion) //Check if it is yes no question
            {
                if (gameEvent.NextEventYes.HasValue || gameEvent.NextEventNo.HasValue)
                {
                    ShowYesNoButtons(gameEvent.NextEventYes, gameEvent.NextEventNo);
                }
                else
                {
                    MessageBox.Show("No valid Yes/No choices found for this event.");
                }
            }
            else
            {
                if (gameEvent.NextEventYes.HasValue)
                {
                    await Task.Delay(2000);

                    LoadEvent(gameEvent.NextEventYes.Value);     
                            
                }
                else
                { 
                    LoadChoices(eventId);
                }
            }
        }
        // Load dynamic choices as buttons
        private void LoadChoices(int eventId)
        {
            pnlChoices.Controls.Clear();// clear previous choices
            List<GameEvent> choices = gameManager.GetChoices(eventId);

            Console.WriteLine($"Choices for Event{eventId}:{choices.Count}");

            int yOffset = 0;

            foreach (var choice in choices)
            {
                Console.WriteLine($"Adding choice button: {choice.EventText} (Event ID {choice.EventID})");

                Button btnChoice = new Button
                {
                    Text = choice.EventText,
                    Tag = choice.EventID,
                    Width = 100,
                    Height = 50,
                    Location = new Point(10,yOffset),
                    BackColor = Color.SlateGray,
                    ForeColor = Color.White,
                    Font = new Font("Tahoma",12),
                    FlatStyle = FlatStyle.Flat
                };

                yOffset += 60;
                btnChoice.Click += (sender, e) => ProcessChoice(choice.EventID);
                pnlChoices.Controls.Add(btnChoice);
                
            }
        }
        // Show Yes/No buttons for decision-making
        private void ShowYesNoButtons(int? nextYes, int? nextNo)
        {
            btnYes.Visible = true;
            btnNo.Visible = true;
            btnYes.Click -= BtnYes_Click;
            btnNo.Click -= BtnNo_Click;
            btnYes.Click += BtnYes_Click;
            btnNo.Click += BtnNo_Click;
            btnYes.Tag = nextYes ?? 0;
            btnNo.Tag = nextNo ?? 0;
            pnlChoices.Controls.Add(btnYes);
            pnlChoices.Controls.Add(btnNo);
        }
        private void HideYesNoButtons()
        {
            btnYes.Visible = false;
            btnNo.Visible = false;
        }
        // Handle Yes button click
        private void BtnYes_Click(object sender, EventArgs e)
        {
            if (btnYes.Tag is int nextEventId && nextEventId > 0)
            {
                LoadEvent(nextEventId);
                HideYesNoButtons();
            }
        }
        // Handle No button click
        private void BtnNo_Click(object sender, EventArgs e)
        {
            if (btnNo.Tag is int nextEventId && nextEventId > 0)
            {
                LoadEvent(nextEventId);
                HideYesNoButtons();
            }
        }
        // Process player choice
        private void ProcessChoice(int eventId)
        {
            Console.WriteLine($"Attempting to load event {eventId}");
            LoadEvent(eventId);
        }

        
    }
}
