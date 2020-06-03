using GameFramework;
using System;
using System.Collections.Generic;
using System.Text;
using GameFramework.Event;
namespace Mcpp
{
    [GameFrameworkModule]
    public class DataObserverModule : GameFrameworkModule
    {
        public override int Priority => 100;
        EventManager m_DataEventManager = new EventManager();

        public EventManager DataEventManager
        {
            get { return m_DataEventManager; }
        }
        public override bool Init()
        {
            var pret = base.Init();
            //Log.Debug("DataObserverModule Init"+this.Priority);
            return pret;
        }
        public override void Update()
        {

        }
        public override bool ShutDown()
        {
            //Log.Debug("DataObserverModule Shut"+this.Priority);
            return base.ShutDown();
        }
    }
    public enum EData_Event_Type
    {
        EData_Change = 0,
    }
}
