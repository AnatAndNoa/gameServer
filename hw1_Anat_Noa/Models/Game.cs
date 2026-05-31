using System.Collections.Generic;
namespace hw1_Anat_Noa.Models
{
    public class Game
    {
        int id;
        string name;
        string steamUrl;
        string image;
        string releaseDate;
        string reviewSummary;
        int price;
        List<string> tags;
        bool windows;
        bool mac;
        bool linux;
        

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string SteamUrl { get => steamUrl; set => steamUrl = value; }
        public string Image { get => image; set => image = value; }
        public string ReleaseDate { get => releaseDate; set => releaseDate = value; }
        public string ReviewSummary { get => reviewSummary; set => reviewSummary = value; }
        public int Price { get => price; set => price = value; }
        public List<string> Tags { get => tags; set => tags = value; }
        public bool Windows { get => windows; set => windows = value; }
        public bool Mac { get => mac; set => mac = value; }
        public bool Linux { get => linux; set => linux = value; }


        public Game(int id, string name, string steamUrl, string image, string releaseDate, string reviewSummary, int price, List<string> tags, bool windows, bool mac, bool linux)
        {
            Id = id;
            Name = name;
            SteamUrl = steamUrl;
            Image = image;
            ReleaseDate = releaseDate;
            ReviewSummary = reviewSummary;
            Price = price;
            Tags = tags;
            Windows = windows;
            Mac = mac;
            Linux = linux;

        }
        public Game()
        {
            Tags = new List<string>();

        }

        public int Insert()
        {
            DBservices db = new DBservices();
            return db.Insert(this);
        }

        public List<Game> Read()
        {
            DBservices db = new DBservices();
            return db.Read();
        }


        public List<Game> GetByName(string name)
        {
            DBservices db = new DBservices();

            return db.GetGameByName(name);
        }

        public bool UpdateGame(int id)
        {
            DBservices db = new DBservices();
            int ans = db.UpdateGame(id, this);
            return ans > 0;
        }

        public bool DeleteById(int id)
        {
            DBservices db = new DBservices();
            int ans = db.DeleteGame(id);
            return ans > 0;
        }

        public List<string> GetAllTags()
        {
            DBservices db = new DBservices();
            return db.GetAllTags();
        }

        public List<Game> GetByTags(string tags)
        {
            DBservices db = new DBservices();
            return db.GetGamesByTags(tags);
        }
    }
}
