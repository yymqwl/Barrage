using System.Collections;
using System.Collections.Generic;
namespace GameFramework
{
    public class ClientTimer : AGameTimer
    {

        public static ClientTimer Instance { get; } = new ClientTimer();
    }

}