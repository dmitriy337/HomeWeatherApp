using System;
using System.Collections.Generic;
using Serilog;


namespace HomeWeatherApp
{
    class Program
    {
        public static Serilog.Core.Logger log = new LoggerConfiguration()
                .WriteTo.File("Log.log", rollingInterval: RollingInterval.Month)
                .CreateLogger();

    static void Main(string[] args)
        {
            

            var t = new Service.Handler();
            

            t.onNewRequest += test;
            t.onException += eTest;
                
                
            t.StartPooling(Timeout: 60, url: Config.url);


            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        public static void test(Dictionary<string, string> valuePairs)
        {
            log.Information($"Temperature: {valuePairs.GetValueOrDefault("temp")} Humidity: {valuePairs.GetValueOrDefault("humidity")}");

            using(TestDbForHomeContext testDbForHomeContext = new TestDbForHomeContext())
            {
                testDbForHomeContext.Recordings.Add(
                    new Recording
                    {
                        Date = DateTime.Now,
                        Temp = double.Parse(valuePairs.GetValueOrDefault("temp")),
                        Hum = double.Parse(valuePairs.GetValueOrDefault("humidity")),
                    }) ;
                testDbForHomeContext.SaveChanges();
            }
        }


        public static void eTest(Exception exception)
        {
            log.Error(exception.Message.ToString());
        }
    }
}
