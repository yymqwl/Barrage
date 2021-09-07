using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using Orleans;
using Orleans.Configuration;
using TableRoom;

namespace TableRoomSilo
{

    [GameFrameworkModule]
    public class SiloControlModule : GameFrameworkModule
    {

        protected IClusterClient m_IClusterClient;
        protected IMainEntry m_IMainEntry;
        protected SiloTimer m_SiloTimer;
        public  override bool Init()
        {
            var bret = base.Init();
            m_IClusterClient = InitialiseClient().Result;
            m_IMainEntry = m_IClusterClient.GetGrain<IMainEntry>(0);

            m_SiloTimer = new SiloTimer();
            m_SiloTimer.Reset();
            return bret;
        }

        public override bool ShutDown()
        {
            m_IClusterClient.AbortAsync().Wait();
            var bret = base.ShutDown();

            return bret;
        }
        protected  async Task<IClusterClient> InitialiseClient()
        {
            var client = new ClientBuilder()
                    .UseLocalhostClustering()
                    .Configure<ClusterOptions>(options =>
                    {
                        options.ClusterId = TableRoom.GameConstant.ClusterId;
                        options.ServiceId = TableRoom.GameConstant.ServiceId;
                    })
                    .ConfigureApplicationParts(parts =>
                    {
                        parts.AddApplicationPart(typeof(IChatRoomEntry).Assembly);
                    }
                    )
                    //.ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning).AddConsole())
                    .Build();

            await client.Connect();
            return client;
        }

        public override void Update()
        {
            if(m_SiloTimer.CheckUpdate())
            {
                m_IMainEntry.Update(m_SiloTimer.DeltaTime).Wait();
            }

        }
    }
}
