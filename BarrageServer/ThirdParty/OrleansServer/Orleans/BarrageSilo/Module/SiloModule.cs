using System;
using GameFramework;
using Orleans.Hosting;
using GameMain;
using Orleans.Configuration;
using Microsoft.Extensions.Logging;
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
            var builder = new SiloHostBuilder()
                .UseAdoNetClustering(options =>
                {
                    options.Invariant = GameConstant.DB_Name;
                    options.ConnectionString = GameConstant.Str_DBConnection;
                })
                .ConfigureEndpoints(siloPort: siloPort, gatewayPort: gatewayPort)
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
