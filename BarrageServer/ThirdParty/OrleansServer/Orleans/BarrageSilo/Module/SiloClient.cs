using GameFramework;
using GameMain;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IHall;
namespace BarrageSilo
{
    [GameFrameworkModule]
    public class SiloClient: GameFrameworkModule
    {
        public override int Priority => 10;
        private IClusterClient m_ClusterClient;
        public IClusterClient ClusterClient
        {
            get
            {
                return m_ClusterClient;
            }
        }
        
        public IMainEntry MainEntry
        {
            get
            {
                return m_MainEntry;
            }
        }

        protected IMainEntry m_MainEntry;
        protected SiloGateWay_Obs m_SiloGateWay_Obs;
        protected IGateWay_Obs m_IGateWay_Obs;

        public override bool Init()
        {

            m_ClusterClient = InitialiseClient();

            m_MainEntry = m_ClusterClient.GetGrain<IMainEntry>(0);

            m_SiloGateWay_Obs = new SiloGateWay_Obs();
            m_IGateWay_Obs = m_ClusterClient.CreateObjectReference<IGateWay_Obs>(m_SiloGateWay_Obs).Result;

            m_MainEntry.GetIGateWay().Result.SubscribeAsync(m_IGateWay_Obs).Wait();




            return base.Init();
        }
        public void StopSilo()
        {
            try
            {
                m_ClusterClient.Close().Wait();
                Log.Debug("ClusterClient Success");
            }
            catch (Exception ex)
            {
                Log.Debug(ex);
            }
        }
        private IClusterClient InitialiseClient()
        {
            try
            {
                var client = new ClientBuilder()
                .UseAdoNetClustering(options =>
                {
                    options.Invariant = GameConstant.DB_Name;
                    options.ConnectionString = GameConstant.Str_DBConnection;
                })
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = GameConstant.ClusterId;
                    options.ServiceId = GameConstant.ServiceId;
                })
                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(IHall.IGateWay).Assembly);
                })
                 .ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning).AddConsole())
                 .Build();
                client.Connect(RetryFilter).Wait();
                return client;
            }
            catch (Exception e)
            {
                Log.Debug(e);
            }
            return null;

        }
        private const int InitializeAttemptsBeforeFailing = 5;
        private static int attempt = 0;
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
        public override void Update()
        {
            m_MainEntry.Update().Wait();
        }
        public override bool ShutDown()
        {
            m_MainEntry.GetIGateWay().Result.UnSubscribeAsync(m_IGateWay_Obs).Wait();
            m_ClusterClient.DeleteObjectReference<IGateWay_Obs>(m_IGateWay_Obs);
            StopSilo();
            return base.ShutDown();
        }
    }
}
