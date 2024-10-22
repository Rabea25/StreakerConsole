using System.Text.Json;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
namespace StreakerConsole
{


    internal class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            Session session = IO.LoadSession();
            IO.session = session;
            UserInteraction ui = new UserInteraction(session);
            ui.GreetUser();
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
