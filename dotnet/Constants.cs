using System;
using System.Collections.Generic;
using System.IO;

namespace Internet_Status
{
    public class Constants
    {
        private const int TwoMinutes = 120_000; // 2 minutes
        public const int OneHour = 3_600_000; // 1 hour

        public static readonly Dictionary<string, string> IpAddresses = new Dictionary<string, string>()
        {
            {"Google-A", "8.8.8.8"},
            {"Google-B", "8.8.4.4"},
            {"Cloudflare", "1.1.1.1"},
            {"Virgin Manchester", "82.25.178.109"},
            {"VirginDNS", "194.168.4.100"}
        };

        public const string InternetStatsFile = "internet-stats.txt";

        private static DirectoryInfo RootDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent;

        public static DirectoryInfo GetTextFolder()
        {
            return Directory.CreateDirectory(Path.Combine(RootDir.FullName, "stats"));
        }
    }
}