using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace SignalR.Chat.Client
{
    internal class Program
    {
        public static async Task<int> ExecuteAsync(string baseUrl)
        {
            baseUrl = string.IsNullOrEmpty(baseUrl) ? "http://localhost:5000/chat" : baseUrl;

            Console.WriteLine("Connecting to {0}", baseUrl);
            var connection = new HubConnectionBuilder()
                .WithJsonProtocol()
                .WithUrl(baseUrl)
                .WithConsoleLogger(LogLevel.Error)
                .Build();

            try
            {
                connection.Closed += e => Console.WriteLine(e?.Message);
                // Set up handler
                connection.On<string>("Send", Console.WriteLine);

                await ConnectAsync(connection);

                Console.WriteLine("Connected to {0}", baseUrl);

                //while (true)
                //{
                //    await Task.Delay(1);
                //}
            }
            catch (AggregateException aex) when (aex.InnerExceptions.All(e => e is OperationCanceledException))
            {
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                //await connection.DisposeAsync();
            }
            return 0;
        }

        private static async Task ConnectAsync(HubConnection connection)
        {
            // Keep trying to until we can start
            while (true)
            {
                try
                {
                    await connection.StartAsync();
                    return;
                }
                catch (Exception)
                {
                    await Task.Delay(1000);
                }
            }
        }

        private static void Main(string[] args)
        {
            var connectUrl = "http://localhost:62941/chat";

            //Parallel.For(0, 1000, async i =>
            //{
            //    await ExecuteAsync(connectUrl);
            //});

            for (int i = 0; i < 3000; i++)
            {
                ExecuteAsync(connectUrl).Wait();
            }

            Console.ReadKey();
        }
    }
}