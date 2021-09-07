﻿using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public interface IMainEntry : IAMainEntry
    {
        Task<IChatRoomEntry> GetIChatRoomEntry();
        Task<INetUserEntry>  GetINetUserEntry();

        Task<ITableRoomEntry> GetITableRoomEntry();

        Task<IMainEntry> GetInstance();
    }
}
