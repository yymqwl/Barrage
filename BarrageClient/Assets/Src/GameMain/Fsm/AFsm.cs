using System;
using System.Collections.Generic;

namespace GameFramework.Fsm
{
    /// <summary>
    /// 有限状态机基类。
    /// </summary>
    public abstract class AFsm<T> where T:class
    {
        private readonly T m_Owner;
        private AFsmState<T> m_CurrentState;
        private readonly Dictionary<string, Variable> m_Datas;
        private readonly Dictionary<Type, AFsmState<T>> m_States;
        private float m_CurrentStateTime;
        private bool m_IsDestroyed;
        public T Owner
        {
            get
            {
                return m_Owner;
            }
        }
        public  Type OwnerType
        {
            get
            {
                return typeof(T);
            }
        }
   
        public string Name
        {
            get
            {
                return GetType().FullName;
            }
        }

        /// <summary>
        /// 获取有限状态机中状态的数量。
        /// </summary>
        public  int FsmStateCount
        {
            get
            {
                return m_States.Count;
            }
        }

        /// <summary>
        /// 获取有限状态机是否正在运行。
        /// </summary>
        public  bool IsRunning
        {
            get
            {
                return m_CurrentState != null;
            }
        }

        /// <summary>
        /// 获取有限状态机是否被销毁。
        /// </summary>
        public  bool IsDestroyed
        {
            get
            {
                return m_IsDestroyed;
            }
        }



        /// <summary>
        /// 获取当前有限状态机状态持续时间。
        /// </summary>
        public  float CurrentStateTime
        {
            get
            {
                return m_CurrentStateTime;
            }
        }

        public AFsm(T owner)
        {
            if (owner == null)
            {
                throw new GameFrameworkException("FSM owner is invalid.");
            }

            m_Owner = owner;
            m_States = new Dictionary<Type, AFsmState<T>>();
            m_Datas = new Dictionary<string, Variable>();

            m_CurrentStateTime = 0f;
            m_CurrentState = null;
            m_IsDestroyed = false;
        }
        /// <summary>
        /// 开始有限状态机。
        /// </summary>
        /// <typeparam name="TState">要开始的有限状态机状态类型。</typeparam>
        public void Start<TState>() where TState : AFsmState<T>
        {
            if (IsRunning)
            {
                throw new GameFrameworkException("FSM is running, can not start again.");
            }

            AFsmState<T> state = GetState<TState>();
            if (state == null)
            {
                throw new GameFrameworkException(string.Format($"FSM can not start state {typeof(TState).FullName} which is not exist."));
            }

            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }
        public TState CreateFsmState<TState>(TState fsmstate) where TState : AFsmState<T>
        {
            if (fsmstate == null)
            {
                throw new GameFrameworkException("Null fsmstate");

            }
            Type tp = fsmstate.GetType();
            if (m_States.ContainsKey(tp))
            {
                throw new GameFrameworkException(string.Format("FSM  State is already exist."));
            }

            m_States.Add(tp, fsmstate);
            fsmstate.OnInit(this);
            return fsmstate;
        }
        public void Start(Type stateType)
        {
            if (IsRunning)
            {
                throw new GameFrameworkException("FSM is running, can not start again.");
            }

            if (stateType == null)
            {
                throw new GameFrameworkException("State type is invalid.");
            }

            if (!typeof(AFsmState<T>).IsAssignableFrom(stateType))
            {
                throw new GameFrameworkException(string.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            AFsmState<T> state = GetState(stateType);
            if (state == null)
            {
                throw new GameFrameworkException(string.Format("FSM '{0}' can not start state '{1}' which is not exist.", state, stateType.FullName));
            }

            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }
        public bool HasState<TState>() where TState : AFsmState<T>
        {
            return m_States.ContainsKey(typeof(TState));
        }
        public bool HasState(Type stateType)
        {
            if (stateType == null)
            {
                throw new GameFrameworkException("State type is invalid.");
            }

            if (!typeof(AFsmState<T>).IsAssignableFrom(stateType))
            {
                throw new GameFrameworkException(string.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            return m_States.ContainsKey(stateType);
        }
        public TState GetState<TState>() where TState : AFsmState<T>
        {
            AFsmState<T> state = null;
            if (m_States.TryGetValue(typeof(TState), out state))
            {
                return (TState)state;
            }
            return null;
        }
        public AFsmState<T> GetState(Type stateType)
        {
            if (stateType == null)
            {
                throw new GameFrameworkException("State type is invalid.");
            }

            if (!typeof(AFsmState<T>).IsAssignableFrom(stateType))
            {
                throw new GameFrameworkException(string.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            AFsmState<T> state = null;
            if (m_States.TryGetValue(stateType, out state))
            {
                return state;
            }

            return null;
        }
        public AFsmState<T>[] GetAllStates()
        {
            List<AFsmState<T>> Temp_Fsm = new List<AFsmState<T>>();
            foreach (var fs in m_States.Values)
            {
                Temp_Fsm.Add(fs);
            }
            return Temp_Fsm.ToArray();
        }
        public void FireEvent(object sender, int eventId)
        {
            if (m_CurrentState == null)
            {
                return;
            }

            m_CurrentState.OnEvent(this, sender, eventId, null);
        }
        public void FireEvent(object sender, int eventId, object userData)
        {
            if (m_CurrentState == null)
            {
                return;
            }

            m_CurrentState.OnEvent(this, sender, eventId, userData);
        }
        public bool HasData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Data name is invalid.");
            }

            return m_Datas.ContainsKey(name);
        }
        public TData GetData<TData>(string name) where TData : Variable
        {
            return (TData)GetData(name);
        }
        public Variable GetData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Data name is invalid.");
            }

            Variable data = null;
            if (m_Datas.TryGetValue(name, out data))
            {
                return data;
            }

            return null;
        }
        public void SetData<TData>(string name, TData data) where TData : Variable
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Data name is invalid.");
            }
            m_Datas[name] = data;
        }
        public void SetData(string name, Variable data)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Data name is invalid.");
            }

            m_Datas[name] = data;
        }
        public bool RemoveData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Data name is invalid.");
            }

            return m_Datas.Remove(name);
        }


        public void ChangeState<TState>() where TState : AFsmState<T>
        {
            ChangeState(typeof(TState));
        }
        public void ChangeState(Type stateType)
        {
            if (m_CurrentState == null)
            {
                throw new GameFrameworkException("Current state is invalid.");
            }

            AFsmState<T> state = GetState(stateType);
            if (state == null)
            {
                throw new GameFrameworkException(string.Format("FSM '{0}' can not change state to '{1}' which is not exist.", state, stateType.FullName));
            }

            m_CurrentState.OnLeave(this);
            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }
        public bool IsInState<TState>() where TState : AFsmState<T>
        {
            if (m_CurrentState == null)
            {
                return false;
            }
            if (m_CurrentState.GetType() == typeof(TState))
            {
                return true;
            }
            return false;
        }
        public virtual void Update(float elapseSeconds)
        {
            if (m_CurrentState == null)
            {
                return;
            }

            m_CurrentStateTime += elapseSeconds;
            m_CurrentState.OnUpdate(this, elapseSeconds);
        }

        /// <summary>
        /// 关闭并清理有限状态机。
        /// </summary>
        public virtual void ShutDown()
        {
            if (m_CurrentState != null)
            {
                m_CurrentState.OnLeave(this);
                m_CurrentState = null;
                m_CurrentStateTime = 0f;
            }

            foreach (var state in m_States.Values)
            {
                state.OnDestroy(this);
            }

            m_States.Clear();
            m_Datas.Clear();

            m_IsDestroyed = true;
        }
    }
}
