using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    
    public abstract class ReactiveSystem<IEntity> : IReactiveSystem where IEntity : Entity
    {
        public virtual int Priority => 0;

        readonly Collector<IEntity> m_Collector;
        readonly List<IEntity> m_Buffer;

        protected ReactiveSystem(Context<IEntity> context)
        {
            m_Collector = GetTrigger(context);
            m_Buffer = new List<IEntity>();
        }
        protected ReactiveSystem(Collector<IEntity> collector)
        {
            m_Collector = collector;
            m_Buffer = new List<IEntity>();
        }
        protected abstract Collector<IEntity> GetTrigger(Context<IEntity> context);

        protected abstract bool Filter(IEntity entity);
        protected abstract void Execute(List<IEntity> entities);

        public virtual void Activate()
        {
            m_Collector.Activate();
        }

        public void Clear()
        {
            m_Collector.ClearCollectedEntities();
        }

        public virtual  void Deactivate()
        {
            m_Collector.Deactivate();
        }

       

        public void Execute(float elapseSeconds)
        {
            if(m_Collector.Count!=0)
            {
                foreach (var e in m_Collector.CollectedEntities)
                {
                    if (Filter(e))
                    {
                        m_Buffer.Add(e);
                    }
                }

                Clear();

                if (m_Buffer.Count != 0)
                {
                    try
                    {
                        Execute(m_Buffer);
                    }
                    finally
                    {
                        m_Buffer.Clear();
                    }
                }
            }
        }
    }
}
