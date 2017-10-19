using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace D.Net_tcp_repro
{
    class Program
    {
        static void Main(string[] args)
        {
            Client.RunAndWait().GetAwaiter().GetResult();
        }
    }

    class Client
    {
        private DiscordSocketClient DiscordClient;
        private string DiscordToken = "Mjc4ODM0MDYwMDUzNDQ2NjY2.DMeFRw.OdFatYyho7eHZH6yaCftRsnpl-U";

        public static async Task RunAndWait()
        {
            Client client = new Client();

            while( true )
            {
                try
                {
                    await client.Connect();
                    await Task.Delay(-1);
                }
                catch(Exception e)
                {
                    PrintException(e);
                }
            }
        }

        public async Task Connect()
        {
            DiscordSocketConfig config = new DiscordSocketConfig();
            config.ShardId = 0;
            config.TotalShards = 1;
            config.LogLevel = LogSeverity.Debug;
            config.DefaultRetryMode = RetryMode.Retry502 & RetryMode.RetryRatelimit & RetryMode.RetryTimeouts;
            config.AlwaysDownloadUsers = true;
            config.LargeThreshold = 100;
            config.HandlerTimeout = null;
            config.MessageCacheSize = 100;
            config.ConnectionTimeout = int.MaxValue;

            this.DiscordClient = new DiscordSocketClient(config);

            this.DiscordClient.Connected += OnConnected;
            this.DiscordClient.Ready += OnReady;
            this.DiscordClient.Disconnected += OnDisconnected;

            Console.WriteLine("LoginAsync().");
            await this.DiscordClient.LoginAsync(TokenType.Bot, this.DiscordToken);

            Console.WriteLine("StartAsync().");
            await this.DiscordClient.StartAsync();
        }

        private Task OnDisconnected(Exception arg)
        {
            Console.WriteLine("Disconnected.");
            PrintException(arg);
            return Task.CompletedTask;
        }

        private Task OnReady()
        {
            Console.WriteLine("Ready.");
            return Task.CompletedTask;
        }

        private Task OnConnected()
        {
            Console.WriteLine("Connected.");
            return Task.CompletedTask;
        }

        private static void PrintException(Exception e)
        {
            Console.WriteLine("Exception message: " + e.Message);
            Console.WriteLine("Exception stack:\n" + e.Message);

            Exception inner = e;
            while( (inner = inner.InnerException) != null )
            {
                Console.WriteLine("InnerException message: " + e.InnerException.Message);
                Console.WriteLine("InnerException stack:\n" + e.InnerException.Message);
            }
        }
    }
}
