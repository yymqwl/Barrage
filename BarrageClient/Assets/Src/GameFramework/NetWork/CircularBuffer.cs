using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace GameFramework
{
    public class CircularBuffer : Stream
    {
        public int ChunkSize = 8192;

        private readonly Queue<byte[]> m_BufferQueue = new Queue<byte[]>();

        private readonly Queue<byte[]> m_BufferCache = new Queue<byte[]>();

        public int LastIndex { get; set; }

        public int FirstIndex { get; set; }

        private byte[] lastBuffer;

        public CircularBuffer()
        {
            ChunkSize = NetWorkConstant.CircularBuffer_ChunkSize;
            this.AddLast();
        }

        public override long Length
        {
            get
            {
                int c = 0;
                if (this.m_BufferQueue.Count == 0)
                {
                    c = 0;
                }
                else
                {
                    c = (this.m_BufferQueue.Count - 1) * ChunkSize + this.LastIndex - this.FirstIndex;
                }
                if (c < 0)
                {
                    Log.Error("CircularBuffer count < 0: {0}, {1}, {2}".Fmt(this.m_BufferQueue.Count, this.LastIndex, this.FirstIndex));
                }
                return c;
            }
        }

        public void AddLast()
        {
            byte[] buffer;
            if (this.m_BufferCache.Count > 0)
            {
                buffer = this.m_BufferCache.Dequeue();
            }
            else
            {
                buffer = new byte[ChunkSize];
            }
            this.m_BufferQueue.Enqueue(buffer);
            this.lastBuffer = buffer;
        }

        public void RemoveFirst()
        {
            this.m_BufferCache.Enqueue(m_BufferQueue.Dequeue());
        }

        public byte[] First
        {
            get
            {
                if (this.m_BufferQueue.Count == 0)
                {
                    this.AddLast();
                }
                return this.m_BufferQueue.Peek();
            }
        }

        public byte[] Last
        {
            get
            {
                if (this.m_BufferQueue.Count == 0)
                {
                    this.AddLast();
                }
                return this.lastBuffer;
            }
        }

        /// <summary>
        /// 从CircularBuffer读到stream中
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task ReadAsync(Stream stream)
        {
            long buffLength = this.Length;
            int sendSize = this.ChunkSize - this.FirstIndex;
            if (sendSize > buffLength)
            {
                sendSize = (int)buffLength;
            }

            await stream.WriteAsync(this.First, this.FirstIndex, sendSize);

            this.FirstIndex += sendSize;
            if (this.FirstIndex == this.ChunkSize)
            {
                this.FirstIndex = 0;
                this.RemoveFirst();
            }
        }

        // 从CircularBuffer读到stream
        public void Read(Stream stream, int count)
        {
            if (count > this.Length)
            {
                throw new Exception($"bufferList length < count, {Length} {count}");
            }

            int alreadyCopyCount = 0;
            while (alreadyCopyCount < count)
            {
                int n = count - alreadyCopyCount;
                if (ChunkSize - this.FirstIndex > n)
                {
                    stream.Write(this.First, this.FirstIndex, n);
                    this.FirstIndex += n;
                    alreadyCopyCount += n;
                }
                else
                {
                    stream.Write(this.First, this.FirstIndex, ChunkSize - this.FirstIndex);
                    alreadyCopyCount += ChunkSize - this.FirstIndex;
                    this.FirstIndex = 0;
                    this.RemoveFirst();
                }
            }
        }

        // 从stream写入CircularBuffer
        public void Write(Stream stream)
        {
            int count = (int)(stream.Length - stream.Position);

            int alreadyCopyCount = 0;
            while (alreadyCopyCount < count)
            {
                if (this.LastIndex == ChunkSize)
                {
                    this.AddLast();
                    this.LastIndex = 0;
                }

                int n = count - alreadyCopyCount;
                if (ChunkSize - this.LastIndex > n)
                {
                    stream.Read(this.lastBuffer, this.LastIndex, n);
                    this.LastIndex += count - alreadyCopyCount;
                    alreadyCopyCount += n;
                }
                else
                {
                    stream.Read(this.lastBuffer, this.LastIndex, ChunkSize - this.LastIndex);
                    alreadyCopyCount += ChunkSize - this.LastIndex;
                    this.LastIndex = ChunkSize;
                }
            }
        }


        /// <summary>
        ///  从stream写入CircularBuffer
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<int> WriteAsync(Stream stream)
        {
            int size = this.ChunkSize - this.LastIndex;

            int n = await stream.ReadAsync(this.Last, this.LastIndex, size);

            if (n == 0)
            {
                return 0;
            }

            this.LastIndex += n;

            if (this.LastIndex == this.ChunkSize)
            {
                this.AddLast();
                this.LastIndex = 0;
            }

            return n;
        }

        // 把CircularBuffer中数据写入buffer
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer.Length < offset + count)
            {
                throw new Exception($"bufferList length < coutn, buffer length: {buffer.Length} {offset} {count}");
            }

            long length = this.Length;
            if (length < count)
            {
                count = (int)length;
            }

            int alreadyCopyCount = 0;
            while (alreadyCopyCount < count)
            {
                int n = count - alreadyCopyCount;
                if (ChunkSize - this.FirstIndex > n)
                {
                    Array.Copy(this.First, this.FirstIndex, buffer, alreadyCopyCount + offset, n);
                    this.FirstIndex += n;
                    alreadyCopyCount += n;
                }
                else
                {
                    Array.Copy(this.First, this.FirstIndex, buffer, alreadyCopyCount + offset, ChunkSize - this.FirstIndex);
                    alreadyCopyCount += ChunkSize - this.FirstIndex;
                    this.FirstIndex = 0;
                    this.RemoveFirst();
                }
            }

            return count;
        }

        // 把buffer写入CircularBuffer中
        public override void Write(byte[] buffer, int offset, int count)
        {
            int alreadyCopyCount = 0;
            while (alreadyCopyCount < count)
            {
                if (this.LastIndex == ChunkSize)
                {
                    this.AddLast();
                    this.LastIndex = 0;
                }

                int n = count - alreadyCopyCount;
                if (ChunkSize - this.LastIndex > n)
                {
                    Array.Copy(buffer, alreadyCopyCount + offset, this.lastBuffer, this.LastIndex, n);
                    this.LastIndex += count - alreadyCopyCount;
                    alreadyCopyCount += n;
                }
                else
                {
                    Array.Copy(buffer, alreadyCopyCount + offset, this.lastBuffer, this.LastIndex, ChunkSize - this.LastIndex);
                    alreadyCopyCount += ChunkSize - this.LastIndex;
                    this.LastIndex = ChunkSize;
                }
            }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override long Position { get; set; }
    }
}