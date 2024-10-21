using System;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using static System.Collections.Specialized.BitVector32;

namespace StreakerConsole
{
        

    internal class Program
    {
        private static string jsonStr = File.ReadAllText("C:\\projects\\StreakerConsole\\Session.json");
        static void Main(string[] args)
        {

            Session session = new Session(jsonStr);
            Console.WriteLine("Welcome back, " + session.Username);
            Console.WriteLine("Your current streaks are:");
            foreach (var streak in session.Streaks)
            {
                if(streak.RunningStreak) Console.WriteLine(streak.Habit + ": " + streak.LongestStreak + " days");
            }
            Console.WriteLine("for a list of commands, enter 'help'");
            Console.WriteLine("To exit, enter 'exit'");
            while (true)
            {
                string xd = string.Empty;
                while(xd == string.Empty)
                {
                    xd = Console.ReadLine();
                }
                if (xd == "exit") break;
                if (xd == "help")
                {
                    Console.WriteLine("Commands:");
                    Console.WriteLine("exit: exits the program");
                    Console.WriteLine("help: displays this message");
                    Console.WriteLine("add: adds a day to a streak");
                    Console.WriteLine("list: lists all streaks");
                    Console.WriteLine("longest: lists the longest streaks");
                    Console.WriteLine("running: lists the running streaks");
                    Console.WriteLine("new: creates a new streak");
                }
            }
        }
    }
}
