using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameMain;
using GameFramework;

namespace ChatRoom
{
    public class ChatRoomEntry : AGameMainEntry
    {
        public override void Entry(string[] args)
        {
            base.Entry(args);

            AssemblyManager.Instance.Add( typeof(ChatRoom.ChatRoomEntry).Assembly);
            GameModuleManager.Instance.CreateModule<ConfigModule>();
            GameModuleManager.Instance.CreateModule<SiloNetWork>();

            GameModuleManager.Instance.Init();
        }
    }
}