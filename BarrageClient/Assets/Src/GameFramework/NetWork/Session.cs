using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public class Session: ABehaviourSet
    {
        private AChannel m_AChannel;


        
        public AChannel AChannel
        {
            get
            {
                return m_AChannel;
            }
        }
        public long Id
        {
            get
            {
                return m_AChannel.Id;
            }
        }
        public ChannelType ChannelType
        {
            get
            {
                return this.m_AChannel.ChannelType;
            }
        }

        public bool IsConnected
        {
            get
            {
                return m_AChannel.IsConnected;
            }
        }
        public IPEndPoint RemoteAddress
        {
            get
            {
                return this.m_AChannel.RemoteAddress;
            }
        }
        public NetworkBv Network
        {
            get
            {
                return this.GetParent<NetworkBv>();
            }
        }

        public Session(AChannel aChannel)
        {
            m_AChannel = aChannel;
            m_AChannel.ReadCallback += this.OnRead;
        }
        public void OnRead(AChannel ac,MemoryStream memoryStream)
        {
            try
            {

            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        public void Send(MemoryStream stream)
        {
            m_AChannel.Send(stream);
        }
        public override bool Init()
        {

            return base.Init();
        }
        public override bool Shut()
        {
            Network.Remove(Id);
            m_AChannel.Dispose();

            return base.Shut();
        }
    }
}
