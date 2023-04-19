/******
작성자 : 이우열
작성 일자 : 23.04.19

최근 수정 일자 : 23.04.19
최근 수정 내용 : 패킷 기본 구조
 ******/

using ServerCore;
using System;

namespace Server
{
    /// <summary> 패킷 종류 enum </summary>
    public enum PacketID
    {
        CTS_CreateRoom,
        CTS_EnterRoom,
        
        STC_OnConnect,
        STC_RejectRoom,
        STC_RejectEnter_Exist,
        STC_RejectEnter_Full,
        STC_SuccessEnter,
        STC_SetSuper,
    }

    /// <summary> 인게임 상 패킷 종류 enum </summary>
    public enum PacketID_Ingame
    {
        CTS_PlayerMove,

        STC_PlayerMove,
    }

    public interface IPacket
    { 
        /// <summary> 패킷 종류 </summary>
        ushort Protocol { get;}
        /// <summary> 받은 데이터 -> 패킷으로 전환 </summary>
        void Read(ArraySegment<byte> segment);
        /// <summary> 패킷 -> 보낼 데이터로 전환 </summary>
        ArraySegment<byte> Write();
    }
}