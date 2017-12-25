using System;
using Hangfire;
using Hangfire.MemoryStorage;

namespace hangfireDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalConfiguration.Configuration
            //    .UseColouredConsoleLogProvider()
                .UseMemoryStorage();        //使用内存保存计划任务

            //虽然下面这个server没有使用，但是没这句任务就不会运行，没有研究具体原因。
            var server = new BackgroundJobServer();
            var id = BackgroundJob.Enqueue(() => Console.WriteLine($"{DateTime.Now} Begin"));


            // ┌───────────── minute (0 - 59)
            // │ ┌───────────── hour (0 - 23)
            // │ │ ┌───────────── day of month (1 - 31)
            // │ │ │ ┌───────────── month (1 - 12)
            // │ │ │ │ ┌───────────── day of week (0 - 6) (Sunday to Saturday;
            // │ │ │ │ │              7 is also Sunday on some systems)
            // │ │ │ │ │
            // │ │ │ │ │
            // * * * * * 
            Run("1/3 * * * *");
            Run("*/3 * * * *");

            Console.ReadLine();
        }
        private static void Run(string cron)
        {
            RecurringJob.AddOrUpdate(cron, () => PrintCron(cron), cron);
        }

        public static void PrintCron(string cron)
        {
            Console.WriteLine($"{DateTime.Now} Job:{cron}");
        }
    }
}
