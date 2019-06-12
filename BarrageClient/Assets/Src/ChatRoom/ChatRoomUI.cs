using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameFramework;
using GameMain;
using GameMain.ChatRoom;
using System.Text;

namespace ChatRoom
{
    public class ChatRoomUI : UInstance<ChatRoomUI>
    {

        public StringBuilder m_Sb = new StringBuilder();

        Session m_Session;

        public ClientNetWork ClientNetWork
        {
            get
            {
                return GameModuleManager.Instance.GetModule<SiloNetWork>().ClientNetWork;
            }
        }
        public void Start()
        {

            Screen.SetResolution(960, 640, false);
            StartCoroutine(Ping_Msg());

        }
        string ChatId;
        string ChatMsg;
        public void OnGUI()
        {

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            GUILayout.TextArea(m_Sb.ToString(), GUILayout.Width(200),GUILayout.Height(400));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            ChatId = GUILayout.TextField(ChatId, GUILayout.Width(20));
            if (GUILayout.Button("Join"))
            {
                long lid = long.Parse(ChatId);
                m_Session.Send(new Login_Req { Id= lid});
            }
            GUILayout.EndVertical();


            GUILayout.BeginVertical();
            ChatMsg = GUILayout.TextField(ChatMsg, GUILayout.Width(100));
            if (GUILayout.Button("Send"))
            {

                m_Session.Send(new Say_Req {Msg = ChatMsg} );
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            if (GUILayout.Button("Exit"))
            {

            }
            GUILayout.EndVertical();


            GUILayout.EndHorizontal();

        }
        public IEnumerator Ping_Msg()
        {
            yield return new WaitForSeconds(1);
            if(m_Session == null|| !m_Session.IsConnected)
            {
                m_Session = ClientNetWork.Create("127.0.0.1:2000");
            }
 
            m_Session.Send(new Ping_Msg { Time = DateTime.Now.Ticks });
            StartCoroutine(Ping_Msg());
            
        }
    }
}