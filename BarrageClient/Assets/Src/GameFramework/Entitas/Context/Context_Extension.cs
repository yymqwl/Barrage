using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public static class Context_Extension
    {
        
        public static Group<IEntity>  GetGroup<IEntity,IC>(this Context<IEntity>  context) where IEntity:Entity where IC:IComponent
        {
            return context.GetGroup(Matcher<IEntity>.AnyOf(context.ContextInfo.GetComponentIndex<IC>()));

        }
        public static IMatcher<IEntity> GetMatcher<IEntity, IC>(this Context<IEntity> context) where IEntity:Entity where IC:IComponent
        {
            return Matcher<IEntity>.AnyOf(context.ContextInfo.GetComponentIndex<IC>());
        }
    }
}
