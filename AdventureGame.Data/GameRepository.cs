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
        //Get chapter name for each event
        public string GetChapterNameForEvent(int eventId)
        {
            string query = @"SELECT gc.ChapterName from GameChapter gc 
                             JOIN GameEvent ge ON gc.ChapterID = ge.ChapterID 
                             WHERE ge.EventID = @eventId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@eventId", eventId } };

            object result = DatabaseHelper.ExecuteScalar(query, parameters);
            return result != null ? result.ToString() : "Unknown Chapter";
        }

        // Get game event 
        public GameEvent GetEvent(int eventId)
        {
            string query = @"SELECT * FROM GameEvent 
                             WHERE EventID = @eventId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@eventId", eventId } };
            var result = DatabaseHelper.ExecuteQuery(query, parameters);
            if (result.Count == 0) return null; // No event found
            Dictionary<string, object> row = result[0];
            return new GameEvent
            {
                EventID = Convert.ToInt32(row["EventID"]),
                EventText = row["EventText"].ToString(),
                IsYesNoQuestion = Convert.ToBoolean(row["IsYesNoQuestion"]),
                NextEventYes = row["NextEventYes"] as int?,
                NextEventNo = row["NextEventNo"] as int?,
                MoveToChapter = row["MoveToChapter"] as int?,
                RedirectToEventID = row["RedirectToEventID"] as int?,
                RequiredItem = row["RequiredItem"].ToString(),
                GrantsItem = row["GrantsItem"].ToString(),
                IsRandomEvent = Convert.ToBoolean(row["IsRandomEvent"]),
                GameStatus = (GameStatus)Convert.ToInt32(row["GameStatus"])
            };
        }


        // Get all available choices for an event
        public List<GameEvent> GetChoices(int eventId)
        {
            string query = @"SELECT g.EventID, g.EventName, c.ChoiceText
                            From ChapterChoice c 
                            JOIN GameEvent g ON c.ToEventID = g.EventID 
                            WHERE c.FromEventID = @eventId";
           
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@eventId", eventId } };
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

        //Get Player Inventory 
        public List<PlayerInventory> GetPlayerInventory(int userId)
        {
            string query = @"SELECT pi.UserID, pi.InventoryID, i.ItemName, pi.Quantity 
                           from PlayerInventory pi
                           JOIN Inventory i ON pi.InventoryID = i.InventoryID
                           WHERE pi.UserID = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@userId", userId } };
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

        //Get Event Item 
        public List<string> GetEventItem(int eventId)
        {
            string query = @"SELECT i.ItemName 
                             FROM EventItem ei
                             JOIN Inventory i ON ei.InventoryID = i.InventoryID
                             WHERE ei.EventID = @eventId";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@eventId", eventId } };
            var result = DatabaseHelper.ExecuteQuery(query, parameters);

            List<string> items = new List<string>();

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

        //Add Items to Player Inventory 
        public void AddItemToPlayerInventory(int userId, List<int> inventoryIds)
        {
            if(inventoryIds.Count == 0) return; // No items to addd

            string query = @"INSERT INTO PlayerInventory (userID, InventoryID) VALUES ";

            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@userId", userId } };

            Console.WriteLine("List of item ID"+inventoryIds);

            List<string> values = new List<string>();
            for (int i = 0; i < inventoryIds.Count; i++) 
            {
                string paramName = "@item" + i;
                values.Add($"(@userId,{paramName})");

                parameters[paramName] = inventoryIds[i];

            }
            query += string.Join(",", values);
            DatabaseHelper.ExecuteNonQuery(query, parameters);
        }

        // Get Inventory ID from Inventory Table by Item Name
        public int GetInventoryIDByName(string itemName)
        {
            string query = @"SELECT InventoryID FROM Inventory WHERE ItemName = @itemName";

            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@itemName", itemName } };

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

        //Get Inventory IDs from Inventory Table by Item Name
        public List<int> GetInventoryIDsByName(List<string> itemName)
        {
            string placeholder = string.Join(",", itemName.Select((_,i) => $"@name{i}"));
            string query = $@"SELECT InventoryID FROM Inventory WHERE ItemName IN({placeholder})";

            Dictionary<string, object> parameters = new Dictionary<string, object> ();
            for(int i=0; i<itemName.Count; i++)
            {
                parameters.Add($"@name{i}", itemName[i]);
            }
            var result = DatabaseHelper.ExecuteQuery(query, parameters);
            return result.Select(row => Convert.ToInt32(row["InventoryID"])).ToList();
        }


        //Update the item quantity in Player Inventory 
        public void UpdatePlayerInventory(int userId, Dictionary<int, int> itemQuantites)
        {
            string query = @"UPDATE PlayerInventory SET Quantity = @quantity
                            WHERE UserID = @userId AND InventoryID = @itemId";
            foreach (var item in itemQuantites) 
            {
                Dictionary<string,object> parameters = new Dictionary<string, object>
                {
                    { "@userId", userId },
                    { "@itemId", item.Key },
                    { "@quantity", item.Value }
                };
                DatabaseHelper.ExecuteNonQuery(query, parameters);
            }
        }
        public void AddPlayerInventory(int userId,int itemId, int quantity)
        {
            string query = @"INSERT INTO PlayerInventory (userID, InventoryID, Quantity) VALUES ";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@userId", userId },
                { "@itemId", itemId },
                { "@quantity", quantity }
            };
            DatabaseHelper.ExecuteNonQuery(query, parameters);
            
        }

        //Check if the player has item in the player inventory to use 
        public bool PlayerHasItem(int userId, string itemName)
        {
            string query = @"SELECT COUNT(*) FROM PlayerInventory pi
                            JOIN Inventory i ON pi.InventoryID = i.InventoryID
                            WHERE pi.UserID = @userId AND i.ItemName = @itemName AND pi.Quantity > 0";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@userId", userId },
                { "@itemName", itemName }
            };
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters)) > 0;
        }

        //Set item quantity - 1
        public void UseItem(int userId, string itemName)
        {
            string query = @"UPDATE PlayerInventory pi
                            JOIN Inventory i ON pi.InventoryID = i.InventoryID
                            SET pi.Quantity = pi.Quantity - 1
                            WHERE pi.UserID = @userId AND i.ItemName = @itemName AND pi.Quantity > 0";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@userId", userId },
                { "@itemName", itemName }
            };

            DatabaseHelper.ExecuteNonQuery(query,parameters);
        }

        //Update Arm status
        public void UpdateArmStatus(int userId, bool hasArm)
        {
            string query = @"UPDATE Player SET HasArm = @hasArm WHERE UserID = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@userId", userId },
                { "@hasArm", hasArm }
            };

            DatabaseHelper.ExecuteNonQuery(query, parameters);
        }

        public bool GetArmStatus(int userId)
        {
            string query = @"SELECT HasArm FROM Player WHERE UserID = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@userId", userId }
            };
            return Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters)) > 0;
        }

        public void CreateNewPlayer(int userId)
        {
            string query = @"INSERT INTO Player (UserID, EventID, GameStatus, LastSaved, HasArm) VALUES
                            (@userId, 1, 0, Now(), FALSE)";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@userId", userId }
            };
            DatabaseHelper.ExecuteNonQuery(query, parameters);
        }

        public Player GetPlayer(int userId)
        {
            string query = @"SELECT * FROM Player WHERE UserID = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@userId", userId }
            };
            var result = DatabaseHelper.ExecuteQuery(query, parameters);
            if (result.Count == 0) return null; // No event found
            Dictionary<string, object> row = result[0];
            return new Player
            {
                UserID = Convert.ToInt32(row["UserID"]),
                EventID = Convert.ToInt32(row["EventID"]),
                GameStatus = (GameStatus)Convert.ToInt32(row["GameStatus"]),
                LastSaved = Convert.ToDateTime(row["LastSaved"]),
                HasArm = Convert.ToBoolean(row["HasArm"])

            };
        }
        public void UpdatePlayerProgress(int userId, int eventId)
        {
            string query = @"UPDATE Player SET EventID = @eventId, LastSaved = NOW() WHERE UserID = @userId" ;
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@userId", userId },
                {"@eventId", eventId }
            };
            DatabaseHelper.ExecuteNonQuery(query, parameters);
        }
        public void UpdatePlayerStatus(int userId, GameStatus status)
        {
            string query = @"UPDATE Player SET GameStatus = @status WHERE UserID = @userId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@userId", userId },
                {"@status", (int)status }
            };
            DatabaseHelper.ExecuteNonQuery(query, parameters);
        }
    }
}
