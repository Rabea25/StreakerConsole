using System.Text.Json;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Net.Http;
using System.Threading.Tasks;
namespace StreakerConsole
{


    internal class Program
    {
        static void Main(string[] args)
        {
            // Call the asynchronous method and wait for it to complete
            Task.Run(async () => await MainAsync(args)).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            Session session = await IO.LoadSession();
            IO.session = session;
            UserInteraction ui = new UserInteraction(session);
            ui.GreetUser();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            while (true)
            {
                Console.WriteLine(new string('-', 40));
                string xd = string.Empty;
                while (xd == string.Empty) xd = Console.ReadLine() ?? string.Empty;
                xd = xd.Trim().ToLower();
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
                if (xd == "exit") break;
            }

            static void OnProcessExit(object sender, EventArgs e)
            {
                Console.WriteLine("Exiting application (window close detected).");
               IO.SaveSession(); 
            }
        }

    }
}
