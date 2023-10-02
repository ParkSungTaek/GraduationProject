/******
작성자 : 이우열
작성 일자 : 23.10.01

최근 수정 일자 : 23.10.01
최근 수정 내용 : 로그인 서버 <-> 클라 간 패킷
 ******/

using ServerCore;
using System.Text;
using System;

namespace LoginServer.Packet
{
    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 회원가입 패킷
    /// </summary>
    class CTL_Regist : IPacket
    {
        public string email;
        public string password;

        public ushort Protocol => (ushort)PacketID.CTL_Regist;

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

            strLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            password = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, strLen);
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

            //password
            strLen = (ushort)Encoding.Unicode.GetBytes(password, 0, password.Length, segment.Array, segment.Offset + count + sizeof(ushort));
            Array.Copy(BitConverter.GetBytes(strLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            count += strLen;

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 로그인 패킷
    /// </summary>
    class CTL_Login : IPacket
    {
        public string email;
        public string password;

        public ushort Protocol => (ushort)PacketID.CTL_Login;

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

            strLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            password = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, strLen);
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

            //password
            strLen = (ushort)Encoding.Unicode.GetBytes(password, 0, password.Length, segment.Array, segment.Offset + count + sizeof(ushort));
            Array.Copy(BitConverter.GetBytes(strLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            count += strLen;

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 회원가입 결과 반환
    /// </summary>
    public class LTC_RegistAck : IPacket
    {
        public bool isSuccess;
        public ushort Protocol => (ushort)PacketID.LTC_RegistAck;


        public void Read(ArraySegment<byte> segment)
        {
            int count = 0;

            //packet size
            count += sizeof(ushort);

            //protocol
            count += sizeof(ushort);

            isSuccess = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
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
            Array.Copy(BitConverter.GetBytes(isSuccess), 0, segment.Array, segment.Offset + count, sizeof(bool));
            count += sizeof(bool);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 로그인 결과 반환
    /// </summary>
    public class LTC_LoginAck : IPacket
    {
        public bool isSuccess;
        public ushort Protocol => (ushort)PacketID.LTC_LoginAck;


        public void Read(ArraySegment<byte> segment)
        {
            int count = 0;

            //packet size
            count += sizeof(ushort);

            //protocol
            count += sizeof(ushort);

            isSuccess = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
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
            Array.Copy(BitConverter.GetBytes(isSuccess), 0, segment.Array, segment.Offset + count, sizeof(bool));
            count += sizeof(bool);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
}
