namespace GameFramework
{
    public interface IMessageDispatcher
    {
        void Dispatch(Session session, MessageInfo messageInfo);
    }
}
