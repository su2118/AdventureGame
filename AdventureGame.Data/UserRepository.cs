using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventureGame.AdventureGame.Model;

namespace AdventureGame.AdventureGame.Data
{
    public class UserRepository
    {
        public bool CreateUser(string username, string password)
        {
            string query = @"INSERT INTO User (UserName, Password) VALUES (@username, @password)";

            var parameters = new Dictionary<string, object>
            {
               { "@username", username },
               { "@password", password }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public int GetUserID(string username)
        {
            string query = @"Select UserID FROM User WHERE UserName = @username";

            var parameters = new Dictionary<string, object> { { "@username", username } };
            var result = DatabaseHelper.ExecuteQuery(query, parameters);

            if (result.Count() > 0)
            {
                return Convert.ToInt32(result[0]["UserID"]);
            }
            return -1; 
        }

        public string GetPassword(string username)
        {
            string query = @"Select Password FROM User WHERE UserName = @username";

            var parameters = new Dictionary<string, object> { { "@username", username } };
            var result = DatabaseHelper.ExecuteQuery(query, parameters);

            if (result.Count() > 0)
            {
                return result[0]["Password"].ToString();
            }
            return null;

        }


        public bool UserExist(string username)
        {
            string query = @"Select Count(*) from User WHERE Username = @username";

            var parameters = new Dictionary<string, object> { { "@username", username } };

            object result = DatabaseHelper.ExecuteScalar(query, parameters);
          
            int count = (result != null && int.TryParse(result.ToString(), out int value)) ? value: 0;

            return count > 0;

        }
    }
}
