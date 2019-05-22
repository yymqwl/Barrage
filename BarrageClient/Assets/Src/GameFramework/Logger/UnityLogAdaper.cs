using System;
#if !SERVER
using UnityEngine;
namespace GameFramework
{
    public class UnityLogAdaper : ALogDecorater, ILog
    {

        public UnityLogAdaper(ALogDecorater decorater = null) : base(decorater)
        {
        }

        public void Trace(string message)
        {
            UnityEngine.Debug.Log(message);
            //WriteLine(message);
        }

        public void Warning(string message)
        {
            UnityEngine.Debug.LogWarning(message);
            //WriteLine(message, ConsoleColor.Yellow);
        }

        public void Info(string message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        public void Debug(string message)
        {
            UnityEngine.Debug.Log(message);
        }

        public void Error(string message)
        {
            UnityEngine.Debug.LogError(message);
        }

        public void Fatal(string message)
        {
            UnityEngine.Debug.LogError(message);
        }


    }
}
#endif