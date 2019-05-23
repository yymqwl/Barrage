using System;
using System.Collections.Generic;
using System.Text;

namespace GameMain
{
    public interface IGameMainEntry
    {
        IGameModuleManager GameModuleManager { get; }
        void Main(string[] args);

        bool IsLoop { get; set; }
    }
}
