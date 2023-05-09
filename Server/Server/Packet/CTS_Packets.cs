/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.05.09
최근 수정 내용 : 로비 -> 캐릭터 선택 전환(ReadyGame) 패킷 추가
 ******/

using ServerCore;
using System;
using System.Text;

namespace Server
{
    //TODO : CTS_Login

    /// <summary>
    /// 작성자 : 이우열<br/>
    /// 방 생성 패킷
    /// </summary>
    public class CTS_CreateRoom : IPacket
    {
        public string roomName;

        public ushort Protocol => (ushort)PacketID.CTS_CreateRoom;

        public void Read(ArraySegment<byte> segment)
        {
            int count = 0;
            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            ushort nameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
            roomName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, nameLen);
            count += nameLen;
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            ushort nameLen = (ushort)Encoding.Unicode.GetBytes(roomName, 0, roomName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
            Array.Copy(BitConverter.GetBytes(nameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            count += nameLen;

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 이우열<br/>
    /// 방 입장 패킷
    /// </summary>
    public class CTS_EnterRoom : IPacket
    {
        public string roomName;

        public ushort Protocol => (ushort)PacketID.CTS_EnterRoom;

        public void Read(ArraySegment<byte> segment)
        {
            int count = 0;
            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            ushort nameLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
            roomName = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, nameLen);
            count += nameLen;
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            ushort nameLen = (ushort)Encoding.Unicode.GetBytes(roomName, 0, roomName.Length, segment.Array, segment.Offset + count + sizeof(ushort));
            Array.Copy(BitConverter.GetBytes(nameLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            count += nameLen;

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 이우열<br/>
    /// 방 퇴장 패킷
    /// </summary>
    public class CTS_LeaveRoom : IPacket
    {
        public ushort Protocol => (ushort)PacketID.CTS_LeaveRoom;

        public void Read(ArraySegment<byte> segment)
        {
            int count = 0;
            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 로비 -> 클래스 선택 전환 패킷, 방장만 보낼 수 있음
    /// </summary>
    public class CTS_ReadyGame : IPacket
    {
        public ushort Protocol { get => (ushort)PacketID.CTS_ReadyGame; }

        public void Read(ArraySegment<byte> segment)
        {

        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
}