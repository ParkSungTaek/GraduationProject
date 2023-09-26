/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.05.09
최근 수정 내용 : 방에 이미 존재하는 플레이어 정보 공유(ExistPlayers) 패킷, ReadyGame 추가
 ******/

using ServerCore;
using System;
using System.Collections.Generic;

namespace Server
{
    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 클라이언트 최초 연결 시, playerId 설정
    /// </summary>
    public class STC_OnConnect : IPacket
    {
        public int playerId;
        public ushort Protocol => (ushort)PacketID.STC_OnConnect;

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


    #region Create/Enter Room
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

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
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

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
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

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

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
        public ushort Protocol => (ushort) PacketID.STC_PlayerEnter;

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
        public ushort Protocol => (ushort)PacketID.STC_ExistPlayers;
        public List<int> Players = new List<int>();

        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeof(ushort);
            //protocol
            count += sizeof(ushort);

            ushort listLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeof(ushort);

            for(ushort i = 0; i < listLen; i++)
            {
                Players.Add(BitConverter.ToInt32(segment.Array, segment.Offset + count));
                count += sizeof(int);
            }
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;

            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes((ushort)Players.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);
            for (int i = 0; i < Players.Count;i++)
            {
                Array.Copy(BitConverter.GetBytes(Players[i]), 0, segment.Array, segment.Offset + count, sizeof(int));
                count += sizeof(int);
            }

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }

    /// <summary>
    /// 작성자 : 박성택 <br/>
    /// 새로 입장한 클라이언트에게 기존 존재 방 정보 알림
    /// </summary>
    public class STC_ExistRooms : IPacket
    {
        public ushort Protocol => (ushort)PacketID.STC_ExistRooms;
        public List<string> Rooms = new List<string>();
        ushort StringLength = 0;
        ushort sizeofUshort = sizeof(ushort);
        public void Read(ArraySegment<byte> segment)
        {
            ushort count = 0;

            //packet size
            count += sizeofUshort;
            //protocol
            count += sizeofUshort;

            ushort listLength = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
            count += sizeofUshort;

            for (ushort i = 0; i < listLength; i++)
            {
                //string 의 Byte길이 우선 저장
                StringLength = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
                count += sizeofUshort;

                // Convert the bytes into a string and add it to the Rooms list.
                string roomName = System.Text.Encoding.UTF8.GetString(segment.Array, segment.Offset + count, StringLength);
                Rooms.Add(roomName);

                // Move past the string data.
                count += StringLength;

            }
        }

        public ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);
            ushort count = 0;
            //packet size

            count += sizeofUshort;
            //protocol

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeofUshort);
            count += sizeofUshort;

            Array.Copy(BitConverter.GetBytes((ushort)Rooms.Count), 0, segment.Array, segment.Offset + count, sizeofUshort);
            count += sizeofUshort;
            ushort StringLength;
            for (int i = 0; i < Rooms.Count; i++)
            {
                //문자열 변환 & Length저장
                byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(Rooms[i]);
                StringLength = (ushort)utf8Bytes.Length;

                //Length 우선 저장
                Array.Copy(BitConverter.GetBytes(StringLength), 0, segment.Array, segment.Offset + count, sizeofUshort);
                count += sizeofUshort;

                //string 저장
                Array.Copy(utf8Bytes, 0, segment.Array, segment.Offset + count, StringLength);
                count += StringLength;
            }

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }



    #endregion Create/Enter Room



    #region Loby
    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// Host 퇴장 시, Host 변경 패킷
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

            Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
            count += sizeof(ushort);

            Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

            return SendBufferHelper.Close(count);
        }
    }
    
    /// <summary>
    /// 작성자 : 이우열 <br/>
    /// 방장의 로비 -> 캐릭터 선택 씬 전환 요구 브로드캐스팅
    /// </summary>
    public class STC_ReadyGame : IPacket
    {
        public ushort Protocol => (ushort)PacketID.STC_ReadyGame;

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
    #endregion Loby
}