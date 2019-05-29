using GameFramework;
using GameMain.Msg;

namespace GameMain
{
    [MessageHandler]
    public class Ping_Handler : AMHandler<Ping_Msg>
    {
        protected override void Run(Session session, Ping_Msg message)
        {
            session.Send(message);
        }
    }
}
