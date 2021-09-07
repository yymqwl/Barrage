using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using IHall;
using System.Threading.Tasks;
using GameFramework;

namespace HallGrains
{
    public class BankGrain : Grain, IBank
    {
        protected int m_TotalMoney;
        public Task Mul(int id, int money)
        {
            if (m_TotalMoney >= money)
            {
                m_TotalMoney -= money;
                Log.Debug($"TotalMoney:{m_TotalMoney}");
            }
            return Task.CompletedTask;
        }

        public Task SetMoney(int money)
        {
            m_TotalMoney = money;
            return Task.CompletedTask;
        }
    }
}
