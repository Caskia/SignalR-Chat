using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using System.IO;
using System.Text;

namespace SignalR.Chat.Client
{
    internal class Program
    {
        public static async Task<int> ExecuteHubAsync(string baseUrl)
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

                await HubConnectAsync(connection);

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

        public static async Task ExecuteWebSocketAsync(string baseUrl)
        {
            var client = new ClientWebSocket();
            await client.ConnectAsync(new Uri(baseUrl), CancellationToken.None);

            try
            {
                //while (client.State == WebSocketState.Open)
                //{
                //    var buffer = new ArraySegment<byte>(new Byte[1024 * 16]);
                //    string serializedMessage = null;
                //    WebSocketReceiveResult result = null;
                //    using (var ms = new MemoryStream())
                //    {
                //        do
                //        {
                //            result = await client.ReceiveAsync(buffer, CancellationToken.None).ConfigureAwait(false);
                //            ms.Write(buffer.Array, buffer.Offset, result.Count);
                //        }
                //        while (!result.EndOfMessage);

                //        ms.Seek(0, SeekOrigin.Begin);

                //        using (var reader = new StreamReader(ms, Encoding.UTF8))
                //        {
                //            serializedMessage = await reader.ReadToEndAsync().ConfigureAwait(false);
                //        }
                //    }
                //}

                //client.Dispose();
            }
            catch (Exception ex)
            {
                client.Dispose();
                throw ex;
            }
        }

        private static async Task HubConnectAsync(HubConnection connection)
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
            var chatUrl = "http://localhost:5001/chat";
            var stockUrl = "ws://localhost:5001/stock";

            //Parallel.For(0, 1000, async i =>
            //{
            //    await ExecuteAsync(connectUrl);
            //});

            for (int i = 0; i < 200; i++)
            {
                //ExecuteHubAsync(chatUrl).Wait();

                Parallel.For(0, 100, async j =>
                {
                    await ExecuteWebSocketAsync(stockUrl);
                });

                //ExecuteWebSocketAsync(stockUrl).Wait();
            }

            Console.ReadKey();
        }
    }
}