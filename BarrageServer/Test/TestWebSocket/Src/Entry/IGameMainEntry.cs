using System;
using System.Collections.Generic;
using System.Text;

namespace TestWebSocket
{
    public interface IGameMainEntry
    {
        void Entry(string[] args);

        bool IsLoop { get; set; }
    }
}
