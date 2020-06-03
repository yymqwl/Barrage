using GameFramework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp.Server;

namespace Mcpp
{
    [GameFrameworkModule]
    public class PlayerModule : GameFrameworkModule
    {

        protected Dictionary<string, WebPlayer> m_Dict_Pys = new Dictionary<string, WebPlayer>();
        protected DoubleMap<string, WebPlayer> m_Session_Pys = new DoubleMap<string, WebPlayer>();
        public override int Priority => 0;

        public Dictionary<string, WebPlayer> Dict_Pys
        {
            get
            {
                return this.m_Dict_Pys;
            }
        }
        public DoubleMap<string, WebPlayer> Session_Pys
        {
            get
            {
                return m_Session_Pys;
            }
        }
        public override bool Init()
        {
            this.m_Dict_Pys.Clear();
            this.m_Session_Pys.Clear();
            return base.Init();
        }
        public WebPlayer GetPlayer(string id)
        {
            WebPlayer py;
            this.m_Dict_Pys.TryGetValue(id, out py);
            return py;
        }
        public WebPlayer CreatePlayer(string id)
        {
            WebPlayer py = new WebPlayer(id);
            py.Init();
            if(!this.m_Dict_Pys.TryAdd(id,py))
            {
                Log.Debug($"Add Failed{id}");
            }
            return py;
        }
        public WebPlayer RemovePlayer(string id)
        {
            WebPlayer py;
            this.m_Dict_Pys.Remove(id, out py);
            if(py!=null)
            {
                py.ShutDown();
            }
            this.m_Session_Pys.RemoveByValue(py);
            return py;
        }
        public override bool ShutDown()
        {
            return base.ShutDown();
        }
        public void SetSession_Py(WebPlayer py, IWebSocketSession session)
        {
            this.m_Session_Pys.RemoveByValue(py);
            this.m_Session_Pys.Add(session.ID, py);

            py.WebSession = session;
        }

        protected List<string> m_OutOfTime_Pys = new List<string>();
        public override void Update()
        {
            m_OutOfTime_Pys.Clear();
            foreach (var kv in this.Dict_Pys)
            {
                kv.Value.Update();
                var ts = DateTime.Now - kv.Value.LastActiveTime;
                if(ts.TotalSeconds>= GameConstant.TPlayerIdle)
                {
                    m_OutOfTime_Pys.Add(kv.Key);
                }
            }
            m_OutOfTime_Pys.ForEach((string id) =>
            {
                RemovePlayer(id);
                Log.Debug("OutTime Remove player Uid" + id);
            });
            //m_OutOfTime_Pys.ForEach()
            //Log.Debug(DateTime.Now.Ticks);
        }
    }
}
