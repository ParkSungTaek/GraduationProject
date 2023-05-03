/******
작성자 : 이우열
작성일 : 23.04.13

최근 수정 일자 : 23.04.19
최근 수정 내용 : PacketSession 클래스 구현
 ******/

using System;

namespace ServerCore
{
    public abstract class PacketSession : Session
    {
        public static readonly int HeaderSize = 2;

        /// <summary> 패킷 검사, 정상 패킷일 시 패킷 분석 </summary>
        public sealed override int OnRecv(ArraySegment<byte> buffer)
        {
            int processLen = 0;

            while (true)
            {
                if (buffer.Count < HeaderSize)
                    break;

                ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
                if (buffer.Count < dataSize)
                    break;

                OnRecvPacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));

                processLen += dataSize;
                buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
            }

            return processLen;
        }

        /// <summary> 패킷 분석 후 해당하는 핸들러 호출 </summary>
        public abstract void OnRecvPacket(ArraySegment<byte> buffer);
    }
}