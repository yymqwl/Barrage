using System;
using System.Threading.Tasks;
using IMySql;
using Orleans;

namespace MysqlGrains
{
    public class MysqlEntryGrain : Grain, IMysqlEntry
    {

        private int m_Count = 0;
        public override Task OnActivateAsync()
        {
            Console.WriteLine($"{typeof(MysqlEntryGrain)}OnActivateAsync");
            //RegisterTimer(Update_Timer, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            return base.OnActivateAsync();
        }
        public override Task OnDeactivateAsync()
        {
            Console.WriteLine($"{typeof(MysqlEntryGrain)}OnDeactivateAsync");
            return base.OnDeactivateAsync();
        }
        public  Task Update_Timer(object obj)
        {
            m_Count++;
            Console.WriteLine($"Update_Timer{m_Count}");
            return Task.CompletedTask;
        }

        public Task<string> GetName()
        {
            string Str_Name = "Mysql:Hello";
            Console.WriteLine($"InMysql{Str_Name}");
            return Task.FromResult(Str_Name);
        }
    }
}
