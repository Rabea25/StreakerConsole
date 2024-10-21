namespace StreakerConsole
{
    public class Streak
    {
        public string Habit { get; set; }
        public List<string> StartDays { get; set; }
        public List<string> EndDays { get; set; }
        public int StreaksCount { get; set; }
        public int LongestStreak { get; set; }
        public bool RunningStreak { get; private set; }


        public void UpdateRunningStreak()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string yesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            string lastEndDay = EndDays[^1]; // last item in EndDays
            RunningStreak = (lastEndDay == today || lastEndDay == yesterday);
        }

        public void AddStreakDay()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            if (RunningStreak && EndDays[^1] != today)
            {
                EndDays[^1] = today;
                LongestStreak = Math.Max(LongestStreak, CalculateStreakLength(StartDays[^1], EndDays[^1]));
            }
        }
        private int CalculateStreakLength(string startDate, string endDate)
        {
            DateTime start = DateTime.Parse(startDate);
            DateTime end = DateTime.Parse(endDate);

            return (end - start).Days + 1; // Adding 1 to include both the start and end days
        }
    }
}