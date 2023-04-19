/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.04.19
최근 수정 내용 : 클래스 생성
 ******/

using System;
using System.Threading;

namespace ServerCore
{
    public class SendBufferHelper
    {
        /// <summary> 쓰레드 별 송신 버퍼 별개 존재, SendBuffer 초기값은 null </summary>
        public static ThreadLocal<SendBuffer> CurrentBuffer = new ThreadLocal<SendBuffer>(() => null);

        /// <summary> 수신 버퍼 기본 크기 - 인 당 65535 가정 </summary>
        public static int ChunkSize { get; set; } = 65535 * 4;

        /// <summary> 송신을 위한 공간 예약 </summary>
        /// <param name="reserveSize"> 송신을 위한 대략적인 크기 </param>
        public static ArraySegment<byte> Open(int reserveSize)
        {
            if (CurrentBuffer.Value == null)
                CurrentBuffer.Value = new SendBuffer(ChunkSize);
            if (CurrentBuffer.Value.FreeSize < reserveSize)
                CurrentBuffer.Value = new SendBuffer(ChunkSize);

            return CurrentBuffer.Value.Open(reserveSize);
        }

        /// <summary> 실제 송신 예약 완료 -> 공간 줄이기 </summary>
        /// <param name="usedSize"> 예약에 쓰인 크기 </param>
        public static ArraySegment<byte> Close(int usedSize)
        {
            return CurrentBuffer.Value.Close(usedSize);
        }
    }

    public class SendBuffer
    {
        /// <summary> 송신 버퍼 </summary>
        byte[] _buffer;
        /// <summary> 현재 사용 중인 공간 크기 </summary>
        int _usedSize = 0;
        /// <summary> 현재 사용 가능한 공간 크기 </summary>
        public int FreeSize { get => _buffer.Length - _usedSize; }

        public SendBuffer(int chunkSize)
        {
            _buffer = new byte[chunkSize];
        }

        /// <summary> 송신을 위한 공간 예약 </summary>
        /// <param name="reserveSize"> 송신을 위한 대략적인 크기 </param>
        public ArraySegment<byte> Open(int reserveSize)
        {
            if (reserveSize > FreeSize)
                return null;

            return new ArraySegment<byte>(_buffer, _usedSize, reserveSize);
        }

        /// <summary> 실제 송신 예약 완료 -> 공간 줄이기 </summary>
        /// <param name="usedSize"> 예약에 쓰인 크기 </param>
        public ArraySegment<byte> Close(int usedSize)
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(_buffer, _usedSize, usedSize);
            _usedSize += usedSize;
            return segment;
        }
    }
}