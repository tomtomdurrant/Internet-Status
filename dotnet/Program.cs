using System;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace Internet_Status
{
    class Program
    {
        private const int TwoMinutes = 120_000; // 2 minutes
        private const int OneHour = 3_600_000; // 1 hour

        static void Main(string[] args)
        {
            Console.WriteLine("Ping google to test internet");
            var pingHelper = new PingHelper();
            var pingTimer = SetUpTimer(PingHelper.DoThePing, 500);
            var summaryTimer = SetUpTimer(PingHelper.PrintToTxt, OneHour);

            Console.ReadLine();
        }

        private static Timer SetUpTimer(ElapsedEventHandler onElapsed, double time)
        {
            var aTimer = new Timer(time);
            aTimer.Elapsed += onElapsed;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            return aTimer;
        }
    }
}
