using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using System.Threading;

namespace RoomServer
{
    [GameFrameworkModule()]
    public class ConsoleModule : GameFrameworkModule
    {
        public override int Priority => 1000;


        public override bool Init()
        {
            ConsoleEntry();

            return base.Init();
        }
        public IGameMainEntry IGameMainEntry
        {
            get
            {
               return  GameMainEntry.Instance;
            }
        }
        //public CancellationTokenSource m_CTS;
        public async void ConsoleEntry()
        {

            Console.WriteLine("ConsoleModule");
            while(true)
            {
                try
                {
                    string str_line = await Task.Factory.StartNew(() =>
                      {
                          return Console.In.ReadLine();

                      });
                    str_line = str_line.Trim();
                    string[] str_lines = str_line.Split("/");

                    Console_Command console_Command = new Console_Command();
                    console_Command.CommandType = str_lines[0];
                    for (int i = 1; i < str_lines.Length; ++i)
                    {
                        console_Command.Params.Add(str_lines[i]);
                    }
                    //Log.Debug($"{console_Command.Params.Count}");
                    Parse(console_Command);
 
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public virtual void Parse(Console_Command cmd)
        {
            switch (cmd.CommandType)
            {
                case "quit":
                    IGameMainEntry.IsLoop = false;
                    break;
                default: break;
            }
        }

        public override void Update()
        {

        }
    }
}
