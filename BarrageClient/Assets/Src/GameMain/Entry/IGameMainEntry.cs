using System.Collections;
using System.Collections.Generic;
namespace GameMain
{
    public interface IGameMainEntry 
    {
        IGameModuleManager GameModuleManager { get; }
        void Main(string[] args);
    }
}