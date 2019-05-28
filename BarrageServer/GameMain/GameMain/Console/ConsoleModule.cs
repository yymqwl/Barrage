using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using CommandLine;
using System.Threading;

namespace GameMain
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

        //public CancellationTokenSource m_CTS;
        public async void ConsoleEntry()
        {
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
                    switch (console_Command.CommandType)
                    {
                        case "quit":
                            TestGameEntry.Instance.IsLoop = false;
                            break;
                        default: break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            /*
            while (true)
            {
                string str_line = Console.In.ReadLine();
                string[] str_lines = str_line.Split("/");
                if (str_lines.Length < 1)
                {
                    Log.Error("格式错误"); ;
                }
                Console_Command console_Command = new Console_Command();
                console_Command.CommandType = str_lines[0];
                for (int i = 1; i < str_lines.Length; ++i)
                {
                    console_Command.Params.Add(str_lines[i]);
                }
                //Log.Debug($"{console_Command.Params.Count}");
                switch (console_Command.CommandType)
                {
                    case "quit":
                        TestGameEntry.Instance.IsLoop = false;
                        break;
                    default: break;
                }
            }*/
        }

        public override void Update()
        {

        }
    }
}
