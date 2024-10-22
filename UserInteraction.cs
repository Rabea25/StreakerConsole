namespace StreakerConsole
{
    internal class UserInteraction
    {
        private Session session;
        public UserInteraction(Session session)
        {
            this.session = session;
        }
        public void GreetUser()
        {
            Console.WriteLine("Welcome back, " + session.Username);
            if (session.Streaks.Count == 0)
            {
                Console.WriteLine("You have no streaks yet, create one by entering 'new'");
            }
            else
            {
                Console.WriteLine("Your current streaks are:");
                foreach (var streak in session.Streaks)
                {
                    if (streak.RunningStreak) Console.WriteLine(streak.Habit + ": " + streak.CurrentStreakLength() + " days, last checked in on " + streak.EndDays[^1]);
                }
            }
            Console.WriteLine("for a list of commands, enter 'help'");
            Console.WriteLine("To exit, enter 'exit'");
        }
        public void Help()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("exit: exits the program");
            Console.WriteLine("help: displays this message");
            Console.WriteLine("checkin: adds a day to a streak");
            Console.WriteLine("list: lists all streak entries");
            Console.WriteLine("longest: lists the longest streaks (under construction)");
            Console.WriteLine("running: lists the running streaks (under construction)");
            Console.WriteLine("new: creates a new streak");
        }
        public void Add()
        {
            if (IO.readOnly)
            {
                Console.WriteLine("DB is not connected, showing local data only, please try restarting your application.");
                return;
            }
            if (session.Streaks.Count == 0)
            {
                Console.WriteLine("You have no streaks yet, create one by entering 'new'");
                return;
            }
            Console.WriteLine("Enter the name of the streak you continued today:");
            string habit = Console.ReadLine() ?? string.Empty;
            habit = habit.Trim().ToLower();
            habit = char.ToUpper(habit[0]) + habit.Substring(1);
            if (session.StreakIndices.ContainsKey(habit))
            {
                var streak = session.Streaks[session.StreakIndices[habit]];
                Console.WriteLine(streak.Habit);
                streak.AddStreakDay();
                IO.changes = true;
            }
            else
            {
                Console.WriteLine("Invalid Streak name");
            }
        }
        public void List()
        {
            if (session.Streaks.Count == 0)
            {
                Console.WriteLine("You have no streaks yet, create one by entering 'new'");
                return;
            }
            Console.WriteLine("All Streaks:");
            foreach (Streak s in session.Streaks)
            {
                Console.WriteLine(s.Habit);
            }
        }
        public void New()
        {
            if (IO.readOnly)
            {
                Console.WriteLine("DB is not connected, showing local data only, please try restarting your application.");
                return;
            }
            Console.WriteLine("Enter the name of the new streak:");
            string habit = Console.ReadLine() ?? string.Empty;
            while (habit == string.Empty) habit = Console.ReadLine() ?? string.Empty;
            habit = habit.Trim().ToLower();
            habit = char.ToUpper(habit[0]) + habit.Substring(1);
            session.AddHabit(0, habit, "", "");
            Console.WriteLine("New streak '" + habit + "' created, good luck!");
            IO.changes = true;
        }

    }
}