using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using StackExchange.Redis;



namespace Mcpp
{


    [GameFrameworkModule]
    public class DbModule : GameFrameworkModule
    {
        protected ConnectionMultiplexer m_Redis;
        protected IDatabase m_Db;

        public IDatabase Db
        {
            get
            {
                return this.m_Db;
            }
        }
        public override bool Init()
        {
            m_Redis = ConnectionMultiplexer.Connect(GameMainEntry.Instance.SettingModule.ServerSetting.Db_Config);
            m_Db = m_Redis.GetDatabase();


            //this.m_Db.()
            /*
            var sub = redis.GetSubscriber();
            sub.Subscribe("Channel1", new Action<RedisChannel, RedisValue>((channel, message) =>
            {
                Console.WriteLine("Channel1:"+ Thread.CurrentThread.ManagedThreadId + " 订阅收到消息：" + message);
            }));
            for (int i = 0; i < 10; i++)
            {
                sub.Publish("Channel1", "msg" + i+"threadid:"+ Thread.CurrentThread.ManagedThreadId);//向频道 Channel1 发送信息
            }

            */
            
            /*
            {
                var st1 = this.m_Db.HashGet("School", 1);
                var st3 = this.m_Db.HashGet("School", 3);
            }
            
            {
                Student st1 = new Student() { Id = 1, Name = "zsy" };
                Student st2= new Student() { Id = 2, Name = "yyx" };

                this.m_Db.HashSet("School", st1.Id, JsonHelper.JsonSerialize_String(st1) );
                this.m_Db.HashSet("School", st2.Id, JsonHelper.JsonSerialize_String(st2));
            }*/
            /*
            {
                
                var v =  db.ListGetByIndex("hello",2);
                Console.WriteLine(Encoding.UTF8.GetString(v));
                //db.StringSet("1", "zsy", TimeSpan.FromSeconds(10));
            }
            {
                string value = db.StringGet("123");
                Console.WriteLine(value); // writes: "abcdefg"
            }*/


                return base.Init();
        }
        
        public class Student
        {
            public int Id;
            public string Name;
        }
        public override bool ShutDown()
        {
            return base.ShutDown();
        }
        public override void Update()
        {

        }
    }
}
