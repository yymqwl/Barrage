using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;

namespace GameMain.LockStep
{
    public class PlayerInput : ABehaviour
    {
        List<ISendCommand> m_List_ISendCommand = new List<ISendCommand>();

        public List<ISendCommand> List_ISendCommand
        {
            get
            {
                return m_List_ISendCommand;
            }
        }


        public override bool Init()
        {
            m_List_ISendCommand.Clear();
            return base.Init();
        }

        public override bool ShutDown()
        {
            this.m_List_ISendCommand.Clear();
            return base.ShutDown();
        }
        public void SendCommand(Command cmd)
        {
            foreach (var isc in m_List_ISendCommand)
            {
                isc.SendCommand(cmd);
            }
        }

        public override void Update()
        {
        }
    }
}
