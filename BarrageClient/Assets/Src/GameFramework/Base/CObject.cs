using System;
using System.ComponentModel;

namespace GameFramework
{
    public abstract class CObject : IDisposable
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {

        }
        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Dispose()
        {
        }

    }
}