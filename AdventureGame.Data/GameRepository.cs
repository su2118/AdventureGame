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
        private string conString = "server=localhost;uid=root;pwd=512788;database=test;";
        
        public GameChapter GetChapter (int chapterID)
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
        }

    }
}
