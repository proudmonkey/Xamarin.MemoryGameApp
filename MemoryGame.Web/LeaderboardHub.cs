using Microsoft.AspNet.SignalR;

namespace MemoryGame.Web
{
    public class LeaderboardHub : Hub
    {
        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<LeaderboardHub>();
            context.Clients.All.displayLeaderBoard();
        }
    }
}