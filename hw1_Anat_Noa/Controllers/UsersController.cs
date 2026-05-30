using hw1_Anat_Noa.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace hw1_Anat_Noa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            User user = new User();
            return user.Read();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("{userId}/games")]
        public List<Game> GetUserGames(int userId)
        {
            User user = new User();
            user.Id = userId;

            return user.GetMyGames();
        }

        // POST api/<UsersController>
        [HttpPost]
        public int Post([FromBody] User user)
        {
            return user.Register();
        }


        [HttpPost("login")]
        public User Login([FromBody] User user)
        {
            return user.Login();
        }


        [HttpPost("{userId}/games/{gameId}")]
        public bool AddGameToUser(int userId, int gameId)
        {
            User user = new User();
            user.Id = userId;

            return user.AddGameToMe(gameId);
        }


        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody] User user)
        {
            return user.UpdateUser(id);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            User user = new User();

            return user.DeleteById(id);
        }
        //DELETE FROM USERGAME
        [HttpDelete("{userId}/games/{gameId}")]
        public bool DeleteGameFromUser(int userId, int gameId)
        {
            User user = new User();
            user.Id = userId;

            return user.DeleteGameFromMe(gameId);
        }
    }
}
