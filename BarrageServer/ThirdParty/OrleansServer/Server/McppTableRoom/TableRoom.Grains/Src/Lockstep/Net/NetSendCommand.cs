using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;

namespace GameMain.LockStep
{
    public class NetSendCommand : ABehaviour, ISendCommand
    {
        public virtual void SendCommand(Command cmd)
        {

        }
    }
}
