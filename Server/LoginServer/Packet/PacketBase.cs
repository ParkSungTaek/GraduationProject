/******
작성자 : 이우열
작성 일자 : 23.04.19

최근 수정 일자 : 23.10.02
최근 수정 내용 : Ingame ID 통합, PacketId 수정
 ******/

using ServerCore;
using System;

namespace LoginServer.Packet
{
    /// <summary> 패킷 종류 enum </summary>
    public enum PacketID
    {
        CTS_Auth,

        CTS_CreateRoom,
        CTS_SetPublicRoom,
        CTS_QuickEnter,
        CTS_GetPublicRoomList,
        CTS_EnterRoom,
        CTS_LeaveRoom,
        CTS_ReadyGame,

        STC_AuthAck,
        STC_DuplicatedLogin,
        STC_CheckAlive,

        STC_RejectRoom,
        STC_RejectEnter,
        STC_QuickEnterFail,

        STC_PlayerEnter,
        STC_ExistPlayers,
        STC_PublicRoomList,
        STC_PlayerLeave,

        STC_SetSuper,
        STC_ReadyGame,

        //---Ingame---//
        CTS_SelectClass,
        CTS_PlayerMove,
        CTS_PlayerAttack,
        CTS_PriestBuff,
        CTS_ItemUpdate,
        CTS_TowerDamage,
        CTS_MonsterHPUpdate,

        STC_SelectClass,
        STC_StartGame,
        STC_PlayerMove,
        STC_PlayerAttack,
        STC_PriestBuff,
        STC_ItemUpdate,
        STC_MosterCreate,
        STC_MonsterHPUpdate,
        STC_TowerUpdate,
        STC_GameOver,

        //--Login--//
        CTL_Regist,
        CTL_ForceRegist,
        CTL_RegistAuth,
        CTL_Login,

        LTC_RegistAck,
        LTC_RegistAuthAck,
        LTC_LoginAck,

        LTS_Auth,
        STL_AuthAck,

        MaxCount
    }

    public interface IPacket
    {
        /// <summary> 패킷 종류 </summary>
        ushort Protocol { get; }
        /// <summary> 받은 데이터 -> 패킷으로 전환 </summary>
        void Read(ArraySegment<byte> segment);
        /// <summary> 패킷 -> 보낼 데이터로 전환 </summary>
        ArraySegment<byte> Write();
    }

    public abstract class SimplePacket : IPacket
    {
        public abstract ushort Protocol { get; }

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
}