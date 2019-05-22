using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;


namespace GameFramework
{
    public delegate void EntityComponentChanged(
    Entity entity, int index, IComponent component
    );

    public delegate void EntityComponentReplaced(
        Entity entity, int index, IComponent previousComponent, IComponent newComponent
    );
    public delegate void EntityEvent(Entity entity);

    [ProtoContract]
    public  class Entity
    {
        [ProtoMember(100)]
        public long Id { set; get; }
        [ProtoMember(101)]
        public Dictionary<int, IComponent> m_Dict_Components;


        public event EntityComponentChanged OnComponentAdded;
        public event EntityComponentChanged OnComponentRemoved;
        public event EntityComponentReplaced OnComponentReplaced;


        public event EntityEvent OnDestroyEntity;
        public event EntityEvent OnDisposeEntity;
        public event EntityEvent OnCreateEntity;
        public event EntityEvent OnInitEntity;

        IContext m_Context;
        public IContext Context
        {
            get { return m_Context; }
        }



        public Entity()
        {
            Id = 0;
            m_Dict_Components = new Dictionary<int, IComponent>();
        }



        
        public void AddComponent<T>(T comp)where T:IComponent
        {
            
            int index = m_Context.ContextInfo.GetComponentIndex<T>();
            AddComponent(index,comp);
        }

        public void AddComponent(int index, IComponent component)
        {
            if (HasComponent(index))
            {
                throw new GameFrameworkException
                    (
                    $"{index}Cannot add component You should check if an entity already has the component"
                    +"You should check if an entity already has the component"+
                    "before adding it or use entity.ReplaceComponent()"
                );
            }
            m_Dict_Components[index] = component;
            if (OnComponentAdded != null)
            {
                OnComponentAdded(this, index, component);
            }
        }
     
        public IComponent GetComponent(int index)
        {
            IComponent component = null;
            m_Dict_Components.TryGetValue(index, out component);
            return component;
        }

        public bool HasComponent<T>()
        {
            int index = GetComponentIndex<T>();
            return HasComponent(index);
        }
        public int GetComponentIndex<T>()
        {
            return m_Context.ContextInfo.GetComponentIndex<T>();
        }

        public bool HasComponent(int index)
        {
            IComponent icp;
            m_Dict_Components.TryGetValue(index, out icp);

            if(icp !=null)
            {
                return true;
            }
            return false;
        }
        public bool HasComponents(int[] indices)
        {
            for (int i = 0; i < indices.Length; i++)
            {
                if (!HasComponent(indices[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public bool HasAnyComponent(int[] indices)
        {
            for (int i = 0; i < indices.Length; i++)
            {
                if (HasComponent(indices[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public T GetComponent<T>() where T : IComponent
        {
            int index = GetComponentIndex<T>();
            return (T)GetComponent(index);
        }
        public void ReplaceComponent<T>(T com) where T : IComponent
        {
            int index = GetComponentIndex<T>();
            ReplaceComponent(index, com);
        }
        public void RemoveComponent<T>() where T : IComponent
        {
            int index = GetComponentIndex<T>();
            RemoveComponent(index);
        }

        public void RemoveComponent(int index)
        {
            if (!HasComponent(index))
            {
                return ;
            }
            replaceComponent(index, null);
        }

        public void ReplaceComponent(int index, IComponent component)
        {
            if (HasComponent(index))
            {
                replaceComponent(index, component);
            }
            else if (component != null)
            {
                AddComponent(index, component);
            }
        }
        void replaceComponent(int index, IComponent replacement)
        {
            var previousComponent = GetComponent(index);
            if (replacement != previousComponent)
            {
                m_Dict_Components[index] = replacement;
                if (replacement != null)
                {
                    if (OnComponentReplaced != null)
                    {
                        OnComponentReplaced(
                            this, index, previousComponent, replacement
                        );
                    }
                }
                else
                {
                    if (OnComponentRemoved != null)
                    {
                        OnComponentRemoved(this, index, previousComponent);
                    }
                }
            }
            else
            {
                if (OnComponentReplaced != null)
                {
                    OnComponentReplaced(
                        this, index, previousComponent, replacement
                    );
                }
            }
        }

        /// Removes all components.
        public void RemoveAllComponents()
        {
            if(m_Dict_Components.Keys.Count <= 0)
            {
                return;
            }

            var Tmpkeys = new int[m_Dict_Components.Keys.Count];
            m_Dict_Components.Keys.CopyTo(Tmpkeys, 0);

            for (int i = 0; i < Tmpkeys.Length; i++)
            {
                replaceComponent(Tmpkeys[i], null);
            }
        }
        
        public virtual void Dispose()
        {
            if(OnDisposeEntity!=null)
            {
                OnDisposeEntity(this);
            }
            
            RemoveAllComponents();
            RemoveAllEventHandlers();


        }
        public void RemoveAllEventHandlers()
        {
            OnComponentRemoved = null;
            OnComponentAdded = null;
            OnComponentReplaced = null;

            OnInitEntity = null;
            OnCreateEntity = null;
            OnDestroyEntity = null;
            OnDisposeEntity = null;
        }

        public void Create()
        {
            if (OnCreateEntity != null)
            {
                OnCreateEntity(this);
            }
        }

        public void Destroy()
        {


            if (OnDestroyEntity != null)
            {
                OnDestroyEntity(this);
            }
            RemoveAllComponents();
            RemoveAllEventHandlers();
        }

        public virtual void Init(IContext context)
        {
            m_Context = context;
            if(OnInitEntity!=null)
            {
                OnInitEntity(this);
            }
        }
    }
}
