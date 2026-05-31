
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
namespace hw1_Anat_Noa.Models

{
    public class User
    {
        int id;
        string name;
        string email;
        string password;
        bool active= true;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public bool Active { get => active; set => active = value; }

        public User() { }

        public User(int id, string name, string email, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }


        public List<User> Read()
        {
            DBservices db = new DBservices();
            return db.ReadUsers();
        }

        public int Register()
        {
            this.Password = CreateHash(this.Password);
            DBservices db = new DBservices();
            return db.InsertUser(this);

        }

        public User Login()
        {
            string hashPassword = CreateHash(this.Password);

            DBservices db = new DBservices();

            return db.LoginUser(this.Email, hashPassword);
        }

        private string CreateHash(string password) 
        {
            SHA256 sha = SHA256.Create();

            byte[] bytes = Encoding.UTF8.GetBytes(password);

            byte[] hashBytes = sha.ComputeHash(bytes);

            string result = "";

            for (int i = 0; i < hashBytes.Length; i++)
            {
                result += hashBytes[i].ToString("X2");
            }

            return result;
        }

        private bool CheckPassword(string inputPassword, string savedPassword)
        {
            string inputHash = CreateHash(inputPassword);
            if (inputHash == savedPassword)
                return true;
            return false;
        }

        public bool DeleteById(int id)
        {
            DBservices db = new DBservices();
            int ans = db.DeleteUser(id);
            return ans > 0;
        }

        public bool UpdateUser(int id)
        {
            this.Password = CreateHash(this.Password);

            DBservices db = new DBservices();
            int ans = db.UpdateUser(id, this);

            return ans > 0;
        }

        public bool AddGameToMe(int gameId)
        {
            DBservices db = new DBservices();
            int ans = db.AddGameToUser(this.Id, gameId);

            return ans > 0;
        }

        public List<Game> GetMyGames()
        {
            DBservices db = new DBservices();
            return db.GetUserGames(this.Id);
        }

        public bool DeleteGameFromMe(int gameId)
        {
            DBservices db = new DBservices();

            int ans = db.DeleteGameFromUser(this.Id, gameId);

            return ans > 0;
        }

        public List<Game> GetRecommendedGames(int userId)
        {
            DBservices db = new DBservices();
            return db.GetRecommendedGames(userId);
        }
    }
}
