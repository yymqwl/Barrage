﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mcpp
{
    public interface IGameMainEntry
    {
        void Entry(string[] args);

        bool IsLoop { get; set; }
    }
}
