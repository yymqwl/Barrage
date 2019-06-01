using Orleans;
using Orleans.Configuration;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Orleans.Hosting;
using IHall;
using Orleans.Streams;

namespace ClientApp
{
    class Program
    {
        private const string ClusterId = "dev";
        private const string ServiceId = "OrleansSample";

        private const string Invariant = "MySql.Data.MySqlClient";
        private const string ConnectionString = "server=localhost;port=3306;database=orleans;user id=root;password=25689328;SslMode=none;";

        private const int InitializeAttemptsBeforeFailing = 5;
        private static int attempt = 0;
        static void Main(string[] args)
        {
            Console.Title = "Client";

            RunMainAsync().Wait();

            //Console.ReadKey();
        }



        private static async Task RunMainAsync()
        {
            try
            {
                using (var client = await InitialiseClient())
                {
                    Console.WriteLine("Silo started successfully");
                    bool bloop = true;
                    MainEntryView m_mev;
                    IAsyncStream<string> m_stream = null;
                    while (bloop)
                    {
                        string str_line = Console.ReadLine();
                        if(str_line=="/exit")
                        {
                            bloop = false;
                        }
                        if(str_line=="/enter")
                        {
                            var me = client.GetGrain<IMainEntry>(0);
                        }
                        if (str_line == "/hello")
                        {
                            var me = client.GetGrain<IMainEntry>(0);
                            /*var me = client.GetGrain<IMySql.IMysqlEntry>(0);

                            await me.GetName();
                            */
                            await me.SayHello();
                        }
                        if (str_line.StartsWith("/msg"))
                        {
               
                            /*
                            long id = long.Parse(str_line.Replace("/msg", "").Trim());
                            var me = client.GetGrain<IMainEntry>(id);
                            await me.SendMsg("Testmsg");
                            */
                            continue;
                        }
                        if (str_line == "/rg")
                        {
                            var me = client.GetGrain<IMainEntry>(0);
                            m_mev = new MainEntryView();
                            var obj = await client.CreateObjectReference<IMainEntry_Obs>(m_mev);
                            await me.SubscribeAsync(obj);
                            continue;
                        }
                        if (str_line.StartsWith("/j"))
                        {
                            long id = long.Parse(str_line.Replace("/j", "").Trim());

                            var me = client.GetGrain<IMainEntry>(0);
                            IUser iuser  = await me.Join(id);
                            await iuser.Say($"{id}:Hello Eventy");
                            continue;
                        }
                        if (str_line == "/stream")
                        {
                            if(m_stream == null)
                            {
                                var me = client.GetGrain<IMainEntry>(0);
                                var guid = await me.Enter();
                                var provider = client.GetStreamProvider(HallGrains.GameConstant.HallStreamProvider);
                                m_stream = provider.GetStream<string>(guid, HallGrains.GameConstant.HallStreamInput);
                            }
                            await m_stream.OnNextAsync("Come From Client");
                            continue;
                        }
                        if (str_line.StartsWith("/s"))
                        {
                            string msg = str_line.Replace("/s", "").Trim();
                            var me = client.GetGrain<IMainEntry>(0);
                            await me.SendMsg(msg);
                            continue;
                        }

                        if (str_line.StartsWith("/gc"))
                        {
                            System.GC.Collect();
                            continue;
                        }
                    }
                    //Console.WriteLine($"开始执行....");
                    //await DoClientWork(client);
                    //Console.WriteLine($"执行结束");
                    await client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }

        private static async Task<IClusterClient> InitialiseClient()
        {
            var client = new ClientBuilder()
                     .UseAdoNetClustering(options =>
                     {
                         options.Invariant = Invariant;
                         options.ConnectionString = ConnectionString;
                     })
                    //.UseLocalhostClustering()
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = ClusterId;
                        options.ServiceId = ServiceId;
                    })
                    .ConfigureApplicationParts(parts =>
                        {
                            parts.AddApplicationPart(typeof(IMainEntry).Assembly);
                            
                        }
                    )
                    .ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning).AddConsole())
                    .AddSimpleMessageStreamProvider(HallGrains.GameConstant.HallStreamProvider)
                    .Build();

            await client.Connect(RetryFilter);

            return client;
        }

        private static async Task<bool> RetryFilter(Exception exception)
        {
            if (exception.GetType() != typeof(SiloUnavailableException))
            {
                Console.WriteLine($"Cluster client failed to connect to cluster with unexpected error.  Exception: {exception}");
                return false;
            }
            attempt++;
            Console.WriteLine($"Cluster client attempt {attempt} of {InitializeAttemptsBeforeFailing} failed to connect to cluster.  Exception: {exception}");
            if (attempt > InitializeAttemptsBeforeFailing)
            {
                return false;
            }
            await Task.Delay(TimeSpan.FromSeconds(3));
            return true;
        }

    }
}
