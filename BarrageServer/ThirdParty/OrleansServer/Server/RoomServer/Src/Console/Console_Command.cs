using System.Collections.Generic;
//using System.Collections.Generic;

namespace RoomServer
{
    public class Console_Command
    {
        //[Option('c',"CommandType",Required =true,HelpText ="操作类型",Separator =' ')]
        public string CommandType { get; set; }

        //[Option('p', "Params",HelpText ="参数")]
        public List<string> Params { get; set; } = new List<string>();

    }
}
