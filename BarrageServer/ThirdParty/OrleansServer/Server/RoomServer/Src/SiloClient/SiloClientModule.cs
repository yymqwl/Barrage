using GameFramework;
using Orleans;
using Orleans.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TableRoom;

namespace RoomServer
{
    [GameFrameworkModule]
    public class SiloClientModule : GameFrameworkModule
    {
        protected IClusterClient m_IClusterClient;
        protected IMainEntry m_IMainEntry;


        public IMainEntry IMainEntry
        {
            get
            {
                return m_IMainEntry;
            }
        }
        protected INetUserEntry m_INetUserEntry;
        public INetUserEntry INetUserEntry
        {
            get
            {
                return m_INetUserEntry;
            }
        }
        protected ITableRoomEntry m_ITableRoomEntry;
        public ITableRoomEntry ITableRoomEntry
        {
            get
            {
                return m_ITableRoomEntry;
            }
        }

        public IClusterClient IClusterClient
        {
            get
            {
                return m_IClusterClient;
            }
        }

        public override bool Init()
        {
            var bret = base.Init();

            m_IClusterClient = InitialiseClient();
            m_IMainEntry = m_IClusterClient.GetGrain<IMainEntry>(0);
            m_INetUserEntry = m_IMainEntry.GetINetUserEntry().Result;
            m_ITableRoomEntry = m_IMainEntry.GetITableRoomEntry().Result;

            Log.Debug("SiloClientModule:Init");

            return bret;
        }
        public override bool ShutDown()
        {
            m_IClusterClient.AbortAsync().Wait();
            var bret = base.ShutDown();
            Log.Debug("SiloClientModule:ShutDown");
            return bret;
        }
        protected IClusterClient InitialiseClient()
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
                    //.ConfigureLogging(log => log.SetMinimumLevel(LogLevel.Warning).AddConsole())
                    .Build();

            client.Connect().Wait();
            return client;
        }
        public override void Update()
        {

        }
    }
}
