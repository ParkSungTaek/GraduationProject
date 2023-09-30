/******
작성자 : 공동 작성
작성 일자 : 23.05.03

최근 수정 일자 : 23.09.29
최근 수정 내용 : 시작한 방 입장 실패 추가
 ******/

using ServerCore;
using System;
using System.Collections.Generic;

namespace Client
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
            #region Create/Enter Room
            _makeFunc.Add((ushort)PacketID.STC_OnConnect, MakePacket<STC_OnConnect>);
			_makeFunc.Add((ushort)PacketID.STC_RejectRoom, MakePacket<STC_RejectRoom>);
			_makeFunc.Add((ushort)PacketID.STC_RejectEnter_Full, MakePacket<STC_RejectEnter_Full>);
			_makeFunc.Add((ushort)PacketID.STC_RejectEnter_Exist, MakePacket<STC_RejectEnter_Exist>);
			_makeFunc.Add((ushort)PacketID.STC_RejectEnter_Start, MakePacket<STC_RejectEnter_Start>);
			_makeFunc.Add((ushort)PacketID.STC_PlayerEnter,MakePacket<STC_PlayerEnter>);
			_makeFunc.Add((ushort)PacketID.STC_PlayerLeave, MakePacket<STC_PlayerLeave>);
			_makeFunc.Add((ushort)PacketID.STC_ExistPlayers, MakePacket<STC_ExistPlayers>);
			_makeFunc.Add((ushort)PacketID.STC_ExistRooms, MakePacket<STC_ExistRooms>);

            _handler.Add((ushort)PacketID.STC_OnConnect, PacketHandler.STC_OnConnectHandler);
            _handler.Add((ushort)PacketID.STC_RejectRoom, PacketHandler.STC_RejectRoomHandler);
            _handler.Add((ushort)PacketID.STC_RejectEnter_Full, PacketHandler.STC_RejectEnter_FullHandler);
            _handler.Add((ushort)PacketID.STC_RejectEnter_Exist, PacketHandler.STC_RejectEnter_ExistHandler);
			_handler.Add((ushort)PacketID.STC_RejectEnter_Start, PacketHandler.STC_RejectEnter_StartHandler);
            _handler.Add((ushort)PacketID.STC_PlayerEnter, PacketHandler.STC_PlayerEnterHandler);
            _handler.Add((ushort)PacketID.STC_PlayerLeave, PacketHandler.STC_PlayerLeaveHandler);
			_handler.Add((ushort)PacketID.STC_ExistPlayers, PacketHandler.STC_ExistPlayersHandler);
			_handler.Add((ushort)PacketID.STC_ExistRooms, PacketHandler.STC_ExistRoomsHandler);

            #endregion Create/Enter Room

            #region Loby
            _makeFunc.Add((ushort)PacketID.STC_SetSuper, MakePacket<STC_SetSuper>);
			_makeFunc.Add((ushort)PacketID.STC_ReadyGame, MakePacket<STC_ReadyGame>);

			_handler.Add((ushort)PacketID.STC_SetSuper, PacketHandler.STC_SetSuperHandler);
			_handler.Add((ushort)PacketID.STC_ReadyGame, PacketHandler.STC_ReadyGameHandler);
            #endregion Loby

            #region Ingame
			_makeFunc.Add((ushort)PacketID_Ingame.STC_SelectClass, MakePacket<STC_SelectClass>);
            _makeFunc.Add((ushort)PacketID_Ingame.STC_StartGame, MakePacket<STC_StartGame>);
			_makeFunc.Add((ushort)PacketID_Ingame.STC_PlayerMove, MakePacket<STC_PlayerMove>);
            _makeFunc.Add((ushort)PacketID_Ingame.STC_PlayerAttack, MakePacket<STC_PlayerAttack>);
			_makeFunc.Add((ushort)PacketID_Ingame.STC_PriestBuff, MakePacket<STC_PriestBuff>);
			_makeFunc.Add((ushort)PacketID_Ingame.STC_ItemUpdate, MakePacket<STC_ItemUpdate>);
			_makeFunc.Add((ushort)PacketID_Ingame.STC_MosterCreate, MakePacket<STC_MosterCreate>);
            _makeFunc.Add((ushort)PacketID_Ingame.STC_MonsterHPUpdate, MakePacket<STC_MonsterHPUpdate>);
            _makeFunc.Add((ushort)PacketID_Ingame.STC_TowerUpdate, MakePacket<STC_TowerUpdate>);



            _handler.Add((ushort)PacketID_Ingame.STC_SelectClass, PacketHandler.STC_SelectClassHandler);
			_handler.Add((ushort)PacketID_Ingame.STC_StartGame, PacketHandler.STC_StartGameHandler);
            _handler.Add((ushort)PacketID_Ingame.STC_PlayerMove, PacketHandler.STC_PlayerMoveHandler);
			_handler.Add((ushort)PacketID_Ingame.STC_PlayerAttack, PacketHandler.STC_PlayerAttackHandler);
			_handler.Add((ushort)PacketID_Ingame.STC_PriestBuff, PacketHandler.STC_PriestBuffHandler);
			_handler.Add((ushort)PacketID_Ingame.STC_ItemUpdate, PacketHandler.STC_ItemUpdateHandler);
			_handler.Add((ushort)PacketID_Ingame.STC_MosterCreate, PacketHandler.STC_MosterCreateHandler);
            _handler.Add((ushort)PacketID_Ingame.STC_MonsterHPUpdate, PacketHandler.STC_MonsterHPUpdate);
            _handler.Add((ushort)PacketID_Ingame.STC_TowerUpdate, PacketHandler.STC_TowerUpdateHandler);

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

			Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
			if (_makeFunc.TryGetValue(id, out func))
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
			if (_handler.TryGetValue(packet.Protocol, out action))
				action.Invoke(session, packet);
        }

        /// <summary> byte data -> 패킷 conversion 함수 </summary>
        /// <typeparam name="T">packet type</typeparam>
        T MakePacket<T>(PacketSession session, ArraySegment<byte> segment) where T : IPacket, new()
        {
            T pkt = new T();
            pkt.Read(segment);

			if(typeof(T) == typeof(STC_ItemUpdate))
				Console.WriteLine(1);

            return pkt;
        }
    }
}