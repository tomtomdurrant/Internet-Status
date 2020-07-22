using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Timers;

namespace Internet_Status
{
    internal class PingHelper
    {
        static Dictionary<string, string> ipAddresses = new Dictionary<string, string>()
        {
            {"Google-A", "8.8.8.8"},
            {"Google-B", "8.8.4.4"},
            {"Cloudflare", "1.1.1.1"},
            {"Virgin Manchester", "82.25.178.109"},
            {"VirginDNS", "194.168.4.100"}
        };

        private static Random _random;

        private const string InternetStatsFile = "internet-stats.txt";
        public static DateTime StartedPing { get; private set; }
        public static DateTime CompletedPing { get; private set; }
        public static List<string> ListOfStatus { get; set; }
        public static List<bool> ListOfInternetUp { get; set; }
        public PingHelper()
        {
            ListOfStatus = new List<string>();
            ListOfInternetUp = new List<bool>();
            _random = new Random();
        }

        internal static void DoThePing(object state, ElapsedEventArgs e)
        {
            var pingSender = new Ping();

            pingSender.PingCompleted += PingCompletedCallback;

            StartedPing = DateTime.Now;

            var number = _random.Next(0, ipAddresses.Count);

            pingSender.SendAsync("8.8.8.8", 120, null);
        }

        private static void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            var ip = e.Reply.Address.ToString();
            var name = ipAddresses.FirstOrDefault(x => x.Value == ip).Key;
            
            Console.WriteLine($"Pinging {name} - {ip}");
            CompletedPing = DateTime.Now;

            var timeTaken = CompletedPing - StartedPing;

            if (e.Cancelled)
            {
                Console.WriteLine("Ping cancelled");
            }
            else if (e.Error != null || e.Reply.Address.ToString() == "0.0.0.0")
            {
                Console.WriteLine($"It failed at {CompletedPing}");
                using var file = new StreamWriter(InternetStatsFile, true);
                var content = $"{DateTime.Now} - Internet down";
                ListOfStatus.Add(content); 
                ListOfInternetUp.Add(false);
                file.WriteLine(content);
            }
            else
            {
                Console.WriteLine($"It worked at {CompletedPing} in {timeTaken}ms");
                using var file = new StreamWriter(InternetStatsFile, true);
                var content = $"{DateTime.Now} - Internet up";
                ListOfStatus.Add(content);
                ListOfInternetUp.Add(true);
                file.WriteLine(content);
            }
        }

        public static void PrintToTxt(object sender, ElapsedEventArgs e)
        {
            var today = $"{DateTime.Now:yyyy-MM-dd}";

            Console.WriteLine($"Printing stats for {today} at {DateTime.Now.TimeOfDay}");

            if (ListOfInternetUp.Contains(false))
            {
                using var file = new StreamWriter($"{today}.txt", true);
                foreach (var status in ListOfStatus)
                {
                    file.WriteLine(status);
                }
            }
            else
            {
                using var file = new StreamWriter($"{today}.txt", true);
                file.WriteLine($"Internet up from {DateTime.Now - TimeSpan.FromHours(1)} until {DateTime.Now}");
            }

            ListOfStatus.Clear();
            ListOfInternetUp.Clear();
        }
    }
}
