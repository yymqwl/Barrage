using System;
using System.Collections.Generic;
using System.Text;
using IHall;
namespace ClientApp
{
    public class MainEntryView : IMainEntry_Obs
    {
        
        public void Handle(string msg)
        {
            Console.WriteLine($"Handle:{msg}");
        }
    }
}
