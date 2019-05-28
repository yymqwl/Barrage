using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using System.IO;
using System.Text;

public class TestAc : MonoBehaviour
{
    public AService m_AService;
    public AChannel m_AChannel;

    public NetworkProtocol ChannelType = NetworkProtocol.KCP;

    void Start()
    {
        if(ChannelType == NetworkProtocol.TCP)
        {
            m_AService = new TService(Packet.PacketSizeLength2);
        }
        else if(ChannelType == NetworkProtocol.KCP)
        {
            m_AService = new KService();
        }
        else if(ChannelType == NetworkProtocol.WebSocket)
        {
            m_AService = new WService();
        }

        m_AService.DisConnectedCallback += AService_DisConnectedCallback;
    }

    //收取断开消息
    private void AService_DisConnectedCallback(AChannel ac)
    {
        Log.Debug($"{ac.Id}:DisConnect");
    }
    private void AChannel_ReadCallback(AChannel ac, MemoryStream ms)
    {
        Log.Debug($"{ac.Id}Msg:{Encoding.UTF8.GetString(ms.ToArray())}");
    }
    void Update()
    {
        m_AService.Update();
    }
    public void OnGUI()
    {
        if(ChannelType == NetworkProtocol.TCP)
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Connect", GUILayout.Width(200)))
            {
                m_AChannel = (TChannel)m_AService.ConnectChannel(NetHelper.ToIPEndPoint("127.0.0.1", 2500));
                m_AChannel.ReadCallback += AChannel_ReadCallback;
            }

            if (GUILayout.Button("Send", GUILayout.Width(200)))
            {
                using (var mem = new MemoryStream())
                {
                    var word_byts = Encoding.UTF8.GetBytes("Hello Tcp!");
                    mem.Write(word_byts, 0, word_byts.Length);
                    mem.Position = 0;
                    m_AChannel.Send(mem);
                }

            }

            if (GUILayout.Button("DisConnect", GUILayout.Width(200)))
            {
                m_AChannel.DisConnect();
            }
            GUILayout.EndVertical();

        }
        else if (ChannelType == NetworkProtocol.KCP)
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Connect", GUILayout.Width(200)))
            {
                m_AChannel = m_AService.ConnectChannel(NetHelper.ToIPEndPoint("127.0.0.1", 2000));
                m_AChannel.ReadCallback += AChannel_ReadCallback;
            }

            if (GUILayout.Button("Send", GUILayout.Width(200)))
            {
                using (var mem = new MemoryStream())
                {
                    var word_byts = Encoding.UTF8.GetBytes("Hello Kcp!");
                    mem.Write(word_byts, 0, word_byts.Length);
                    mem.Position = 0;
                    m_AChannel.Send(mem);
                }

            }

            if (GUILayout.Button("DisConnect", GUILayout.Width(200)))
            {
                m_AChannel.DisConnect();
            }
            GUILayout.EndVertical();
        }
        else if (ChannelType == NetworkProtocol.WebSocket)
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Connect", GUILayout.Width(200)))
            {
                m_AChannel = m_AService.ConnectChannel("ws://127.0.0.1:3000/");
                m_AChannel.ReadCallback += AChannel_ReadCallback;
            }

            if (GUILayout.Button("Send", GUILayout.Width(200)))
            {
                using (var mem = new MemoryStream())
                {
                    var word_byts = Encoding.UTF8.GetBytes("Hello websocket!");
                    mem.Write(word_byts, 0, word_byts.Length);
                    mem.Position = 0;
                    m_AChannel.Send(mem);
                }

            }

            if (GUILayout.Button("DisConnect", GUILayout.Width(200)))
            {
                m_AChannel.DisConnect();
            }
            GUILayout.EndVertical();
        }

    }


}
