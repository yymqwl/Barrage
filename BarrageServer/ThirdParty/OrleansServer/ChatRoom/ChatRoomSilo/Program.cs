using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System.Net;
using GHall;
using IHall;
using Orleans.Runtime;

namespace ChatRoom
{
    class Program
    {
        static  void Main(string[] args)
        {
            /*
            return new HostBuilder()
                .UseOrleans(builder =>
                {
                    builder
                     .UseLocalhostClustering()
                     .Configure<ClusterOptions>(options =>
                     {
                         options.ClusterId = GameConstant.ClusterId;
                         options.ServiceId = GameConstant.ServiceId;
                     })
                     .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                     .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(GChatRoomEntry).Assembly).WithReferences());
                }).RunConsoleAsync();
            */
            RunMainAsync().Wait();
       


        }

        private static async Task RunMainAsync()
        {
            try
            {
                var host = await InitialiseSilo();


                var client = await InitialiseClient();


                /*
                var IME0 = client.GetGrain<IHall.IChatRoomEntry>(0);
                await IME0.Init();
                await IME0.SayHello();
                */

                var gs = client.GetGrain<IHall.ISession>(0);
                var gsobs = new GateSession();
                var gsobs_ref = await client.CreateObjectReference<ISessionObserver>(gsobs);
               
                bool bloop = true;
                while (bloop)
                {
                    string str_line = Console.ReadLine();
                    if (str_line == "exit")
                    {
                        bloop = false;
                    }
                    if(str_line == "add")
                    {
                        await gs.SetISessionObserver(await client.CreateObjectReference<ISessionObserver>(new GateSession()));
                    }
                    if (str_line == "say")
                    {
                        await  gs.SayHello();
                    }
                    if (str_line == "remove")
                    {
                        await gs.SetISessionObserver(null);
                    }


                }
                await client.AbortAsync();
                await host.StopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static async Task<ISiloHost> InitialiseSilo()
        {
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = GameConstant.ClusterId;
                    options.ServiceId = GameConstant.ServiceId;
                })
                .Configure<GrainCollectionOptions>(options =>
                {
                    options.CollectionAge = TimeSpan.FromSeconds(61);
                    options.DeactivationTimeout = TimeSpan.FromSeconds(5);
                    options.CollectionQuantum = TimeSpan.FromSeconds(1);
                })
                .ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning).AddConsole());
            builder.ConfigureApplicationParts(parts =>
            {
                parts.AddApplicationPart(typeof(GChatRoomEntry).Assembly).WithReferences();
            });
            var host = builder.Build();
            await host.StartAsync();



            return host;
        }


        //模拟网关
        private static async Task<IClusterClient> InitialiseClient()
        {

            var client = new ClientBuilder()
                    .UseLocalhostClustering()
                    .ConfigureApplicationParts(parts => {
                        parts.AddApplicationPart(typeof(IHall.IChatRoomEntry).Assembly);
                    })
                    .ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning).AddConsole())
                    .Build();
            await client.Connect();

            return client;
        }
    }
}


public class GateSession : IHall.ISessionObserver
{
    public void ReceiveMessage(string msg)
    {
        Console.WriteLine($"GateSession {msg}");
    }
}
