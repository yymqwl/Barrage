using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IHall
{
    public interface IBank : IGrainWithIntegerKey
    {
        Task Mul(int id,int money);
        Task SetMoney(int money);
    }
}
