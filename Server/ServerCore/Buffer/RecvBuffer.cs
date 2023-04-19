/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.04.19
최근 수정 내용 : 클래스 생성
 ******/

using System;

namespace ServerCore
{
    /// <summary> 세션 당 가지고 있는 수신 버퍼 </summary>
    public class RecvBuffer
    {
        ArraySegment<byte> _buffer;
        /// <summary> 아직 읽지 않은 데이터 시작 index </summary>
        int _readPos;
        /// <summary> 새로운 데이터 수신 시 쓰기 시작할 index </summary>
        int _writePos;

        public RecvBuffer(int bufferSize)
        {
            _buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
        }

        /// <summary> 수신했지만 읽지 않은 데이터 크기 </summary>
        public int DataSize { get => _writePos - _readPos; }
        /// <summary> 새로 수신 가능한 공간 크기 </summary>
        public int FreeSize { get => _buffer.Count - _writePos; }

        /// <summary> 수신했지만 읽지 않은 데이터들 </summary>
        public ArraySegment<byte> ReadSegment
        {
            get => new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _readPos, DataSize);
        }

        /// <summary> 새롭게 수신할 수 있는 공간 </summary>
        public ArraySegment<byte> WriteSegment
        {
            get => new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _writePos, FreeSize);
        }

        /// <summary> read/write position 정리 </summary>
        public void Clean()
        {
            int dataSize = DataSize;
            
            //읽지 않은 데이터 없음 -> 위치 0으로 초기화
            if(dataSize == 0)
                _readPos = _writePos = 0;
            //아직 읽지 않은 데이터 있음 -> 앞으로 땡겨오기
            else
            {
                Array.Copy(_buffer.Array, _buffer.Offset + _readPos, _buffer.Array, _buffer.Offset, dataSize);
                _readPos = 0;
                _writePos = dataSize;
            }
        }

        /// <summary> 데이터 읽기 수행 완료 시 호출 </summary>
        /// <param name="numOfBytes">읽은 데이터 길이</param>
        public bool OnRead(int numOfBytes)
        {
            if (numOfBytes > DataSize)
                return false;

            _readPos += numOfBytes;
            return true;
        }
        /// <summary> 데이터 쓰기 수행 완료 시 호출 </summary>
        /// <param name="numOfBytes">쓴 데이터 길이</param>
        public bool OnWrite(int numOfBytes)
        {
            if (numOfBytes > FreeSize)
                return false;

            _writePos += numOfBytes;
            return true;
        }
    }
}