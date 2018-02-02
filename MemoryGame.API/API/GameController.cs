using MemoryGame.API.Models.DataManager;
using MemoryGame.API.Models.DB;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MemoryGame.API.API
{
    [EnableCors(origins: "http://localhost:60273", headers: "*", methods: "*")]
    public class GameController : ApiController
    {
        GameManager _gm; 
        public GameController()
        {
            _gm = new GameManager();
        }

        public IEnumerable<ChallengerViewModel> Get()
        {
            return _gm.GetAll;
        }

        [HttpPost]
        public HTTPApiResponse AddPlayer(Challenger user)
        {
            return _gm.AddChallenger(user);
        }

        [HttpPost]
        public void AddScore(Rank user)
        {
            _gm.UpdateCurrentBest(user);
        }

        [HttpPost]
        public HTTPApiResponse DeletePlayer(int id)
        {
            return _gm.DeleteChallenger(id);
        }

        public int GetPlayerID(string email)
        {
            return _gm.GetChallengerID(email);
        }

        public ChallengerViewModel GetPlayerProfile(string email)
        {
            return _gm.GetChallengerByEmail(email);
        }
    }
}
