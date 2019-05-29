
namespace GameFramework
{
    public struct MessageInfo
    {
        public ushort Opcode { get; }
        public IMessage Message { get; }

        public MessageInfo(ushort opcode, IMessage message)
        {
            this.Opcode = opcode;
            this.Message = message;
        }
    }
}
