using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventureGame.AdventureGame.Model;
using MySql.Data.MySqlClient;

namespace AdventureGame.AdventureGame.Data
{
    public class GameRepository
    {
        public string GetChapterNameForEvent(int eventId)
        {
            string query = "SELECT gc.ChapterName from GameChapter gc " +
                "JOIN GameEvent ge ON gc.ChapterID = ge.ChapterID " +
                "WHERE ge.EventID = @eventId";
            var parameters = new Dictionary<string, object> { { "@eventId", eventId } };

            object result = DatabaseHelper.ExecuteScalar(query, parameters);
            return result != null ? result.ToString() : "Unknown Chapter";
        }

        public GameEvent GetEvent(int eventId)
        {
            string query = "SELECT * FROM GameEvent WHERE EventID = @eventId";
            var parameters = new Dictionary<string, object> { { "@eventId", eventId } };
            var result = DatabaseHelper.ExecuteQuery(query, parameters);
            if (result.Count == 0) return null; // No event found
            var row = result[0];
            return new GameEvent
            {
                EventID = Convert.ToInt32(row["EventID"]),
                EventText = row["EventText"].ToString(),
                IsYesNoQuestion = Convert.ToBoolean(row["IsYesNoQuestion"]),
                NextEventYes = row["NextEventYes"] as int?,
                NextEventNo = row["NextEventNo"] as int?,
                MoveToChapter = row["MoveToChapter"] as int?,
                RedirectToEventID = row["RedirectToEventID"] as int?
            };
        }


        // Get all available choices for an event
        public List<GameEvent> GetChoices(int eventId)
        {
            string query = "SELECT g.EventID, g.EventName, c.ChoiceText " +
                "From ChapterChoice c " +
                "JOIN GameEvent g ON c.ToEventID = g.EventID " +
                "WHERE c.FromEventID = @eventId";
           
            var parameters = new Dictionary<string, object> { { "@eventId", eventId } };
            var result = DatabaseHelper.ExecuteQuery(query, parameters);
            List<GameEvent> choices = new List<GameEvent>();
            foreach (var row in result)
            {
                Console.WriteLine($"Found Choice: {row["ChoiceText"]} -> Event {row["EventID"]}");
                choices.Add(new GameEvent
                {
                    EventID = Convert.ToInt32(row["EventID"]),
                    EventText = row["ChoiceText"].ToString()
                });
            }
            return choices;
        }

        // To save player's choice
        public void SavePlayerChoice(int playerId, int eventId, string choice, int chosenNextEventId)
        {
            string query = "INSERT INTO PlayerChoice (PlayerID, EventID, ChoiceMade, ChosenNextEventID) " +
                           "VALUES (@playerId, @eventId, @choice, @chosenNextEventId)";
            var parameters = new Dictionary<string, object>
            {
               { "@playerId", playerId },
               { "@eventId", eventId },
               { "@choice", choice },
               { "@chosenNextEventId", chosenNextEventId }
            };
            DatabaseHelper.ExecuteNonQuery(query, parameters);
        }

        public List<PlayerInventory> GetPlayerInventory(int playerId)
        {
            string query = "SELECT pi.InventoryID, i.ItemName, pi.Quantity from PlayerInventory pi" +
                           "JOIN Inventory i ON pi.InventoryID = i.InventoryID" +
                           "WHERE pi.PlayerID = @playerId";
            var parameters = new Dictionary<string, object> { { "@playerId", playerId } };
            var result = DatabaseHelper.ExecuteQuery(query, parameters);

            List<PlayerInventory> inventory = new List<PlayerInventory>();
            foreach (var row in result)
            {
                inventory.Add(new PlayerInventory
                {
                    InventoryID = Convert.ToInt32(row["InventoryID"]),
                    ItemName = row["ItemName"].ToString(),
                    Quantity = Convert.ToInt32(row["Quantitiy"])
                });
            }
            return inventory;

        }

        public List<string> GetEventItem(int eventId)
        {
            string query = @"SELECT i.ItemName 
                             FROM EventItem ei
                             JOIN Inventory i ON ei.InventoryID = i.InventoryID
                             WHERE ei.EventID = @eventId";
            var parameters = new Dictionary<string, object> { { "@eventId", eventId } };
            var result = DatabaseHelper.ExecuteQuery(query, parameters);

            var items = new List<string>();

            foreach(var row in result)
            {
                items.Add(row["ItemName"].ToString());
            }

            return items;

        }

        public bool AddItemToPlayerInventory(int userId, string item)
        {

            string query = @"INSERT INTO PlayerInventory (userID, InventoryID) VALUES (@userId, @item)";

            var parameters = new Dictionary<string, object>
            {
               { "@userId", userId },
               { "@item", item }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public List<int> GetInventoryIDByName(string itemName)
        {
            string query = @"Select InventoryID FROM Inventory WHERE ItemName = @itemName";

            var parameters = new Dictionary<string, object> { { "@ItemName", itemName } };
            var result = DatabaseHelper.ExecuteQuery(query, parameters);
            var itemID = new List<int>();

            foreach (var row in result)
            {
                itemID.Add(Convert.ToInt32(row["InventoryID"]));
            }

            return itemID;
        }
    }
}
