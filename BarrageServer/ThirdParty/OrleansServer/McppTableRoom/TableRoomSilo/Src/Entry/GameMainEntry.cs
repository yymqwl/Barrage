using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Orleans.Hosting;
using Orleans.Configuration;
using System.Net;
using TableRoom;
using Microsoft.Extensions.Logging;
using Orleans;

namespace TableRoomSilo
{
    public class GameMainEntry : AGameMainEntry
    {

        public static GameMainEntry Instance { get; } = new GameMainEntry();

        protected ISiloHost m_Host;
        protected override void Init()
        {


            RunSilo().Wait();

            
            AssemblyManager.Instance.Add(typeof(GameMainEntry).Assembly);
            GameModuleManager.Instance.CreateModules(typeof(GameMainEntry).Assembly);
            GameModuleManager.Instance.Init();
            
        }


        protected Task RunSilo()
        {
            try
            {
                var builder = new SiloHostBuilder();
                builder.Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = TableRoom.GameConstant.ClusterId;
                    options.ServiceId = TableRoom.GameConstant.ServiceId;
                })
                .Configure<GrainCollectionOptions>(options =>
                {
                    options.CollectionAge = TimeSpan.FromSeconds(61);
                    options.DeactivationTimeout = TimeSpan.FromSeconds(5);
                    options.CollectionQuantum = TimeSpan.FromSeconds(1);
                })
                .ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Error).AddConsole());//LogLevel.Warning
                builder.ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(GChatRoomEntry).Assembly).WithReferences();
                });
                m_Host = builder.Build();
                m_Host.StartAsync().Wait();

            }
            catch(Exception e)
            {
                Log.Debug(e.ToString());
            }
            return Task.CompletedTask;

        }




        protected  override void ShutDown()
        {
            m_Host.StopAsync().Wait();
            GameModuleManager.Instance.ShutDown();
        }
      
    }
}
