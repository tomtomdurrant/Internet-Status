using System;
using System.Timers;

namespace Internet_Status
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ping a big site to test internet");
            var pingHelper = new PingHelper();
            var pingTimer = SetUpTimer(PingHelper.DoThePing, 15000);
            var summaryTimer = SetUpTimer(PingHelper.PrintToTxt, Constants.OneHour);

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
