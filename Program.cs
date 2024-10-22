using System.Text.Json;

namespace StreakerConsole
{


    internal class Program
    {
        private static string datapath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "StreakerConsole", "Session.json");

        static void Main(string[] args)
        {
            Session session = LoadSession();
            UserInteraction ui = new UserInteraction(session);
            ui.GreetUser();
            while (true)
            {
                Console.WriteLine(new string('-', 40));
                string xd = string.Empty;
                while (xd == string.Empty) xd = Console.ReadLine() ?? string.Empty;
                xd = xd.ToLower();

                switch (xd)
                {
                    case "exit":
                        break;
                    case "help":
                        ui.Help();
                        break;
                    case "checkin":
                        ui.Add();
                        break;
                    case "list":
                        ui.List();
                        break;
                    case "longest":
                        // TODO: Implement longest streak logic
                        break;
                    case "running":
                        // TODO: Implement running streak logic
                        break;
                    case "new":
                        ui.New();
                        break;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }

                if (xd == "exit")
                {
                    break;
                }
            }
            SaveSession(session);
        }

        private static void CreateDefaultSessionFile()
        {
            //to be synced via a database
            Console.WriteLine("Creating default session file...");
            Session defaultSession = new Session
            {
                UserId = 0,
                Username = "NewUser",
                Token = string.Empty,
                Streaks = new List<Streak>(),
                StreakIndices = new Dictionary<string, int>(),
                Index = 0
            };

            // Serialize default session to JSON
            string defaultJsonStr = JsonSerializer.Serialize(defaultSession, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(datapath, defaultJsonStr);
        }
        private static void SaveSession(Session session)
        {
            string jsonStr = JsonSerializer.Serialize(session, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(jsonStr);
            File.WriteAllText(datapath, jsonStr);
        }
        private static Session LoadSession()
        {
            //check if session file and directory exist, if not create them
            var dir = Path.GetDirectoryName(datapath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (!File.Exists(datapath)) CreateDefaultSessionFile();

            //init session
            string jsonStr = File.ReadAllText(datapath);
            return new Session(jsonStr);
        }

    }
}
