using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameMain;
using GameMain.Msg;
public class TestNetWork : MonoBehaviour
{

    public Session m_Session;
    void Start()
    {
       
    }
    void Update()
    {
        
    }
    public ClientNetWork ClientNetWork
    {
        get
        {
            return GameModuleManager.Instance.GetModule<NetWorkModule>().ClientNetWork;
        }
    }
    public void OnGUI()
    {
        GUILayout.BeginVertical();
        if (GUILayout.Button("Connect", GUILayout.Width(200)))
        {
            m_Session = ClientNetWork.Create("127.0.0.1:2000");
        }

        if (GUILayout.Button("Send", GUILayout.Width(200)))
        {
            for(int i=0;i<100;++i)
            {
                Ping_Msg ping_Msg = new Ping_Msg();
                ping_Msg.Time = TimeHelper.ClientNow();
                m_Session.Send(ping_Msg);
            }

        }

        if (GUILayout.Button("DisConnect", GUILayout.Width(200)))
        {
            m_Session.ShutDown();
        }
        GUILayout.EndVertical();
    }
}
