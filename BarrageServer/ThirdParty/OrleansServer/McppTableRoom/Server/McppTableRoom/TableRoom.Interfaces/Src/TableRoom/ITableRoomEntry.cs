using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public interface ITableRoomEntry :IEntry
    {
        Task<int> Join(TableUser_Data user);

        Task<int> Exit(TableUser_Data user);

    }
}
