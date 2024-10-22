using System.Text.Json;

namespace StreakerConsole
{

    internal class Session
    {

        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public List<Streak> Streaks { get; set; }
        public int IDX { get; set; }
        public Dictionary<string, int> StreakIndices { get; set; } = new Dictionary<string, int>();
        public Session() { }
        public Session(string jsonStr)
        {
            var sesh = JsonSerializer.Deserialize<Session>(jsonStr);
            UserId = sesh.UserId;
            Username = sesh.Username;
            Token = sesh.Token;
            Streaks = sesh.Streaks;
            StreakIndices = sesh.StreakIndices;
            IDX = sesh.IDX;
            foreach (var s in Streaks) s.UpdateRunningStreak();

        }

        internal void AddHabit(int old, string habit, string d1, string d2)
        {
            if (old == 0)
            {
                d1 = d2 = DateTime.Now.ToString("yyyy-MM-dd");
            }
            Streaks.Add(new Streak { Habit = habit, StartDays = [d1], EndDays = [d2], StreaksCount = 1, LongestStreak = 1, RunningStreak = true });
            StreakIndices.Add(habit, IDX++);
        }
    }
}