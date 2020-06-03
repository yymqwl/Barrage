using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Runtime;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TableRoom;

namespace TableSiloClient
{
    //测试使用
    class Program
    {

        private const int InitializeAttemptsBeforeFailing = 5;
        private static int attempt = 0;
        static void Main(string[] args)
        {
            Console.Title = "Client";

            RunMainAsync1().Wait();
            //Hello1().Wait();
            //DoAsync().Wait();
        }

         protected static async Task DoAsync()
         {
            await Task.Run(() => { Console.WriteLine("DoAsync"); });
         }


        private static async Task RunMainAsync1()
        {
            using (var client = await InitialiseClient())
            {
                Console.WriteLine("Silo started successfully");
                bool bloop = true;
                var grentry = client.GetGrain<IMainEntry>(0);
                var inue = await grentry.GetINetUserEntry();

                await grentry.Init();
                while (bloop)
                {
                    Thread.Sleep(20);
                    await grentry.Update(0);
                }
                await grentry.ShutDown();
                Console.WriteLine($"开始执行....");
                Console.WriteLine($"执行结束");
            }
        }

            private static async Task RunMainAsync()
        {
            try
            {
                using (var client = await InitialiseClient())
                {
                    Console.WriteLine("Silo started successfully");
                    bool bloop = true;
                    var grentry = client.GetGrain<IChatRoomEntry>(0);
                    string Id = "zsy";

                    NetUserObser nuobser_del = new NetUserObser();

                    //INetUserObserver nuobs;
                    while (bloop)
                    {
                        string str_line = Console.ReadLine();
                        if (str_line == "/exit")
                        {
                            bloop = false;
                        }
                        if (str_line.StartsWith("/join"))
                        {
                            var cu_data =new ChatUser_Data();
                            cu_data.Id = str_line.Replace("/join", "").Trim();
                            Id = cu_data.Id;
                            cu_data.Name = cu_data.Id;
                            var iret = await grentry.Join(cu_data);

                            var nu = client.GetGrain<IChatUser>(cu_data.Id);
                            var nuobs = await client.CreateObjectReference<INetUserObserver>(nuobser_del);
                            //await nu.SetObserver(nuobs);
                            Console.WriteLine(iret);
                            //var me = client.GetGrain<IMainEntry>(0);
                        }
                        if (str_line.StartsWith("/tjoin"))
                        {
                            
                           var nu  = client.GetGrain<INetUser>(Id, typeof(NetUserGrain).FullName);
                           await nu.SendMessage(Encoding.UTF8.GetBytes("测试以下"));
                        }
                        if (str_line.StartsWith("/exit"))
                        {
                            var cu_data = new ChatUser_Data();
                            cu_data.Id = str_line.Replace("/exit", "").Trim();
                            var iret = await grentry.Exit(cu_data.Id);
                            Console.WriteLine(iret);
                            //var me = client.GetGrain<IMainEntry>(0);
                            /*var me = client.GetGrain<IMySql.IMysqlEntry>(0);

                            await me.GetName();
                            */
                        }
                        if (str_line.StartsWith("/msg"))
                        {
                            str_line =  str_line.Replace("/msg", "").Trim();
                            await grentry.SendMessage(Id, Encoding.UTF8.GetBytes(str_line));

                            continue;
                        }
                        if (str_line == "/rg")
                        {

                            continue;
                        }
                        if (str_line.StartsWith("/j"))
                        {
                            /*
                            long id = long.Parse(str_line.Replace("/j", "").Trim());

                            var me = client.GetGrain<IMainEntry>(0);
                            IUser iuser = await me.Join(id);
                            await iuser.Say($"{id}:Hello Eventy");
                            */
                            continue;
                        }
                        if (str_line == "/stream")
                        {
                            /*
                            if (m_stream == null)
                            {
                                var me = client.GetGrain<IMainEntry>(0);
                                var guid = await me.Enter();
                                var provider = client.GetStreamProvider(HallGrains.GameConstant.HallStreamProvider);
                                m_stream = provider.GetStream<string>(guid, HallGrains.GameConstant.HallStreamInput);
                            }
                            await m_stream.OnNextAsync("Come From Client");
                            */
                            continue;
                        }
                        if (str_line.StartsWith("/s"))
                        {
                            /*
                            string msg = str_line.Replace("/s", "").Trim();
                            var me = client.GetGrain<IMainEntry>(0);
                            await me.SendMsg(msg);
                            */
                            continue;
                        }

                        if (str_line.StartsWith("/gc"))
                        {
                            System.GC.Collect();
                            continue;
                        }
                    }
                    Console.WriteLine($"开始执行....");
                    Console.WriteLine($"执行结束");
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
                    .UseLocalhostClustering()
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = "dev";
                        options.ServiceId = "ChatRoomSlio";
                    })
                    .ConfigureApplicationParts(parts =>
                    {
                        parts.AddApplicationPart(typeof(IChatRoomEntry).Assembly);
                    }
                    )
                    .ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning).AddConsole())
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
    public class NetUserObser : INetUserObserver
    {
        public void Receive(byte[] msg)
        {
            Console.WriteLine(Encoding.UTF8.GetString(msg));
        }
    }
}
