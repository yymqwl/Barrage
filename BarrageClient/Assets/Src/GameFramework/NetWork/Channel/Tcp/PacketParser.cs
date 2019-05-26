using System;
using System.IO;

namespace GameFramework
{
    public enum ParserState
    {
        PacketSize,
        PacketBody
    }

    public static class Packet
    {
        public const int PacketSizeLength2 = 2;
        public const int PacketSizeLength4 = 4;
        public const int FlagIndex = 0;
        public const int OpcodeIndex = 1;
        public const int MessageIndex = 3;
    }

    public class PacketParser
    {
        private readonly CircularBuffer m_Buffer;
        private int m_PacketSize;
        private ParserState m_State;
        public MemoryStream m_MemoryStream;
        private bool m_IsOk;
        private readonly int m_PacketSizeLength;

        public PacketParser(int m_PacketSizeLength, CircularBuffer m_Buffer, MemoryStream m_MemoryStream)
        {
            this.m_PacketSizeLength = m_PacketSizeLength;
            this.m_Buffer = m_Buffer;
            this.m_MemoryStream = m_MemoryStream;
        }

        public bool Parse()
        {
            if (this.m_IsOk)
            {
                return true;
            }

            bool finish = false;
            while (!finish)
            {
                switch (this.m_State)
                {
                    case ParserState.PacketSize:
                        if (this.m_Buffer.Length < this.m_PacketSizeLength)
                        {
                            finish = true;
                        }
                        else
                        {
                            this.m_Buffer.Read(this.m_MemoryStream.GetBuffer(), 0, this.m_PacketSizeLength);

                            switch (this.m_PacketSizeLength)
                            {
                                case Packet.PacketSizeLength4:
                                    this.m_PacketSize = BitConverter.ToInt32(this.m_MemoryStream.GetBuffer(), 0);
                                    if (this.m_PacketSize > ushort.MaxValue * 16 || this.m_PacketSize < 3)
                                    {
                                        throw new Exception($"recv packet size error: {this.m_PacketSize}");
                                    }
                                    break;
                                case Packet.PacketSizeLength2:
                                    this.m_PacketSize = BitConverter.ToUInt16(this.m_MemoryStream.GetBuffer(), 0);
                                    if (this.m_PacketSize > ushort.MaxValue || this.m_PacketSize < 3)
                                    {
                                        throw new Exception($"recv packet size error: {this.m_PacketSize}");
                                    }
                                    break;
                                default:
                                    throw new Exception("packet size byte count must be 2 or 4!");
                            }

                            this.m_State = ParserState.PacketBody;
                        }
                        break;
                    case ParserState.PacketBody:
                        if (this.m_Buffer.Length < this.m_PacketSize)
                        {
                            finish = true;
                        }
                        else
                        {
                            this.m_MemoryStream.Seek(0, SeekOrigin.Begin);
                            this.m_MemoryStream.SetLength(this.m_PacketSize);
                            byte[] bytes = this.m_MemoryStream.GetBuffer();
                            this.m_Buffer.Read(bytes, 0, this.m_PacketSize);
                            this.m_IsOk = true;
                            this.m_State = ParserState.PacketSize;
                            finish = true;
                        }
                        break;
                }
            }
            return this.m_IsOk;
        }

        public MemoryStream GetPacket()
        {
            this.m_IsOk = false;
            return this.m_MemoryStream;
        }
    }
}