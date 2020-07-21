using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Timers;

namespace Internet_Status
{
    internal static class PingHelper
    {
        private static string InternetStatsFile = "internet-stats.txt";

        public static DateTime StartedPing { get; private set; }
        public static DateTime CompletedPing { get; private set; }
        public static DateTime EventRaised { get; set; }

        internal static void DoThePing(object state, ElapsedEventArgs e)
        {
            Ping pingSender = new Ping();

            pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

            if (StartedPing == null)
            {
                StartedPing = new DateTime();
            }

            StartedPing = DateTime.Now;

            pingSender.SendAsync("8.8.8.8", 120);

            GC.Collect();
        }

        private static void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            if (CompletedPing == null)
            {
                CompletedPing = new DateTime();
            }

            CompletedPing = DateTime.Now;

            var timeTaken = CompletedPing - StartedPing;

            if (e.Cancelled)
            {
                System.Console.WriteLine("Ping cancelled");
            }
            else if (e.Error != null)
            {
                System.Console.WriteLine($"It failed at {CompletedPing}");
                using (StreamWriter file = new StreamWriter(InternetStatsFile, true))
                {
                    file.WriteLine($"{DateTime.Now} - Internet down");
                }
            }
            else
            {
                System.Console.WriteLine($"It worked at {CompletedPing} in {timeTaken}ms");
                using (StreamWriter file = new StreamWriter(InternetStatsFile, true))
                {
                    file.WriteLine($"{DateTime.Now} - Internet up");
                }

            }
        }
    }
}
