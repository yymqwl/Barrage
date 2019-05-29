//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework
{
    /// <summary>
    /// 变量。
    /// </summary>
    public abstract class Variable
    {
        /// <summary>
        /// 初始化变量的新实例。
        /// </summary>
        protected Variable()
        {

        }

        /// <summary>
        /// 获取变量类型。
        /// </summary>
        public abstract Type Type
        {
            get;
        }

        /// <summary>
        /// 重置变量值。
        /// </summary>
        public abstract void Reset();
    }
}
