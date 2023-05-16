/******
작성자 : 이우열
작성 일자 : 23.04.19

최근 수정 일자 : 23.05.09
최근 수정 내용 : PacketId 수정
 ******/

using System;

namespace Client
{
    /// <summary> 패킷 종류 enum </summary>
    public enum PacketID
    {
        CTS_CreateRoom,
        CTS_EnterRoom,
        CTS_LeaveRoom,
        CTS_ReadyGame,

        STC_OnConnect,

        STC_RejectRoom,
        STC_RejectEnter_Exist,
        STC_RejectEnter_Full,

        STC_PlayerEnter,
        STC_ExistPlayers,
        STC_PlayerLeave,

        STC_SetSuper,
        STC_ReadyGame,

        MaxCount
    }

    /// <summary> 인게임 상 패킷 종류 enum </summary>
    public enum PacketID_Ingame
    {
        CTS_SelectClass = PacketID.MaxCount,
        CTS_PlayerMove,
        CTS_PlayerAttack,
        CTS_PriestBuff,
        CTS_ItemUpdate,
        CTS_TowerDamage,

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
}