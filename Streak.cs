namespace StreakerConsole
{
    public class Streak
    {
        public string Habit { get; set; }
        public List<string> StartDays { get; set; }
        public List<string> EndDays { get; set; }
        public int StreaksCount { get; set; }
        public int LongestStreak { get; set; }
        public bool RunningStreak { get; set; }


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
            if (RunningStreak)
            {
                if (EndDays[^1] != today)
                {
                    EndDays[^1] = today;
                    LongestStreak = Math.Max(LongestStreak, CalculateStreakLength(StartDays[^1], EndDays[^1]));
                    Console.WriteLine("Day added to " + Habit + ", current streak length is " + CurrentStreakLength());
                }
                else
                {
                    Console.WriteLine("You already checked in today!");
                }
            }
            else
            {
                Console.WriteLine("Your last check in was at " + EndDays[^1] + ", starting a new streak today, good luck!");
                StartDays.Add(today);
                EndDays.Add(today);
                StreaksCount++;
            }
        }
        private int CalculateStreakLength(string startDate, string endDate)
        {
            DateTime start = DateTime.Parse(startDate);
            DateTime end = DateTime.Parse(endDate);

            return (end - start).Days + 1; // Adding 1 to include both the start and end days
        }
        public int CurrentStreakLength()
        {
            return CalculateStreakLength(StartDays[^1], EndDays[^1]);
        }
    }
}