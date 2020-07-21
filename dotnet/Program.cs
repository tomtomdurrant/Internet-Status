using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Timers;

namespace Internet_Status
{
    class Program
    {
        private const string InternetStatsFile = "internet-stats.txt";
        private const int Period = 120_000;

        static void Main(string[] args)
        {
            System.Console.WriteLine("Ping google to test internet");
            if (!File.Exists(InternetStatsFile))
            {
                FileStream fileStream = File.Create(InternetStatsFile);
            }

            Timer aTimer = new Timer(Period);
            aTimer.Elapsed += PingHelper.DoThePing;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

            Console.ReadLine();
        }

        private static void WriteOutput(bool internetStatus, PingCompletedEventArgs e)
        {
            if (internetStatus)
            {
                System.Console.WriteLine("It worked");
                System.Console.WriteLine($"Address: {e.Reply.Address.ToString()}");
                System.Console.WriteLine($"RoundTrip time: {e.Reply.RoundtripTime}ms");
            }
            else
            {
                System.Console.WriteLine("It didn't work");
                System.Console.WriteLine(e.Error.ToString());
            }
        }
    }
}
