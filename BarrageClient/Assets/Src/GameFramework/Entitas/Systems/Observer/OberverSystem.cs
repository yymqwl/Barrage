using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    /*

    public delegate void ObserverChange<IEntity, IC>(IEntity e, IC ic) where IEntity : Entity where IC : IComponent;

    public abstract class OberverSystem<IEntity,IC> : ReactiveSystem<IEntity> where IEntity : Entity where IC: IComponent 
    {

        public event ObserverChange<IEntity, IC> OnObserverChange;

        protected OberverSystem(Context<IEntity> context) : base(context)
        {
            
        }

        public override void Activate()
        {
            base.Activate();
        }
        public override void Deactivate()
        {
            base.Deactivate();
            RemoveAllEventHandlers();
        }
        public void RemoveAllEventHandlers()
        {
            OnObserverChange = null;
        }
        protected override void Execute(List<IEntity> entities)
        {
            foreach (var e in entities)
            {
                if(OnObserverChange!=null)
                {

                    var ic = e.GetComponent<IC>();
                    if (ic != null)
                    {
                        OnObserverChange(e, ic);
                    }
                    else
                    {
                        Log.Error("ic get null");
                    }
                }
            }
        }
        protected override bool Filter(IEntity entity)
        {
            return true;
        }
        protected override Collector<IEntity> GetTrigger(Context<IEntity> context)
        {
            return context.CreateCollector(new TriggerOnEvent<IEntity>(Matcher<IEntity>.AnyOf(context.ContextInfo.GetComponentIndex<IC>()), GroupEvent.Added));
        }
    }*/
}
