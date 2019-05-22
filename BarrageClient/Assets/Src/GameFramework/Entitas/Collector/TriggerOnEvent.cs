using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public struct TriggerOnEvent<TEntity> where TEntity :  Entity
    {

        public readonly IMatcher<TEntity> matcher;
        public readonly GroupEvent groupEvent;

        public TriggerOnEvent(IMatcher<TEntity> matcher, GroupEvent groupEvent)
        {
            this.matcher = matcher;
            this.groupEvent = groupEvent;
        }
    }
}
