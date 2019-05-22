using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public static class CollectorContextExtension
    {
        
        public static Collector<IEntity> CreateCollector<IEntity>(
            this Context<IEntity> context,IMatcher<IEntity> macher,GroupEvent groupEvent
            )where IEntity:Entity
        {

            return context.CreateCollector(new TriggerOnEvent<IEntity>(macher, groupEvent) );
        }
        
        public static Collector<IEntity> CreateCollector<IEntity>(
            this Context<IEntity> context, params TriggerOnEvent<IEntity>[] triggers) where IEntity :  Entity
        {

            var groups = new Group<IEntity>[triggers.Length];
            var groupEvents = new GroupEvent[triggers.Length];

            for (int i = 0; i < triggers.Length; i++)
            {
                groups[i] = context.GetGroup(triggers[i].matcher);
                groupEvents[i] = triggers[i].groupEvent;
            }

            return new Collector<IEntity>(groups, groupEvents);
        }
    }
}
