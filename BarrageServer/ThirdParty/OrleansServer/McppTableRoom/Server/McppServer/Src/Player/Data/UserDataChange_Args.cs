using GameFramework.Event;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mcpp
{
    public class UserDataChange_Args : GameEventArgs
    {
        public override int Id => (int)EData_Event_Type.EData_Change;
        public KeyValuePair<string, JToken?> m_Item;
        public static UserDataChange_Args Create(KeyValuePair<string, JToken?> item)
        {
            var args = new UserDataChange_Args();
            args.m_Item = item;
            return args;
        }
    }
}
