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

        //
        private readonly byte[] m_OpCode_Bytes = new byte[2];


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
        public int Error
        {
            get
            {
                return this.AChannel.Error;
            }
            set
            {
                this.AChannel.Error = value;
            }
        }
        public IPEndPoint RemoteAddress
        {
            get
            {
                return this.m_AChannel.RemoteAddress;
            }
        }
        public NetWorkBs Network
        {
            get
            {
                return this.GetParent<NetWorkBs>();
            }
        }
        public MemoryStream Stream
        {
            get
            {
                return this.AChannel.Stream;
            }
        }

        public Session(AChannel aChannel)
        {
            m_AChannel = aChannel;
            m_AChannel.ReadCallback += this.OnRead;
        }
        public void OnRead(AChannel ac,MemoryStream memoryStream)
        {
            ushort opcode = 0;
            IMessage message = null;
            try
            {
                memoryStream.Seek(Packet.MessageIndex, SeekOrigin.Begin);
                opcode = BitConverter.ToUInt16(memoryStream.GetBuffer(), Packet.OpcodeIndex);
                message = Network.MessagePacker.DeserializeFrom(Network.OpCodeTypeBv.GetType(opcode), memoryStream) as IMessage;
            }
            catch (Exception e)
            {
                // 出现任何消息解析异常都要断开Session，防止客户端伪造消息
                Log.Error($"opcode:{opcode}:{e}");
                this.Error = ErrorCode.ERR_PacketParserError;
                this.Network.Remove(this.Id);
                return;
            }
            Network.MessageDispatherBv.Handle(this, new MessageInfo(opcode, message));
            
        }
        public void Send(MemoryStream stream)
        {
            m_AChannel.Send(stream);
        }


        public void Send(IMessage message)
        {
           ushort opcode = Network.OpCodeTypeBv.GetOpcode(message.GetType());
           Send(opcode,message);

        }
        public void Send( ushort opcode, IMessage message)
        {
            if(!IsConnected)
            {
                Log.Debug("Session Not Connected");
            }

            MemoryStream stream = this.Stream;
            stream.Seek(Packet.MessageIndex, SeekOrigin.Begin);
            stream.SetLength(Packet.MessageIndex);

            Network.MessagePacker.SerializeTo(message, stream);
            stream.Seek(0, SeekOrigin.Begin);

            m_OpCode_Bytes.WriteTo(0, opcode);
            Array.Copy(m_OpCode_Bytes, 0, stream.GetBuffer(), 0, m_OpCode_Bytes.Length);




             this.Send(stream);

        }
        public override bool Init()
        {

            return base.Init();
        }
        public override bool ShutDown()
        {
            Network.Remove(Id);
            m_AChannel.Dispose();

            return base.ShutDown();
        }
    }
}
