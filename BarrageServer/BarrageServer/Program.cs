using GameMain;
using System;
namespace BarrageServer
{
    public class Program
    {

        static void Main(string[] args)
        {
            TestGameEntry testGameEntry = new TestGameEntry();


            testGameEntry.Main(args);
            Console.Read();
  
        }
    }
}
