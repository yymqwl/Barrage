using System;
using System.Threading.Tasks;
using IMySql;
using Orleans;

namespace MysqlGrains
{
    public class MysqlEntryGrain : Grain, IMysqlEntry
    {
        public override Task OnActivateAsync()
        {
            Console.WriteLine($"{typeof(MysqlEntryGrain)}OnActivateAsync");
            return base.OnActivateAsync();
        }
        public override Task OnDeactivateAsync()
        {
            Console.WriteLine($"{typeof(MysqlEntryGrain)}OnDeactivateAsync");
            return base.OnDeactivateAsync();
        }

        public Task<string> GetName()
        {
            string Str_Name = "Mysql:Hello";
            Console.WriteLine($"InMysql{Str_Name}");
            return Task.FromResult(Str_Name);
        }
    }
}
