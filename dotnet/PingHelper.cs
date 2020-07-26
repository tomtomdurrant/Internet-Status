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
        private static Random _random;
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

            var randomNumber = _random.Next(0, Constants.IpAddresses.Count);

            pingSender.SendAsync(Constants.IpAddresses.Values.ElementAt(randomNumber), 120, null);
        }

        private static void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            var ip = e.Reply.Address.ToString();
            var name = Constants.IpAddresses.FirstOrDefault(x => x.Value == ip).Key;

            Console.WriteLine($"Pinging {name} - {ip}");
            CompletedPing = DateTime.Now;

            var timeTaken = CompletedPing - StartedPing;

            if (e.Cancelled)
            {
                Console.WriteLine("Ping cancelled");
            }
            else if (e.Error != null || e.Reply.Address.ToString() == "0.0.0.0" || e.Reply.Address.ToString() == "192.168.0.1")
            {
                var content = $"{DateTime.Now} - Internet down";
                Console.WriteLine($"It failed at {CompletedPing}");
                WriterHelper(Constants.InternetStatsFile, content);
                ListOfStatus.Add(content);
                ListOfInternetUp.Add(false);
            }
            else
            {
                var content = $"{DateTime.Now} - Internet up";
                Console.WriteLine($"It worked at {CompletedPing} in {timeTaken}ms");
                WriterHelper(Constants.InternetStatsFile, content);
                ListOfStatus.Add(content);
                ListOfInternetUp.Add(true);
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
                var content = $"Internet up from {DateTime.Now - TimeSpan.FromHours(1)} until {DateTime.Now}";
                var path = $"{today}.txt";
                WriterHelper(path, content);
            }

            ListOfStatus.Clear();
            ListOfInternetUp.Clear();
        }

        private static void WriterHelper(string path, string content)
        {
            var folder = Constants.GetTextFolder();
            using var file = new StreamWriter(Path.Combine(folder.FullName, path), true);
            file.WriteLine(content);
        }
    }
}
