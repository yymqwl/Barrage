using System;
using GameFramework;
using Orleans.Hosting;
using GameMain;
using Orleans.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using Orleans;

namespace BarrageSilo
{
    [GameFrameworkModule]
    public class SiloModule : GameFrameworkModule
    {

        public override int Priority => 9;
        private ISiloHost m_SiloHost;
        public  override bool Init()
        {
            //Console.WriteLine("输入Silo序号:");
            //int index = Convert.ToInt32(Console.ReadLine());
            int index = 0;
            RunSilo(11111 + index, 30000 + index);
            return base.Init();
        }
        public  void RunSilo(int siloPort, int gatewayPort)
        {
            try
            {
                m_SiloHost  =  InitialiseSilo(siloPort, gatewayPort);
                Log.Debug("Silo started successfully");
            }
            catch (Exception ex)
            {
                Log.Debug(ex);
            }
        }
        public void  StopSilo()
        {
            try
            {
                m_SiloHost.StopAsync().Wait();
            }
            catch (Exception ex)
            {
                Log.Debug(ex);
            }
        }
        private   ISiloHost InitialiseSilo(int siloPort, int gatewayPort)
        {
            var gameconfig = GameModuleManager.Instance.GetModule<ConfigManager>().GameConfig;
            var builder = new SiloHostBuilder()
                /*
                .Configure<EndpointOptions>(options =>
                {
                    options.AdvertisedIPAddress = IPAddress.Parse(gameconfig.SiloIp);
                    options.SiloPort = gameconfig.SiloPort;
                    options.GatewayPort = gameconfig.SiloGatePort;
                    
                    options.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, 40000);
                    options.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, 50000);
                })*/
                //.UseLocalhostClustering(gameconfig.SiloPort, gameconfig.SiloGatePort)
                //.ConfigureEndpoints(gameconfig.SiloIp, gameconfig.SiloPort, gameconfig.SiloGatePort)
                //.UseLocalhostClustering()
                .UseAdoNetClustering(options =>
                {
                    options.Invariant = gameconfig.DB_Name;//GameConstant.DB_Name;
                    options.ConnectionString = gameconfig.Str_DBConnection;//GameConstant.Str_DBConnection;
                })
                .ConfigureEndpoints(siloPort: siloPort, gatewayPort: gatewayPort)
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = gameconfig.ClusterId;
                    options.ServiceId = gameconfig.ServiceId;
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
                parts.AddApplicationPart(typeof(HallGrains.GateWayGrain).Assembly).WithReferences();
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
            host.StartAsync().Wait();



            return host;
        }
        public override void Update()
        {

        }
        public override bool ShutDown()
        {
            StopSilo();
            return base.ShutDown();
        }
    }
}
