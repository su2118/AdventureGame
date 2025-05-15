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

        private GameEvent gameEvent;

     //   private int currentPlayerId;

        private int userId; 
        public GameForm(int userId)
        {
            InitializeComponent();
            gameManager = new GameManager();
            gameRepository = new GameRepository();
            this.userId = userId;
           // LoadEvent(1);
           LoadPlayerProgress();
        }

        // Load and display a game event
        private async void LoadEvent(int eventId)
        {
            HideYesNoButtons(); // Hide buttons when loading new event
            currentEventId = eventId;
            Console.WriteLine($"Current EventID : {eventId}");
            gameEvent = gameManager.GetGameEvent(eventId);
            if (gameEvent == null)
            {
                MessageBox.Show("Error loading event. Please check the database.");
                return;
            }
            lblStoryText.Text = gameEvent.EventText;
            pnlChoices.Controls.Clear(); // Clear previous choices

            HandleGrantsItem(gameEvent);

            Console.WriteLine("Random:" + gameEvent.IsRandomEvent);
            if (gameEvent.IsRandomEvent)
            {
                Console.WriteLine("It is a random event");
                await HandleRandomEvent();
                return;
            }

            //ChapterName 
            string chapterName = gameManager.GetChapterNameForEvent(eventId);
            if (chapterName != "Unknown Chapter")
            {
                lblChapterName.Text = $"Chapter: {chapterName}";
            }
            else
            {
                lblChapterName.Text = "Moving to Next Chapter";
            }

            //redirecting to the menu if the player does not have items
            if (gameEvent.RedirectToEventID.HasValue)
            {
                Console.WriteLine($"Redirecting to Menu: {gameEvent.RedirectToEventID.Value}(Chapater Menu)");
                await Task.Delay(1500);

                LoadEvent(gameEvent.RedirectToEventID.Value);
                return;
            }
            //check if this event has items to pick up 
            List<string> foundItems = gameManager.GetEventItem(eventId);
            if (foundItems.Count > 0)
            {
                ShowItemSelection(eventId);
                return;
            }

            if (gameEvent.IsYesNoQuestion && !gameEvent.IsRandomEvent) //Check if it is yes no question
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
            CheckGameStatus(gameEvent);
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
               // GameEvent nextEvent = gameManager.GetGameEvent(nextEventId);
                if (!string.IsNullOrEmpty(gameEvent.RequiredItem))
                {
                    List<string> requiredItems = gameEvent.RequiredItem
                                                .Split(',')
                                                .Select(x => x.Trim())
                                                .ToList();
                    if (gameManager.HasAllItems(userId, requiredItems))
                    {
                        gameManager.UseItems(userId, requiredItems);
                        gameManager.SaveProgress(userId, nextEventId);
                        LoadEvent(nextEventId);
                    }
                    else
                    {
                        MessageBox.Show($"You need: {gameEvent.RequiredItem}.\nYou don't have the required item(s) yet");
                        return;
                    }

                   // bool hasAllItems = gameManager.HasAllItems(userId, requiredItems);
                }
               // LoadEvent(nextEventId);
                HideYesNoButtons();
            }
        }
        // Handle No button click
        private void BtnNo_Click(object sender, EventArgs e)
        {
            if (btnNo.Tag is int nextEventId && nextEventId > 0)
            {
                if (!string.IsNullOrEmpty(gameEvent.RequiredItem))
                {
                    List<string> requiredItems = gameEvent.RequiredItem
                                                .Split(',')
                                                .Select(x => x.Trim())
                                                .ToList();
                    if (gameManager.HasAllItems(userId, requiredItems))
                    {
                        DialogResult result = MessageBox.Show(
                            $"You have the required items : {gameEvent.RequiredItem}. Please press Yes",
                            "Warning Message",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                            );

                        if (result == DialogResult.OK)
                        {
                            return;// Cancel and stay on current event
                        }

                    }
                }
                gameManager.SaveProgress(userId, nextEventId);
                LoadEvent(nextEventId);
                HideYesNoButtons();
            }
        }
        // Process player choice
        private void ProcessChoice(int eventId)
        {
            Console.WriteLine($"Attempting to load event {eventId}");
            gameManager.SaveProgress(userId,eventId);
            LoadEvent(eventId);
        }
        private void ShowItemSelection(int eventId)
        {
            GameEvent gameEvent = gameManager.GetGameEvent(eventId);
            List<string> foundItems = gameManager.GetEventItem(eventId);
            List<CheckBox> checkBoxes = new List<CheckBox>();
            int yOffset = 10;

            pnlChoices.Controls.Clear();

            //create check box for found items
            foreach (string item in foundItems)
            {
                CheckBox chkItem = new CheckBox()
                {
                    Text = item,
                    Tag = item,
                    Location = new Point(10, yOffset),
                    AutoSize = true
                };

                checkBoxes.Add(chkItem);
                pnlChoices.Controls.Add(chkItem);
                yOffset += 30;
            }

            //Add confirm selection button
            Button btnConfirm = new Button()
            {
                Text = "Pick Selected Items",
                Location = new Point(10, yOffset),
                Width = 200
            };
            btnConfirm.Click += (sender, e) =>
            {
                if (gameEvent.NextEventYes.HasValue)
                {
                    LoadEvent(gameEvent.NextEventYes.Value);
                }
                PickSelectedItems(checkBoxes, eventId);
            };
            pnlChoices.Controls.Add(btnConfirm);
            
        }

          private void btnInventory_Click(object sender, EventArgs e)
          {
              InventoryForm inventoryForm = new InventoryForm(userId);
              inventoryForm.ShowDialog();
          }

        private void PickSelectedItems(List<CheckBox> checkBoxes, int eventId)
        {
            List<string> selectItems = checkBoxes
                .Where(chk => chk.Checked) //only check items
                .Select(chk => chk.Tag.ToString()) //covert to string
                .ToList();
            if (selectItems.Count == 0)
            {
                MessageBox.Show("Please select at least one item");
                return;
            }
           
            gameManager.AddItemToPlayerInventory(userId, selectItems);
            
            MessageBox.Show($"You picked up :{string.Join(", ", selectItems)}");

        }

        private void HandleGrantsItem(GameEvent currentEvent)
        {
            if(string.IsNullOrEmpty(currentEvent.GrantsItem))
            {
                return;
            }
            string item = currentEvent.GrantsItem.Trim();

            Console.WriteLine("Item Name: " + item);

            if(item.Equals("Arm",StringComparison.OrdinalIgnoreCase))
            {
                if(!gameManager.IsArmAttached(userId))
                {
                    gameManager.SetArmAttached(userId, true);
                    MessageBox.Show("Rusty's Arm is now attached!");
                }
            }
            else
            {
                int itemId = gameManager.GetInventoryIDByName(item);
                int quantity = 1;
                gameManager.AddPlayerInventory(userId,itemId,quantity);
                MessageBox.Show($"You received:{item}");
            }
        }
        private void LoadPlayerProgress()
        {
            Player player = gameManager.GetPlayer(userId);
            if (player == null)
            {
                //Create new player 
                gameRepository.CreateNewPlayer(userId);
                LoadEvent(1);
            }
            else
            {
                LoadEvent(player.EventID);
            }
        }
        private async Task HandleRandomEvent()
        {
            //Creating loading progress
            ProgressBar progressBar = new ProgressBar()
            {
                Style= ProgressBarStyle.Marquee,
                Width = 200,
                Height = 20,
                Location = new Point(10,10),
                MarqueeAnimationSpeed = 50
            };
            pnlChoices.Controls.Add(progressBar);
            progressBar.Visible = true;
            await Task.Delay(3000);

            Random rand = new Random();
            int chance = rand.Next(0, 100); //0 to 99
            bool success = chance < 70;

            int nextEventId = success ? gameEvent.NextEventYes.Value : gameEvent.NextEventNo.Value;
            LoadEvent(nextEventId);
        }
        private void CheckGameStatus(GameEvent gameEvent)
        {
            if(gameEvent.GameStatus == GameStatus.GameOver)
            { 
                gameManager.SaveStatus(userId, GameStatus.GameOver);
                MessageBox.Show("Game Over!");
            }
            else if (gameEvent.GameStatus == GameStatus.Completed)
            {
                gameManager.SaveStatus(userId, GameStatus.Completed);
                MessageBox.Show("Game Completed!");
            }
        }
    }
}
