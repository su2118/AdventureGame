using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventureGame.AdventureGame.Model;
using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using MySql.Data.MySqlClient;
using static DevExpress.XtraEditors.Mask.MaskSettings;

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

        public List<PlayerInventory> GetPlayerInventory(int userId)
        {
            string query = @"SELECT pi.UserID, pi.InventoryID, i.ItemName, pi.Quantity 
                           from PlayerInventory pi
                           JOIN Inventory i ON pi.InventoryID = i.InventoryID
                           WHERE pi.UserID = @userId";
            var parameters = new Dictionary<string, object> { { "@userId", userId } };
            var result = DatabaseHelper.ExecuteQuery(query, parameters);

            List<PlayerInventory> inventory = new List<PlayerInventory>();
            foreach (var row in result)
            {
                inventory.Add(new PlayerInventory
                {
                    UserID = Convert.ToInt32(row["UserID"]),
                    InventoryID = Convert.ToInt32(row["InventoryID"]),
                    ItemName = row["ItemName"].ToString(),
                    Quantity = Convert.ToInt32(row["Quantity"])
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

    /*    public void AddItemToPlayerInventory(int userId, string item)
        {

            string query = @"INSERT INTO PlayerInventory (userID, InventoryID) VALUES (@userId, @item)";

            var parameters = new Dictionary<string, object>
            {
               { "@userId", userId },
               { "@item", item }
            };
             DatabaseHelper.ExecuteNonQuery(query, parameters);
        }*/

        public void AddItemToPlayerInventory(int userId, List<int> inventoryIds)
        {
            if(inventoryIds.Count == 0) return; // No items to addd

            string query = @"INSERT INTO PlayerInventory (userID, InventoryID) VALUES ";

            var parameters = new Dictionary<string, object> { { "@userId", userId } };

            Console.WriteLine("List of item ID"+inventoryIds);

            List<string> values = new List<string>();
            for (int i = 0; i < inventoryIds.Count; i++) 
            {
                string paramName = "@item" + i;
                values.Add($"(@userId,{paramName})");

                parameters[paramName] = inventoryIds[i];

            }
            query += string.Join(",", values);

            Console.WriteLine("Final Query"+query);
            DatabaseHelper.ExecuteNonQuery(query, parameters);
        }


        public int GetInventoryIDByName(string itemName)
        {
            string query = @"Select InventoryID FROM Inventory WHERE ItemName = @itemName";

            var parameters = new Dictionary<string, object> { { "@itemName", itemName } };

            var result = DatabaseHelper.ExecuteQuery(query,parameters);

            return result.Count == 1 ? Convert.ToInt32(result[0]["InventoryID"]) : -1; 
        }

       /* public List<int> GetInventoryIdsByName(List<string> itemNames)
        {
            List<int> inventoryIds = new List<int>();

            foreach(var itemName in itemNames)
            {
                int inventoryId = GetInventoryIDByName(itemName);
                if(inventoryId != 1)
                {
                    inventoryIds.Add(inventoryId);
                }
            }

            return inventoryIds;
        }*/

        public List<int> GetInventoryIDsByName(List<string> itemName)
        {
            string placeholder = string.Join(",", itemName.Select((_,i) => $"@name{i}"));
            string query = $@"Select InventoryID FROM Inventory WHERE ItemName IN({placeholder})";

            var parameters = new Dictionary<string, object> ();
            for(int i=0; i<itemName.Count; i++)
            {
                parameters.Add($"@name{i}", itemName[i]);
            }
            var result = DatabaseHelper.ExecuteQuery(query, parameters);
            return result.Select(row => Convert.ToInt32(row["InventoryID"])).ToList();
        }

        public void UpdatePlayerInventory(int userId, Dictionary<int, int> itemQuantites)
        {
            string query = @"Update PlayerInventory SET Quantity = @quantity
                            WHERE UserID = @userId AND InventoryID = @itemId";
            foreach (var item in itemQuantites) 
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@userId", userId },
                    { "@itemId", item.Key },
                    { "@quantity", item.Value }
                };
                DatabaseHelper.ExecuteNonQuery(query, parameters);
            }
        }

    }
}
