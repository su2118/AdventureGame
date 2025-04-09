using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdventureGame.AdventureGame.Data;
using AdventureGame.AdventureGame.Model;
using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using DevExpress.XtraPrinting.Native.Interaction;

namespace AdventureGame.AdventureGame.Business
{
    public class GameManager
    {
        private GameRepository repository;

        public GameChapter CurrentChapter { get; private set; }

        public int UserID { get; private set; }=-1; //Default: not logged in

        public void SetUser(int userId)
        {
            this.UserID = userId;
        }

        public GameManager()
        {
            repository = new GameRepository();

        }
        public void LoadChapter(int chapterID)
        {
            //CurrentChapter = repository.GetChapter(chapterID);
        }

        // Fetch and validate a game event
        public GameEvent GetGameEvent(int eventId)
        {
            GameEvent gameEvent = repository.GetEvent(eventId);
            if (gameEvent == null)
            {
                throw new Exception($"Game event with ID {eventId} not found.");
            }
            return gameEvent;
        }
        // Get available choices for an event
        public List<GameEvent> GetChoices(int eventId)
        {
            return repository.GetChoices(eventId);
        }
        public string GetChapterNameForEvent (int eventId)
        {
            return repository.GetChapterNameForEvent(eventId);
        }
        public List<PlayerInventory> GetPlayerInventory(int userId)
        {
            return repository.GetPlayerInventory(userId);
        }
        public List<string> GetEventItem(int eventId)
        {
            return repository.GetEventItem(eventId);
        }
        public void AddItemToPlayerInventory(int userId, List<string> itemNames)
        {
            List<int> itemIds = repository.GetInventoryIDsByName(itemNames);
            if (itemIds.Count > 0)
            {
                repository.AddItemToPlayerInventory(userId, itemIds);
            }
            else
            {
                MessageBox.Show("Error");
            }
        }
        public void UpdatePlayerInventory(int userId, List<int> selectedItems)
        {
            Dictionary<int,int> quantities = selectedItems.ToDictionary(itemId => itemId, _ => 0 );
            repository.UpdatePlayerInventory(userId, quantities);
        }
        // Handle player choice and determine next event
        /* public int ProcessChoice(int eventId, string choice)
         {
             GameEvent gameEvent = GetGameEvent(eventId);
             if (gameEvent.IsYesNoQuestion)
             {
                 return choice == "Yes" ? gameEvent.NextEventYes ?? 0 : gameEvent.NextEventNo ?? 0;
             }
             return gameEvent.NextEventYes ?? 0; // Default to NextEventYes if no Yes/No choice
         }*/
    }
}
