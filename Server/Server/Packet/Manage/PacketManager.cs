/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.10.02
최근 수정 내용 : 로그인 서버, 인증 추가
 ******/

using ServerCore;
using System;
using System.Collections.Generic;

namespace Server
{
    public class PacketManager
    {
        public static PacketManager Instance { get; } = new PacketManager();

        PacketManager()
        {
            Register();
        }

        /// <summary> byte data -> 패킷 conversion 함수들 </summary>
        Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
        /// <summary> 패킷 핸들러 함수들 </summary>
        Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();
        
        public void Register()
        {
            _makeFunc.Add((ushort)PacketID.LTS_Auth, MakePacket<LTS_Auth>);

            _handler.Add((ushort)PacketID.LTS_Auth, PacketHandler.LTS_AuthHandler);

            _makeFunc.Add((ushort)PacketID.CTS_Auth, MakePacket<CTS_Auth>);

            _handler.Add((ushort)PacketID.CTS_Auth, PacketHandler.CTS_AuthHandler);

            _makeFunc.Add((ushort)PacketID.CTS_CreateRoom, MakePacket<CTS_CreateRoom>);
            _makeFunc.Add((ushort)PacketID.CTS_SetPublicRoom, MakePacket<CTS_SetPublicRoom>);
            _makeFunc.Add((ushort)PacketID.CTS_QuickEnter, MakePacket<CTS_QuickEnter>);
            _makeFunc.Add((ushort)PacketID.CTS_GetPublicRoomList, MakePacket<CTS_GetPublicRoomList>);

            _makeFunc.Add((ushort)PacketID.CTS_EnterRoom, MakePacket<CTS_EnterRoom>);
            _makeFunc.Add((ushort)PacketID.CTS_LeaveRoom, MakePacket<CTS_LeaveRoom>);
            _makeFunc.Add((ushort)PacketID.CTS_ReadyGame, MakePacket<CTS_ReadyGame>);
            
            _handler.Add((ushort)PacketID.CTS_CreateRoom, PacketHandler.CTS_CreateRoomHandler);
            _handler.Add((ushort)PacketID.CTS_SetPublicRoom, PacketHandler.CTS_SetPublicRoomHandler);
            _handler.Add((ushort)PacketID.CTS_QuickEnter, PacketHandler.CTS_QuickEnterRoomHandler);
            _handler.Add((ushort)PacketID.CTS_GetPublicRoomList, PacketHandler.CTS_GetExistRoomsHandler);

            _handler.Add((ushort)PacketID.CTS_EnterRoom, PacketHandler.CTS_EnterRoomHandler);
            _handler.Add((ushort)PacketID.CTS_LeaveRoom, PacketHandler.CTS_LeaveRoomHandler);
            _handler.Add((ushort)PacketID.CTS_ReadyGame, PacketHandler.CTS_ReadyGameHandler);


            #region Ingame
            _makeFunc.Add((ushort)PacketID.CTS_SelectClass, MakePacket<CTS_SelectClass>);
            _makeFunc.Add((ushort)PacketID.CTS_PlayerMove, MakePacket<CTS_PlayerMove>);
            _makeFunc.Add((ushort)PacketID.CTS_PlayerAttack, MakePacket<CTS_PlayerAttack>);
            _makeFunc.Add((ushort)PacketID.CTS_PriestBuff, MakePacket<CTS_PriestBuff>);
            _makeFunc.Add((ushort)PacketID.CTS_ItemUpdate, MakePacket<CTS_ItemUpdate>);
            _makeFunc.Add((ushort)PacketID.CTS_MonsterHPUpdate, MakePacket<CTS_MonsterHPUpdate>);
            _makeFunc.Add((ushort)PacketID.CTS_TowerDamage, MakePacket<CTS_TowerDamage>);


            _handler.Add((ushort)PacketID.CTS_SelectClass, PacketHandler.CTS_SelectClassHandler);
            _handler.Add((ushort)PacketID.CTS_PlayerMove, PacketHandler.CTS_PlayerMoveHandler);
            _handler.Add((ushort)PacketID.CTS_PlayerAttack, PacketHandler.CTS_PlayerAttackHandler);
            _handler.Add((ushort)PacketID.CTS_PriestBuff, PacketHandler.CTS_PriestBuffHandler);
            _handler.Add((ushort)PacketID.CTS_ItemUpdate, PacketHandler.CTS_ItemUpdateHandler);
            _handler.Add((ushort)PacketID.CTS_MonsterHPUpdate, PacketHandler.CTS_MonsterHPUpdateHandler);
            _handler.Add((ushort)PacketID.CTS_TowerDamage, PacketHandler.CTS_TowerDamageHandler);

            #endregion Ingame
        }

        /// <summary> 패킷 종류에 따라 handler 호출 </summary>
        public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
        {
            ushort count = 0;

            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            count += 2;
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;

            if(id != (ushort)PacketID.CTS_PlayerMove)
                ServerCore.Logger.Log($"Packet 수신 : {(PacketID)id}");

            Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
            if(_makeFunc.TryGetValue(id, out func))
            {
                IPacket packet = func.Invoke(session, buffer);

                if (onRecvCallback != null)
                    onRecvCallback.Invoke(session, packet);
                else
                    HandlePacket(session, packet);

            }
        }

        /// <summary> 패킷 종류에 따른 핸들러 호출 </summary>
        public void HandlePacket(PacketSession session, IPacket packet)
        {
            Action<PacketSession, IPacket> action = null;
            if(_handler.TryGetValue(packet.Protocol, out action))
                action.Invoke(session, packet);
        }

        /// <summary> byte data -> 패킷 conversion 함수 </summary>
        /// <typeparam name="T">packet type</typeparam>
        T MakePacket<T>(PacketSession session, ArraySegment<byte> segment) where T : IPacket, new()
        {
            T pkt = new T();
            pkt.Read(segment);
            return pkt;
        }
    }
}