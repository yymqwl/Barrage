using System;
namespace GameFramework
{
    public interface IMHandler
    {
        void Handle(Session session, IMessage message);
        Type GetMessageType();
    }
}
