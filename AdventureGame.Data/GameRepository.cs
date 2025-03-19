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
        //private string conString = "server=localhost;uid=root;pwd=512788;database=test;";

        /*public GameChapter GetChapter (int chapterID)
        {
            using (MySqlConnection con = new MySqlConnection(conString))
            {
                con.Open();

                string query = "Select ChapterID, ChapterName FROM GameChapter " +
                    "WHERE ChapterID = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", chapterID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new GameChapter
                            {
                                ChapterID = Convert.ToInt32(reader["ChapterID"]),

                                ChapterName = reader["ChapterName"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }*/

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
                RedirectToMenu = Convert.ToBoolean(row["RedirectToMenu"]),
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
    }
}
