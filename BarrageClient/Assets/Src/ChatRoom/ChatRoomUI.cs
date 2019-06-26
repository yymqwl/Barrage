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

        public string Str_Ip = "127.0.0.1:2000";
        public StringBuilder m_Sb = new StringBuilder();

        protected string Str_Ping;
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
            Application.targetFrameRate = -1;
            Screen.SetResolution(960, 640, false);
            StartCoroutine(Ping_Msg());

        }

        public void SetPing_Str(string ping_str)
        {
            Str_Ping = ping_str;
        }
        string ChatId;
        string ChatMsg;

        public void OnGUI()
        {

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            GUILayout.TextArea(m_Sb.ToString(), GUILayout.Width(200),GUILayout.Height(400));
            GUILayout.Label(Str_Ping, GUILayout.Width(200));

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
                m_Session.Send(new ExitRoom_Req { Id =long.Parse(ChatId)});
            }
            GUILayout.EndVertical();


            GUILayout.EndHorizontal();

        }
        public IEnumerator Ping_Msg()
        {
            yield return new WaitForSeconds(1);
            if(m_Session == null|| !m_Session.IsConnected)
            {
                m_Session = ClientNetWork.Create(Str_Ip);
            }
 
            m_Session.Send(new Ping_Msg { Time = DateTime.UtcNow.Ticks });
            StartCoroutine(Ping_Msg());
            
        }
    }
}