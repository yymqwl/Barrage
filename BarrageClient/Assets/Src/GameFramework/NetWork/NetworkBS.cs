using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public abstract class NetWorkBs: ABehaviourSet
    {
        protected AService m_AService;
        protected readonly Dictionary<long, Session> m_Dict_Sessions = new Dictionary<long, Session>();
        protected NetworkProtocol m_NetworkProtocol;

        protected ChannelType m_ChannelType;
        protected string m_StrIpEndPoint;
        
        
        public IMessagePacker MessagePacker { get; set; }
        public NetworkProtocol NetworkProtocol { get { return m_NetworkProtocol; } }
        public string StrIpEndPoint { get { return m_StrIpEndPoint; } } 
        public IOpCodeType IOpCodeType
        {
            get;set;
        }
        public IMessageDispatcher IMessageDispatcher
        {
            get;set;
        }
        //public IMessageDispatcher MessageDispatcher { get; set; }
        /*
        public NetworkRoot(NetworkProtocol networkProtocol, ChannelType channelType,string StrIpEndPoint)
        {
            m_NetworkProtocol = networkProtocol;
            m_ChannelType = channelType;
            m_StrIpEndPoint = StrIpEndPoint;
        }*/


        
       
        public override bool Init()
        {
            if(m_ChannelType == ChannelType.Connect)
            {
                switch (m_NetworkProtocol)
                {
                    case NetworkProtocol.KCP:
                        this.m_AService = new KService();
                        break;
                    case NetworkProtocol.TCP:
                        this.m_AService = new TService(Packet.PacketSizeLength2);
                        break;
                   
                    case NetworkProtocol.WebSocket:
                        this.m_AService = new WService();
                        break;
                        
                }
            }
            else
            {
                try
                {
                    IPEndPoint ipEndPoint;
                    switch (m_NetworkProtocol)
                    {
                        case NetworkProtocol.KCP:
                            ipEndPoint = NetHelper.ToIPEndPoint(m_StrIpEndPoint);
                            this.m_AService = new KService(ipEndPoint, this.OnAccept);
                            break;
                        case NetworkProtocol.TCP:
                            ipEndPoint = NetHelper.ToIPEndPoint(m_StrIpEndPoint);
                            this.m_AService = new TService(Packet.PacketSizeLength2, ipEndPoint, this.OnAccept);
                            break;
                        case NetworkProtocol.WebSocket:
                            string[] prefixs = m_StrIpEndPoint.Split(';');
                            this.m_AService = new WService(prefixs, this.OnAccept);
                            break;
                    }
                    this.m_AService.DisConnectedCallback += OnDisConnected;

                }
                catch (Exception e)
                {
                    throw new Exception($"NetworkComponent Awake Error {m_StrIpEndPoint}", e);
                }

            }

            return base.Init();
        }

        private void OnDisConnected(AChannel ac)
        {
            Log.Debug($"session disconnect {ac.Id}");
            Remove(ac.Id);
        }

        protected void OnAccept(AChannel channel)
        {
            Session session =  Create(channel);
        }
        public virtual void Remove(Session session)
        {
            Remove(session.Id);
        }
        public virtual void Remove(long id)
        {
            Session session;
            if (!this.m_Dict_Sessions.TryGetValue(id, out session))
            {
                return;
            }
            this.m_Dict_Sessions.Remove(id);
        }
        public Session Get(long id)
        {
            Session session;
            this.m_Dict_Sessions.TryGetValue(id, out session);
            return session;
        }

        /// <summary>
        /// 创建一个新Session
        /// </summary>
        public Session Create(IPEndPoint ipEndPoint)
        {
            AChannel channel = this.m_AService.ConnectChannel(ipEndPoint);
            Session session = Create(channel);
            return session;
        }
        private Session Create(AChannel channel)
        {
            Session session = new Session(channel);
            session.Parent = this;
            this.m_Dict_Sessions.Add(session.Id, session);
            return session;
        }
        /// <summary>
        /// 创建一个新Session
        /// </summary>
        public Session Create(string address)
        {
            AChannel channel = this.m_AService.ConnectChannel(address);
            Session session = Create(channel);
            return session;
        }
        public override void Update()
        {
            if(m_AService == null)
            {
                return;
            }
            m_AService.Update();
        }
        public override bool ShutDown()
        {
            bool pret= base.ShutDown();

            var sessions = this.m_Dict_Sessions.Values.ToList();
            foreach (var session in sessions)
            {
                session.ShutDown();
            }

            m_AService.Dispose();
            return pret;
        }




    }

}
