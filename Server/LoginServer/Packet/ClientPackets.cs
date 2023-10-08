/******
작성자 : 이우열
작성 일자 : 23.10.01

최근 수정 일자 : 23.10.03
최근 수정 내용 : 메일 인증 패킷 추가
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
    /// 디버깅 용 메일 인증 없는 회원가입
    /// </summary>
    class CTL_ForceRegist : IPacket
    {
        public string email;
        public string password;

        public ushort Protocol => (ushort)PacketID.CTL_ForceRegist;

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
    /// 회원가입 메일 인증 패킷
    /// </summary>
    class CTL_RegistAuth : IPacket
    {
        public string email;
        public int authNo;

        public ushort Protocol => (ushort)PacketID.CTL_RegistAuth;

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

            authNo = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);
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
            Array.Copy(BitConverter.GetBytes(authNo), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);

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
        public enum ErrorCode
        {
            Success,
            Duplicate,
            MailError,
        }

        public ushort errorCode;
        public ushort Protocol => (ushort)PacketID.LTC_RegistAck;


        public void Read(ArraySegment<byte> segment)
        {
            int count = 0;

            //packet size
            count += sizeof(ushort);

            //protocol
            count += sizeof(ushort);

            errorCode = BitConverter.ToUInt16(segment.Array,segment.Offset + count);
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
            Array.Copy(BitConverter.GetBytes(errorCode), 0, segment.Array, segment.Offset+count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 메일 인증 결과 반환
    /// </summary>
    public class LTC_RegistAuthAck : IPacket
    {
        public enum ErrorCode
        {
            Success,
            DBError,
            WrongCode,
            Expired,
        }

        public ushort errorCode;
        public ushort Protocol => (ushort)PacketID.LTC_RegistAuthAck;


        public void Read(ArraySegment<byte> segment)
        {
            int count = 0;

            //packet size
            count += sizeof(ushort);

            //protocol
            count += sizeof(ushort);

            errorCode = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
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
            Array.Copy(BitConverter.GetBytes(errorCode), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

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
