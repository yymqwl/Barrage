using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;

namespace ServerApp
{
    class Program
    {
        private const string ClusterId = "dev";
        private const string ServiceId = "OrleansSample";

        private const string Invariant = "MySql.Data.MySqlClient";
        private const string ConnectionString = "server=localhost;port=3306;database=orleans;user id=root;password=25689328;SslMode=none;";
        static void Main(string[] args)
        {
            Console.WriteLine("输入Silo序号:");
            var index = Convert.ToInt32(Console.ReadLine());
            Console.Title = "Silo" + index;

            RunMainAsync(11111 + index, 30000 + index).Wait();

            Console.ReadKey();
        }
        private static async Task RunMainAsync(int siloPort, int gatewayPort)
        {
            try
            {
                var host = await InitialiseSilo(siloPort, gatewayPort);
                Console.WriteLine("Silo started successfully");
                //await RunGataWall();

                bool bloop = true;
                while (bloop)
                {
                    string str_line = Console.ReadLine();
                    if (str_line == "exit")
                    {
                        bloop = false;
                    }
                }
                await host.StopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private static async Task<ISiloHost> InitialiseSilo(int siloPort, int gatewayPort)
        {
            var builder = new SiloHostBuilder()
                .UseAdoNetClustering(options =>
                {
                    options.Invariant = Invariant;
                    options.ConnectionString = ConnectionString;
                })
                .UseAdoNetReminderService(options =>
                {
                    options.Invariant = Invariant;
                    options.ConnectionString = ConnectionString;
                })
                /*
                .AddAdoNetGrainStorage("OrleansStorage", options =>
                {
                    options.Invariant = Invariant;
                    options.ConnectionString = ConnectionString;
                    options.UseJsonFormat = true;
                })*/
                .ConfigureEndpoints(siloPort: siloPort, gatewayPort: gatewayPort)
                //.UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = ClusterId;
                    options.ServiceId = ServiceId;
                })
                .Configure<GrainCollectionOptions>(options =>
                {
                    options.CollectionAge = TimeSpan.FromSeconds(61);
                    options.DeactivationTimeout = TimeSpan.FromSeconds(5);
                    options.CollectionQuantum = TimeSpan.FromSeconds(1);
                })
                //.AddMemoryGrainStorageAsDefault()
                //.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(MainEntryGrain).Assembly).WithReferences())
                .ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning).AddConsole());
                //.ConfigureLogging(log => log.AddConsole())
                //.AddMemoryGrainStorage(HallGrains.GameConstant.HallPubSubStore)
                //.AddSimpleMessageStreamProvider(HallGrains.GameConstant.HallStreamProvider) ;
            
            builder.ConfigureApplicationParts(parts =>
            {
                parts.AddApplicationPart(typeof(HallGrains.MainEntryGrain).Assembly).WithReferences();
                parts.AddApplicationPart(typeof(MysqlGrains.MysqlEntryGrain).Assembly).WithReferences();
            });
            
            /*
            if(gatewayPort == 30000)
            {
                builder.ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(HallGrains.MainEntryGrain).Assembly).WithReferences();
                });
            }
            else
            {
                builder.ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(MysqlGrains.MysqlEntryGrain).Assembly).WithReferences();
                });
            }*/

            var host = builder.Build();
            await host.StartAsync();



            return host;
        }
        public static async Task RunGataWall()
        {
            try
            {
                using (var client = await InitialiseClient())
                {
                    Console.WriteLine("Silo started successfully");

                    //MainEntryView mev = new MainEntryView();
                    var IME0 = client.GetGrain<IHall.IMainEntry >(0);
                    await IME0.Enter();



                    bool bloop = true;
                    DateTime lasttime = DateTime.Now;
                    TimeSpan ts = TimeSpan.FromMilliseconds(1);
                    while (bloop)
                    {
                        await Task.Delay(1);

                        ts = DateTime.Now - lasttime;
                        lasttime = DateTime.Now;

                    }
                    await client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }

        //模拟网关
        private static async Task<IClusterClient> InitialiseClient()
        {

            var client = new ClientBuilder()
                     .UseAdoNetClustering(options =>
                     {
                         options.Invariant = Invariant;
                         options.ConnectionString = ConnectionString;
                     })
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = ClusterId;
                        options.ServiceId = ServiceId;
                    })
                    .ConfigureApplicationParts(parts => {
                        parts.AddApplicationPart(typeof(IHall.IMainEntry).Assembly);
                    })
                    .ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning).AddConsole())
                    .Build();
            await client.Connect();

            return client;
        }

    }
}
