/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.10.02
최근 수정 내용 : 로그인 인증 결과 패킷 추가
 ******/

using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 로그인 결과 반환
    /// </summary>
    public class STC_AuthAck : IPacket
    {
        public bool isSuccess;
        public ushort Protocol => (ushort)PacketID.STC_AuthAck;

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
    /// 중복 로그인 알림
    /// </summary>
    public class STC_DuplicatedLogin : SimplePacket
    {
        public override ushort Protocol => (ushort)PacketID.STC_DuplicatedLogin;
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 주기적으로 연결 검사
    /// </summary>
    public class STC_CheckAlive : SimplePacket
    {
        public override ushort Protocol => (ushort)PacketID.STC_CheckAlive;
    }

    #region Create/Enter Room
    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 서버 -> 클라 방 생성 불가
    /// </summary>
    public class STC_RejectRoom : SimplePacket
    {
        public override ushort Protocol => (ushort)PacketID.STC_RejectRoom;
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 서버 -> 클라 방 입장 불가
    /// </summary>
    public class STC_RejectEnter : IPacket
    {
        public enum ErrorCode
        {
            NotExist,
            Full,
            Start,
        }

        public ushort Protocol => (ushort)PacketID.STC_RejectEnter;
        public ErrorCode errorCode;

        public void Read(ArraySegment<byte> segment)
        {
            int count = 0;

            //packet size
            count += sizeof(ushort);

            //protocol
            count += sizeof(ushort);

            errorCode = (ErrorCode)BitConverter.ToUInt16(segment.Array, segment.Offset + count);
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
            Array.Copy(BitConverter.GetBytes((ushort)errorCode), 0, segment.Array, segment.Offset + count, sizeof(bool));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 박성택 <br/>
    /// 빠른 입장 실패 알림
    /// </summary>
    public class STC_QuickEnterFail : SimplePacket
    {
        public override ushort Protocol => (ushort)PacketID.STC_QuickEnterFail;
    }

    /// <summary>
    /// 작성자 : 박성택 <br/>
    /// 새로 입장한 클라이언트에게 기존 존재 방 정보 알림
    /// </summary>
    public class STC_PublicRoomList : IPacket
    {
        public ushort Protocol => (ushort)PacketID.STC_PublicRoomList;
        public List<string> RoomNames = new List<string>();

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            ushort listLength = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);

            ushort strLen;
            for (ushort i = 0; i < listLength; i++)
            {
                strLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
                count += sizeof(ushort);
                string room = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, strLen);
                count += strLen;

                RoomNames.Add(room);
            }
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            //packet size
            count += sizeof(ushort);

            //protocol
            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)RoomNames.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            ushort strLen;
            for (int i = 0; i < RoomNames.Count; i++)
            {
                strLen = (ushort)Encoding.Unicode.GetBytes(RoomNames[i], 0, RoomNames[i].Length, segment.Array, segment.Offset + count + sizeof(ushort));
                Array.Copy(BitConverter.GetBytes(strLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
                count += sizeof(ushort);
                count += strLen;
            }

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 서버 -> 방 내 모든 클라 새로운 플레이어 입장 알림
    /// </summary>
    public class STC_PlayerEnter : IPacket
    {
        public int playerId;
        public string email;
        public ushort Protocol => (ushort)PacketID.STC_PlayerEnter;

        public void Read(ArraySegment<byte> segment)
        {
            int count = 0;

            //packet size
            count += sizeof(ushort);

            //protocol
            count += sizeof(ushort);

            playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
            count += sizeof(int);

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
            Array.Copy(BitConverter.GetBytes(playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);

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
    /// 서버 -> 방 내 모든 클라 새로운 플레이어 입장 알림
    /// </summary>
    public class STC_PlayerLeave : IPacket
    {
        public int playerId;
        public ushort Protocol => (ushort)PacketID.STC_PlayerLeave;

        public void Read(ArraySegment<byte> segment)
        {
            int count = 0;

            //packet size
            count += sizeof(ushort);

            //protocol
            count += sizeof(ushort);

            playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
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
            Array.Copy(BitConverter.GetBytes(playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
            count += sizeof(int);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 새로 입장한 클라이언트에게 기존 존재 클라이언트 정보 알림
    /// </summary>
    public class STC_ExistPlayers : IPacket
    {
        public class PlayerInfo
        {
            public int playerId;
            public string email;
        }

        public ushort Protocol => (ushort)PacketID.STC_ExistPlayers;
        public bool IsPublicRoom;
        public List<PlayerInfo> Players = new List<PlayerInfo>();

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            IsPublicRoom = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
            count += sizeof(bool);

            ushort listLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);

            for (ushort i = 0; i < listLen; i++)
            {
                PlayerInfo info = new PlayerInfo();
                info.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
                count += sizeof(int);

                ushort strLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
                count += sizeof(ushort);
                info.email = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, strLen);
                count += strLen;

                Players.Add(info);
            }
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(IsPublicRoom), 0, segment.Array, segment.Offset + count, sizeof(bool));
            count += sizeof(bool);

            Array.Copy(BitConverter.GetBytes((ushort)Players.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            for (int i = 0; i < Players.Count; i++)
            {
                Array.Copy(BitConverter.GetBytes(Players[i].playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
                count += sizeof(int);

                ushort strLen = (ushort)Encoding.Unicode.GetBytes(Players[i].email, 0, Players[i].email.Length, segment.Array, segment.Offset + count + sizeof(ushort));
                Array.Copy(BitConverter.GetBytes(strLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
                count += sizeof(ushort);
                count += strLen;
            }

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
    #endregion Create/Enter Room

    #region Lobby
    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// Host 퇴장 시, Host 변경 패킷
    /// </summary>
    public class STC_SetSuper : SimplePacket
    {
        public override ushort Protocol => (ushort)PacketID.STC_SetSuper;
    }

    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 방장의 로비 -> 캐릭터 선택 씬 전환 요구 브로드캐스팅
    /// </summary>
    public class STC_ReadyGame : SimplePacket
    {
        public override ushort Protocol => (ushort)PacketID.STC_ReadyGame;
    }

    /// <summary> 공개 방으로 설정됨을 모든 플레이어게 알림 </summary>
    public class STC_SetPublicRoomAck : SimplePacket
    {
        public override ushort Protocol => (ushort)PacketID.STC_SetPublicRoomAck;
    }
    #endregion Lobby
}