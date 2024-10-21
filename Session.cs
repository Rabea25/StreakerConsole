using System.Text.Json.Serialization;
using System.Text.Json;

namespace StreakerConsole
{
    
    internal class Session
    {

        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public List<Streak> Streaks { get; set; }

        public Session() { }
        public Session(string jsonStr)
        {
            var sesh = JsonSerializer.Deserialize<Session>(jsonStr);
            UserId = sesh.UserId;
            Username = sesh.Username;
            Token = sesh.Token;
            Streaks = sesh.Streaks;
            foreach(var s in Streaks)
            {
                s.UpdateRunningStreak();
            }
        }

    }
}