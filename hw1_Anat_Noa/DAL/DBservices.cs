using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;


namespace hw1_Anat_Noa.Models
{
    public class DBservices
    {
        public DBservices()
        {

        }
        public SqlConnection connect(String conString)
        {

            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString(conString);
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        public int Insert(Game game)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@name", game.Name);
            paramDic.Add("@steamUrl", game.SteamUrl);
            paramDic.Add("@image", game.Image);
            paramDic.Add("@releaseDate", game.ReleaseDate);
            paramDic.Add("@reviewSummary", game.ReviewSummary);
            paramDic.Add("@price", game.Price);
            paramDic.Add("@windows", game.Windows);
            paramDic.Add("@mac", game.Mac);
            paramDic.Add("@linux", game.Linux);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_INSERT_GAME", con, paramDic);

            try
            {
                int newGameId = Convert.ToInt32(cmd.ExecuteScalar());

                foreach (string tag in game.Tags)
                {
                    InsertGameTag(newGameId, tag);
                }

                return newGameId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------
        // This method Reads all students from the students table 
        //--------------------------------------------------------------------------------------------------
        public List<Game> Read()
        {

            SqlConnection con;
            SqlCommand cmd;
            List<Game> games = new List<Game>();

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateCommandWithStoredProcedureGeneral("SP_READ_GAMES", con, null);          // create the command

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


            try
            {

                while (dataReader.Read())
                {
                    Game g = new Game();
                    g.Id = Convert.ToInt32(dataReader["Id"]);
                    g.Name = dataReader["Name"].ToString();
                    g.SteamUrl = dataReader["SteamUrl"].ToString();
                    g.Image = dataReader["Image"].ToString();
                    g.ReleaseDate = dataReader["ReleaseDate"].ToString();
                    g.ReviewSummary = dataReader["ReviewSummary"].ToString();
                    g.Price = Convert.ToInt32(dataReader["Price"]);
                    g.Windows = Convert.ToBoolean(dataReader["Windows"]);
                    g.Mac = Convert.ToBoolean(dataReader["Mac"]);
                    g.Linux = Convert.ToBoolean(dataReader["Linux"]);
                    games.Add(g);
                }

                return games;


            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }



            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }



        //insertUser
        public int InsertUser(User user)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@name", user.Name);
            paramDic.Add("@email", user.Email);
            paramDic.Add("@password", user.Password);
            

            cmd = CreateCommandWithStoredProcedureGeneral("SP_INSERT_USER", con, paramDic);

            try
            {
                int numEffected = cmd.ExecuteNonQuery();
                return numEffected;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }


        //read all users from UsersTable
        public List<User> ReadUsers()
        {

            SqlConnection con;
            SqlCommand cmd;
            List<User> users = new List<User>();

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateCommandWithStoredProcedureGeneral("SP_READ_USERS", con, null);          // create the command

            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);


            try
            {

                while (dataReader.Read())
                {
                    User u = new User();
                    u.Id = Convert.ToInt32(dataReader["Id"]);
                    u.Name = dataReader["Name"].ToString();
                    u.Email = dataReader["Email"].ToString();
                    u.Password = dataReader["Password"].ToString();
                    u.Active = Convert.ToBoolean(dataReader["Active"]);
                    users.Add(u);
                }

                return users;


            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }



            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        ///login for user
        public User LoginUser(string email, string password)
        {
            SqlConnection con = connect("myProjDB");

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@email", email);
            paramDic.Add("@password", password);

            SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_LOGIN_USER", con, paramDic);
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            try
            {
                if (dataReader.Read())
                {
                    User u = new User();

                    u.Id = Convert.ToInt32(dataReader["Id"]);
                    u.Name = dataReader["Name"].ToString();
                    u.Email = dataReader["Email"].ToString();
                    u.Password = dataReader["Password"].ToString();
                    u.Active = Convert.ToBoolean(dataReader["Active"]);

                    return u;
                }

                return null;
            }
            finally
            {
                con.Close();
            }
        }


        //DELETE GAME FROM GAMES
        public int DeleteGame(int id)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@id", id);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_DELETE_GAME", con, paramDic);

            try
            {
                cmd.ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }



        //delete user from UsersTable by ID
        public int DeleteUser(int id)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            {
                throw ex;
            }


            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            paramDic.Add("@id", id);


            cmd = CreateCommandWithStoredProcedureGeneral("SP_DELETE_USER", con, paramDic);


            try
            {
                cmd.ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }



        //UPDATE A GAME
        public int UpdateGame(int id, Game game)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            paramDic.Add("@id", id);
            paramDic.Add("@name", game.Name);
            paramDic.Add("@steamUrl", game.SteamUrl);
            paramDic.Add("@image", game.Image);
            paramDic.Add("@releaseDate", game.ReleaseDate);
            paramDic.Add("@reviewSummary", game.ReviewSummary);
            paramDic.Add("@price", game.Price);
            paramDic.Add("@windows", game.Windows);
            paramDic.Add("@mac", game.Mac);
            paramDic.Add("@linux", game.Linux);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_UPDATE_GAME", con, paramDic);

            try
            {
                cmd.ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        //FIND GAME BY NAME
        public List<Game> GetGameByName(string name)
        {
            SqlConnection con;
            SqlCommand cmd;

            List<Game> games = new List<Game>();

            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            {
                throw ex;
            }


            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            paramDic.Add("@name", name);


            cmd = CreateCommandWithStoredProcedureGeneral(
                "SP_GET_GAME_BY_NAME",
                con,
                paramDic);


            SqlDataReader dataReader =
                cmd.ExecuteReader(CommandBehavior.CloseConnection);


            try
            {
                while (dataReader.Read())
                {
                    Game g = new Game();

                    g.Id = Convert.ToInt32(dataReader["Id"]);
                    g.Name = dataReader["Name"].ToString();
                    g.SteamUrl = dataReader["SteamUrl"].ToString();
                    g.Image = dataReader["Image"].ToString();
                    g.ReleaseDate = dataReader["ReleaseDate"].ToString();
                    g.ReviewSummary = dataReader["ReviewSummary"].ToString();
                    g.Price = Convert.ToInt32(dataReader["Price"]);

                    g.Windows =
                        Convert.ToBoolean(dataReader["Windows"]);

                    g.Mac =
                        Convert.ToBoolean(dataReader["Mac"]);

                    g.Linux =
                        Convert.ToBoolean(dataReader["Linux"]);


                    games.Add(g);
                }

                return games;
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }


        //UPDAT E USER
        public int UpdateUser(int id, User user)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@id", id);
            paramDic.Add("@name", user.Name);
            paramDic.Add("@password", user.Password);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_UPDATE_USER", con, paramDic);

            try
            {
                cmd.ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        


        //insert tags to TagsTAble
        public int InsertGameTag(int gameId, string tagName)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@gameId", gameId);
            paramDic.Add("@tagName", tagName);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_INSERT_GAME_TAG", con, paramDic);

            try
            {
                cmd.ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        //ADD ROW TO CONNECTION TABLE
        public int AddGameToUser(int userId, int gameId)
        {
            SqlConnection con = connect("myProjDB");

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@userId", userId);
            paramDic.Add("@gameId", gameId);

            SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_ADD_GAME_TO_USER", con, paramDic);

            try
            {
                cmd.ExecuteNonQuery();
                return 1;
            }
            finally
            {
                con.Close();
            }
        }

        //select * from connection table
        public List<Game> GetUserGames(int userId)
        {
            SqlConnection con = connect("myProjDB");
            List<Game> games = new List<Game>();

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@userId", userId);

            SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_GET_USER_GAMES", con, paramDic);
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            try
            {
                while (dataReader.Read())
                {
                    Game g = new Game();

                    g.Id = Convert.ToInt32(dataReader["Id"]);
                    g.Name = dataReader["Name"].ToString();
                    g.SteamUrl = dataReader["SteamUrl"].ToString();
                    g.Image = dataReader["Image"].ToString();
                    g.ReleaseDate = dataReader["ReleaseDate"].ToString();
                    g.ReviewSummary = dataReader["ReviewSummary"].ToString();
                    g.Price = Convert.ToInt32(dataReader["Price"]);
                    g.Windows = Convert.ToBoolean(dataReader["Windows"]);
                    g.Mac = Convert.ToBoolean(dataReader["Mac"]);
                    g.Linux = Convert.ToBoolean(dataReader["Linux"]);

                    games.Add(g);
                }

                return games;
            }
            finally
            {
                con.Close();
            }
        }
        //DELETE GAME FROM CONNECTION TABLE WITH USER
        public int DeleteGameFromUser(int userId, int gameId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@userId", userId);
            paramDic.Add("@gameId", gameId);

            cmd = CreateCommandWithStoredProcedureGeneral("SP_DELETE_GAME_FROM_USER", con, paramDic);

            try
            {
                cmd.ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        //read all tags
        public List<string> GetAllTags()
        {
            SqlConnection con = connect("myProjDB");
            SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_GET_ALL_TAGS", con, null);

            List<string> tags = new List<string>();
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            try
            {
                while (dataReader.Read())
                {
                    tags.Add(dataReader["tagName"].ToString());
                }

                return tags;
            }
            finally
            {
                con.Close();
            }
        }

        //find a game by tags
        public List<Game> GetGamesByTags(string tags)
        {
            SqlConnection con = connect("myProjDB");

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@tags", tags);

            SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_GET_GAMES_BY_TAGS", con, paramDic);
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<Game> games = new List<Game>();

            try
            {
                while (dataReader.Read())
                {
                    Game g = new Game();

                    g.Id = Convert.ToInt32(dataReader["Id"]);
                    g.Name = dataReader["Name"].ToString();
                    g.SteamUrl = dataReader["SteamUrl"].ToString();
                    g.Image = dataReader["Image"].ToString();
                    g.ReleaseDate = dataReader["ReleaseDate"].ToString();
                    g.ReviewSummary = dataReader["ReviewSummary"].ToString();
                    g.Price = Convert.ToInt32(dataReader["Price"]);
                    g.Windows = Convert.ToBoolean(dataReader["Windows"]);
                    g.Mac = Convert.ToBoolean(dataReader["Mac"]);
                    g.Linux = Convert.ToBoolean(dataReader["Linux"]);

                    games.Add(g);
                }

                return games;
            }
            finally
            {
                con.Close();
            }
        }


        //RECOMENDATIONS BY TAGS
        public List<Game> GetRecommendedGames(int userId)
        {
            SqlConnection con = connect("myProjDB");

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@userId", userId);

            SqlCommand cmd = CreateCommandWithStoredProcedureGeneral("SP_GET_RECOMMENDED_GAMES", con, paramDic);
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<Game> games = new List<Game>();

            try
            {
                while (dataReader.Read())
                {
                    Game g = new Game();

                    g.Id = Convert.ToInt32(dataReader["Id"]);
                    g.Name = dataReader["Name"].ToString();
                    g.SteamUrl = dataReader["SteamUrl"].ToString();
                    g.Image = dataReader["Image"].ToString();
                    g.ReleaseDate = dataReader["ReleaseDate"].ToString();
                    g.ReviewSummary = dataReader["ReviewSummary"].ToString();
                    g.Price = Convert.ToInt32(dataReader["Price"]);
                    g.Windows = Convert.ToBoolean(dataReader["Windows"]);
                    g.Mac = Convert.ToBoolean(dataReader["Mac"]);
                    g.Linux = Convert.ToBoolean(dataReader["Linux"]);

                    if (dataReader["Tags"] != DBNull.Value && dataReader["Tags"].ToString() != "")
                    {
                        g.Tags = dataReader["Tags"].ToString().Split(',').ToList();
                    }
                    else
                    {
                        g.Tags = new List<string>();
                    }

                    games.Add(g);
                }

                return games;
            }
            finally
            {
                con.Close();
            }
        }







        //---------------------------------------------------------------------------------
        // Create the SqlCommand
        //---------------------------------------------------------------------------------
        private SqlCommand CreateCommandWithStoredProcedureGeneral(String spName, SqlConnection con, Dictionary<string, object> paramDic)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            if (paramDic != null)
                foreach (KeyValuePair<string, object> param in paramDic)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);

                }


            return cmd;
        }
    }
}
