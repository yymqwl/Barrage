using GameFramework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class TestTcp : MonoBehaviour
{
    TService m_TService;
    TChannel m_TChannel;
    void Start()
    {
        m_TService = new TService(Packet.PacketSizeLength2);
        m_TService.DisConnectedCallback += TService_DisConnectedCallback; ;
    }

    private void TService_DisConnectedCallback(AChannel ac)
    {
        Log.Debug($"{ac.Id}:DisConnect");
    }



    void Update()
    {
        m_TService.Update();
        OneThreadSynchronizationContext.Instance.Update();
    }
    public void OnGUI()
    {
        GUILayout.BeginVertical();
        if (GUILayout.Button("Connect", GUILayout.Width(200)))
        {
            m_TChannel = (TChannel)m_TService.ConnectChannel(NetHelper.ToIPEndPoint("127.0.0.1", 2500));
        }

        if (GUILayout.Button("Send", GUILayout.Width(200)))
        {
            using (var mem = new MemoryStream())
            {
                var word_byts = Encoding.UTF8.GetBytes("Hello Udp!");
                mem.Write(word_byts, 0, word_byts.Length);
                mem.Position = 0;
                m_TChannel.Send(mem);
            }

        }

        if (GUILayout.Button("DisConnect", GUILayout.Width(200)))
        {
            m_TChannel.DisConnect();
        }
        if (GUILayout.Button("Dispose", GUILayout.Width(200)))
        {
            m_TChannel.Dispose();
        }
        GUILayout.EndVertical();
    }
}
