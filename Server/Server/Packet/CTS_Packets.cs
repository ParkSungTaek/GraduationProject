/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.10.02
최근 수정 내용 : 로그인 인증 패킷 추가
 ******/

using ServerCore;
using System;
using System.Text;

namespace Server
{
    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 로그인 패킷
    /// </summary>
    public class CTS_Auth : IPacket
    {
        public string email;
        public ushort Protocol => (ushort)PacketID.CTS_Auth;

        public void Read(ArraySegment<byte> segment)
        {

            int count = 0;
            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            ushort strLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);
            email = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, strLen);
            count += strLen;
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            //email
            ushort strLen = (ushort)Encoding.Unicode.GetBytes(email, 0, email.Length, segment.Array, segment.Offset + count + sizeof(ushort));
            Array.Copy(BitConverter.GetBytes(strLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            count += strLen;

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

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
    /// 작성자 : 박성택<br/>
    /// 빠른 입장 패킷
    /// </summary>
    public class CTS_QuickEnter : SimplePacket
    {
        public override ushort Protocol => (ushort)PacketID.CTS_QuickEnter;
    }

    /// <summary>
    /// 작성자 : 박성택 <br/>
    /// 공개 방 목록 요청
    /// </summary>
    public class CTS_GetPublicRoomList : SimplePacket
    {
        public override ushort Protocol => (ushort)PacketID.CTS_GetPublicRoomList;
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
    public class CTS_LeaveRoom : SimplePacket
    {
        public override ushort Protocol => (ushort)PacketID.CTS_LeaveRoom;
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 로비 -> 클래스 선택 전환 패킷, 방장만 보낼 수 있음
    /// </summary>
    public class CTS_ReadyGame : SimplePacket
    {
        public override ushort Protocol => (ushort)PacketID.CTS_ReadyGame;
    }

    /// <summary>
    /// 작성자 : 박성택 <br/>
    /// 방의 빠른 입장을 허용/불허 
    /// </summary>
    public class CTS_SetPublicRoom : IPacket
    {
        public bool isPublic;
        public ushort Protocol => (ushort)PacketID.CTS_SetPublicRoom;

        public void Read(ArraySegment<byte> segment)
        {
            int count = 0;
            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            isPublic = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
            count += sizeof(bool);
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(isPublic), 0, segment.Array, segment.Offset + count, sizeof(bool));
            count += sizeof(bool);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
}