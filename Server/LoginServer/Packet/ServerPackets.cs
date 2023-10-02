/******
작성자 : 이우열
작성 일자 : 23.10.01

최근 수정 일자 : 23.10.01
최근 수정 내용 : 로그인 서버 <-> 게임 서버 간 패킷
 ******/

using ServerCore;
using System.Text;
using System;

namespace LoginServer.Packet
{
    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 서버로 로그인 회원 전송
    /// </summary>
    class LTS_Auth : IPacket
    {
        public string email;

        public ushort Protocol => (ushort)PacketID.LTS_Auth;

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
    /// 작성자 : 이우열 <br/>
    /// 서버에서 로그인 회원 확인 완료
    /// </summary>
    class STL_AuthAck : IPacket
    {
        public string email;

        public ushort Protocol => (ushort)PacketID.STL_AuthAck;

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
}
