using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using System.IO;
using System.Text;
using System.Collections.Generic;

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
            //var list = new List<byte[]>();

            //for (int i = 0; i < 5; i++)
            //{
            //    list.Add(Enumerable.Repeat((byte)0x20, 1024 * 1024 * 1024).ToArray());
            //}

            var chatUrl = "http://192.168.31.125:5002/chat";
            var stockUrl = "ws://192.168.31.125:5002/stock";
            //stockUrl = "ws://localhost:5001/chat?access_token=eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE1MTgxNTI1NjgsImV4cCI6MTU0OTY4ODU2OCwiaXNzIjoiSGljb2luIiwiYXVkIjpbIkhpY29pbi9yZXNvdXJjZXMiLCJIaWNvaW4iXSwiY2xpZW50X2lkIjoicm8uY2xpZW50Iiwic3ViIjoiMjI5MDY1NzgzNTMwMDQ1NDQiLCJhdXRoX3RpbWUiOjE1MTgxNTI1NjgsImlkcCI6ImxvY2FsIiwiZW1haWwiOiIxMjM0Mzk4ODRAcXEuY29tIiwidXNlcm5hbWUiOiJDYXNwZXIiLCJyb2xlIjoidXNlciIsInNjb3BlIjpbIkhpY29pbiIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJwYXNzd29yZCJdfQ.LoKWy58X6hIuBBMbyuygSnEPqxuRuNc_H0MvlAAaSnr69ui7QIwG7TTuEUFZpLVmhogQZukn6zmG3NShl0eUZgxxeAxeCTec0euPljLGXaKjvTfe4czE3M7-ie0LEW1Aoe8LZ34uBnVzZBcIywfBpYCcbkE-ElD7X-EoCAkBNFR6at61WtPolljUGwDHbdWX994NUxt01rhqych_yAOrGkieiS-Nb_VTkyqwhwUhzl16ijuwL4zLHo2V-ZBjTitgtAUcbQgMCynMHVPGIbLoQnlOQlDPbHcksdGFynpCR9SbDN9qRrv8NUEmR9CNhbfzhpHXKbsfxERsHpP9UIDe-A";

            //Parallel.For(0, 1000, async i =>
            //{
            //    await ExecuteHubAsync(connectUrl);
            //});

            for (int i = 0; i < 100; i++)
            {
                //ExecuteHubAsync(chatUrl).Wait();

                Parallel.For(0, 200, async j =>
                {
                    //await ExecuteHubAsync(chatUrl);
                    try
                    {
                        await ExecuteWebSocketAsync(stockUrl);
                    }
                    catch
                    {
                    }
                });

                //ExecuteHubAsync(chatUrl).Wait();
                //ExecuteWebSocketAsync(stockUrl).Wait();
            }

            Console.ReadKey();
        }
    }
}

public class City
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Url { get; set; }
}

public class Province
{
    public List<City> Cities { get; set; }

    public long Id { get; set; }

    public string Name { get; set; }
}