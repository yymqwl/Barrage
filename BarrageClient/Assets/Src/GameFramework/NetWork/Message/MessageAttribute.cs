namespace GameFramework
{
    public class MessageAttribute : BaseAttribute
    {
        public ushort Opcode { get; }

        public uint ServerType { get; }

        public MessageAttribute(ushort opcode,uint servertype = 0)
        {
            this.Opcode = opcode;
            this.ServerType = servertype;
        }
    }
}
