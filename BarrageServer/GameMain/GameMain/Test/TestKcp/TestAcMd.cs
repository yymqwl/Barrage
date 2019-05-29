using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameMain
{
    //[GameFrameworkModule()]
    public class TestAcMd : GameFrameworkModule
    {
        public NetworkProtocol ChannelType = NetworkProtocol.WebSocket;

        AService m_AService;
        public override bool Init()
        {
            IdGenerater.AppId = 10;
            if(ChannelType == NetworkProtocol.TCP)
            {
                m_AService = new TService(Packet.PacketSizeLength2, NetHelper.ToIPEndPoint("127.0.0.1", 2500), Accept);
            }
            else if(ChannelType == NetworkProtocol.KCP)
            {
                m_AService = new KService(NetHelper.ToIPEndPoint("127.0.0.1", 2000), Accept);
            }
            else if (ChannelType == NetworkProtocol.WebSocket)
            {
                m_AService = new WService(new string[] { $"http://127.0.0.1:3000/" }, Accept);
            }

            m_AService.DisConnectedCallback += AService_DisConnectedCallback;
            return base.Init();
        }

        private void AService_DisConnectedCallback(AChannel ac)
        {
            Log.Debug($"{ac.Id}:DisConnect");
        }

        public void Accept(AChannel ac)
        {
            ac.ReadCallback += Ac_ReadCallback;
            Log.Debug($"{ac.Id}:Accept");
        }
        private void Ac_ReadCallback(AChannel ac,MemoryStream ms)
        {

            var str_rv = Encoding.UTF8.GetString(ms.ToArray());
            Log.Debug($"{ac.Id}Msg:{str_rv}");
            var word_byts =  Encoding.UTF8.GetBytes("Server:"+str_rv);
            ms.Write(word_byts, 0, word_byts.Length);
            ms.Position = 0;
            ac.Send(ms);
        }
        public override void Update()
        {
            m_AService.Update();
        }
        public override bool ShutDown()
        {
            m_AService.Dispose();
            return base.ShutDown();
        }
    }
}
