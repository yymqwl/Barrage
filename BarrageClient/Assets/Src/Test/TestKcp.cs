using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using GameFramework;
using System.Text;

public class TestKcp : MonoBehaviour
{

    KService m_KService;
    KChannel m_kChannel;
    void Start()
    {
        m_KService = new KService();
        m_KService.RemoveCallback += KService_DisConnectCallback;

        m_kChannel = (KChannel)m_KService.ConnectChannel(NetHelper.ToIPEndPoint("127.0.0.1", 2000));
        Log.Debug($"{TimeHelper.ClientNowSeconds()}");
    }

    private void KService_DisConnectCallback(AChannel ac)
    {
        Log.Debug($"{ac.Id}:disconnect");
    }

    void Update()
    {
        m_KService.Update();
    }
    /*
    public void OnDisable()
    {
        KService.Instance.Dispose();
    }*/

    public void OnGUI()
    {
        GUILayout.BeginVertical();
        if(GUILayout.Button("Connect", GUILayout.Width(200)))
        {
            m_kChannel.Connect();

            //m_kChannel = (KChannel)m_KService.ConnectChannel(NetHelper.ToIPEndPoint("127.0.0.1", 2000));
        }
        if (GUILayout.Button("DisConnect", GUILayout.Width(200)))
        {
            m_kChannel.Disconnect();
        }
        if (GUILayout.Button("Send",GUILayout.Width(200)) )
        {
            using (var mem = new MemoryStream())
            {
                var word_byts = Encoding.UTF8.GetBytes("Hello Udp!");
                mem.Write(word_byts, 0, word_byts.Length);
                m_kChannel.Send(mem);
            }
                
        }
        GUILayout.EndVertical();
    }
}
