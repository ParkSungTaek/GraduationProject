/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.04.19
최근 수정 내용 : 플레이어 이동, 방 생성, 방장 변경 패킷 추가
 ******/

using ServerCore;
using System;

namespace Server
{
    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 서버 -> 클라 방 입장 불가 : 방 없음
    /// </summary>
    public class STC_RejectEnter_Exist : IPacket
    {
        public ushort Protocol => (ushort)PacketID.STC_RejectEnter_Exist;

        public void Read(ArraySegment<byte> segment) { }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)PacketID.STC_RejectEnter_Exist), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 서버 -> 클라 방 입장 불가 : 가득찬 방
    /// </summary>
    public class STC_RejectEnter_Full : IPacket
    {
        public ushort Protocol => (ushort)PacketID.STC_RejectEnter_Full;

        public void Read(ArraySegment<byte> segment) { }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)PacketID.STC_RejectEnter_Full), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    public class STC_SuccessEnter : IPacket
    {
        public ushort Protocol => throw new NotImplementedException();

        public void Read(ArraySegment<byte> segment)
        {
            throw new NotImplementedException();
        }

        public ArraySegment<byte> Write()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 서버 -> 클라 방 생성 불가
    /// </summary>
    public class STC_RejectRoom : IPacket
    {
        public ushort Protocol => (ushort)PacketID.STC_RejectRoom;

        public void Read(ArraySegment<byte> segment) { }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)PacketID.STC_RejectRoom), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 서버 -> 클라 방장 변경 패킷
    /// </summary>
    public class STC_SetSuper : IPacket
    {
        public ushort Protocol => (ushort)PacketID.STC_SetSuper;

        public void Read(ArraySegment<byte> segment) { }
        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)PacketID.STC_SetSuper), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
}