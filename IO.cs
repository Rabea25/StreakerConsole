using MySql.Data.MySqlClient;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
namespace StreakerConsole
{
    internal class IO
    {
        static IO() { DotNetEnv.Env.Load(); }
        private static string datapath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "StreakerConsole", "Session.json");
        private static string connectionString = $"Server={Environment.GetEnvironmentVariable("DB_HOST")};" +
                                                 $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                                                 $"User ID={Environment.GetEnvironmentVariable("DB_USER")};" +
                                                 $"Password={Environment.GetEnvironmentVariable("DB_PASS")};";
        internal static string token = Environment.GetEnvironmentVariable("TOKEN"); //implement later loading token from env or from credentials or whatever
        internal static bool changes = false;
        internal static bool readOnly = false;
        internal static Session session;
        internal static void CreateDefaultSessionFile()
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
                IDX = 0
            };

            // Serialize default session to JSON
            string defaultJsonStr = JsonSerializer.Serialize(defaultSession, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(datapath, defaultJsonStr);
        }
        internal static void SaveSession()
        {
            //Save session locally
            string jsonStr = JsonSerializer.Serialize(session, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(datapath, jsonStr);
            if (!changes) return;
            // TODO: update session in the db when there are changes
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // SQL query to update the session for the specific token
                string updateQuery = @"UPDATE sessions
                               SET username = @username, 
                                   streaks = @streaks, 
                                   streak_indices = @streakIndices, 
                                   IDX = @idx 
                               WHERE token = @token";

                using (var command = new MySqlCommand(updateQuery, connection))
                {
                    // Add parameters from the session object to the query
                    command.Parameters.AddWithValue("@username", session.Username);
                    command.Parameters.AddWithValue("@streaks", JsonSerializer.Serialize(session.Streaks));
                    command.Parameters.AddWithValue("@streakIndices", JsonSerializer.Serialize(session.StreakIndices));
                    command.Parameters.AddWithValue("@idx", session.IDX);
                    command.Parameters.AddWithValue("@token", session.Token);  // Use token for row identification

                    // Execute the update
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if the update affected any rows (optional)
                    if (rowsAffected == 0)
                    {
                        Console.WriteLine("No rows updated, something might be wrong!");
                    }
                    else
                    {
                        Console.WriteLine("Session updated successfully.");
                    }
                }
            }
        }
        internal static async Task<Session> LoadSession()
        {
            try
            {
                var xd = await GetData(token);
                if (xd != null) {
                    xd = xd.Replace("\\\"", "\"");
                    xd = xd.Substring(1, xd.Length - 3);
                    Console.WriteLine(xd);
                    return JsonSerializer.Deserialize<Session>(xd) ?? throw new InvalidOperationException("Deserialization returned null");
                }
                else
                {
                    Console.WriteLine("Failed to get data, loading local session.");
                    readOnly = true;
                    return LoadOfflineSession();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                readOnly = true;
                return LoadOfflineSession();
            }
        }
        internal static Session LoadOfflineSession()
        {
            var dir = Path.GetDirectoryName(datapath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (!File.Exists(datapath)) CreateDefaultSessionFile();

            //init session
            string jsonStr = File.ReadAllText(datapath);
            return new Session(jsonStr);
        }

        private static readonly HttpClient client = new HttpClient();
        public static async Task<string> GetData(string token)
        {
            var url = Environment.GetEnvironmentVariable("WEBAPP"); //webapp url, example http://192.168.1.1/verify
            var json = JsonSerializer.Serialize(new { session_token = token });
            HttpResponseMessage response = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}



