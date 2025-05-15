using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AdventureGame.AdventureGame.Data;
using ZstdSharp.Unsafe;

namespace AdventureGame.AdventureGame.Business
{
    public class UserManager
    {
        private UserRepository userRepository = new UserRepository();

        public bool RegisterUser(string username, string password)
        {
            if(string.IsNullOrWhiteSpace(username) || username == "UserName" || password =="Password" || string.IsNullOrWhiteSpace(password))
            {
                return false; // prevent empty name or password
            }
            if (userRepository.UserExist(username))
            {
                return false; //Username already exists
            }
            string passwordHash = HashPassword(password);
            return userRepository.CreateUser(username, passwordHash);
        }
        public int LoginUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return -1; // prevent empty name or password
            }
            string storedHash = userRepository.GetPassword(username);

            if (storedHash != null && VerifyPassword (password,storedHash) )
            {
                return userRepository.GetUserID(username);
            }
            return -1; //Invalid login
        }
        public string HashPassword(string password) 
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
        public bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
        public int getUserID(string username)
        {
            return userRepository.GetUserID(username);
        }
    }
}
