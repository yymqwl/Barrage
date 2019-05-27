using System;
using System.Collections.Generic;
using System.Text;

namespace GameMain
{
    public interface IGameMainEntry
    {
        IGameModuleManager GameModuleManager { get; }
        void Entry(string[] args);

        bool IsLoop { get; set; }
    }
}
