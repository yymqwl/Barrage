using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using CommandLine;
namespace GameMain
{
    [GameFrameworkModule()]
    public class ConsoleModule : GameFrameworkModule
    {
        public override int Priority => 1000;


        public override bool Init()
        {
            Task.Factory.StartNew( () =>
            {
                while(true)
                {
                    string str_line = Console.In.ReadLine();
                    string[] str_lines = str_line.Split("/");
                    if(str_lines.Length<1)
                    {
                        Log.Error("格式错误"); ;
                    }
                    Console_Command console_Command = new Console_Command();
                    console_Command.CommandType = str_lines[0];
                    for(int i=1;i< str_lines.Length;++i)
                    {
                        console_Command.Params.Add(str_lines[i]);
                    }
                    Log.Debug($"{console_Command.Params.Count}");
                }
            });
            return base.Init();
        }
        public override void Update()
        {

        }
    }
}
