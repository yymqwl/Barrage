using System;
using System.Collections.Generic;

namespace GameFramework.Fsm
{

    public abstract class AFsmState<T> where T : class
    {

        private readonly Dictionary<int, FsmEventHandler<T>> m_EventHandlers;


        public string Name
        {
            get
            {
                return GetType().FullName;
            }
        }

        /// <summary>
        /// 初始化有限状态机状态基类的新实例。
        /// </summary>
        public AFsmState()
        {
            m_EventHandlers = new Dictionary<int, FsmEventHandler<T>>();
        }

        /// <summary>
        /// 有限状态机状态初始化时调用。
        /// </summary>
        /// <param name="fsm">有限状态机引用。</param>
        public  virtual void OnInit(AFsm<T> fsm)
        {

        }

      
        public virtual void OnEnter(AFsm<T> fsm)
        {

        }

        public virtual void OnUpdate(AFsm<T> fsm, float elapseSeconds)
        {

        }
        public virtual void OnLeave(AFsm<T> fsm)
        {

        }

        public virtual void OnDestroy(AFsm<T> fsm)
        {
            m_EventHandlers.Clear();
        }

        public void SubscribeEvent(int eventId, FsmEventHandler<T> eventHandler)
        {
            if (eventHandler == null)
            {
                throw new GameFrameworkException("Event handler is invalid.");
            }

            if (!m_EventHandlers.ContainsKey(eventId))
            {
                m_EventHandlers[eventId] = eventHandler;
            }
            else
            {
                m_EventHandlers[eventId] += eventHandler;
            }
        }
        public void UnsubscribeEvent(int eventId, FsmEventHandler<T> eventHandler)
        {
            if (eventHandler == null)
            {
                throw new GameFrameworkException("Event handler is invalid.");
            }

            if (m_EventHandlers.ContainsKey(eventId))
            {
                m_EventHandlers[eventId] -= eventHandler;
            }
        }
        public void ChangeState<TState>(AFsm<T> fsm) where TState : AFsmState<T>
        {
            AFsm<T> fsmImplement = (AFsm<T>)fsm;
            if (fsmImplement == null)
            {
                throw new GameFrameworkException("FSM is invalid.");
            }

            fsmImplement.ChangeState<TState>();
        }

        public void ChangeState(AFsm<T> fsm, Type stateType)
        {
            AFsm<T> fsmImplement = (AFsm<T>)fsm;
            if (fsmImplement == null)
            {
                throw new GameFrameworkException("FSM is invalid.");
            }

            if (stateType == null)
            {
                throw new GameFrameworkException("State type is invalid.");
            }

            if (!typeof(AFsmState<T>).IsAssignableFrom(stateType))
            {
                throw new GameFrameworkException(string.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            fsmImplement.ChangeState(stateType);
        }

        public void OnEvent(AFsm<T> fsm, object sender, int eventId, object userData)
        {
            FsmEventHandler<T> eventHandlers = null;
            if (m_EventHandlers.TryGetValue(eventId, out eventHandlers))
            {
                if (eventHandlers != null)
                {
                    eventHandlers(fsm, sender, userData);
                }
            }
        }
    }
}
