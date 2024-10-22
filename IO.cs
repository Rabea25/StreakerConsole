using MySql.Data.MySqlClient;
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
        private static string token = Environment.GetEnvironmentVariable("TOKEN"); //implement later loading token from env or from credentials or whatever
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
        internal static Session LoadSession()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT JSON_OBJECT('UserId', user_id, 'Username', username, 'Token', token, 'Streaks', streaks, 'IDX', IDX, 'StreakIndices', streak_indices) AS session_json FROM sessions WHERE token = @token";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@token", token);
                        // TODO: handle when token is new to db and no session is found
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return JsonSerializer.Deserialize<Session>(reader.GetString("session_json")) ?? new Session();
                            }
                            else
                            {
                                Console.WriteLine("Connection to DB failed, loading cached local session with read-only permissions.");
                                readOnly = true;
                                //check if session file and directory exist, if not create them
                                return LoadOfflineSession();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection to DB failed, loading cached local session with read-only permissions.");
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
    }
}



