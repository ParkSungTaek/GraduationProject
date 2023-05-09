/******
작성자 : 공동 작성
작성 일자 : 23.05.03

최근 수정 일자 : 23.05.09
최근 수정 내용 : 로비 관련 Handler 추가
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
			_makeFunc.Add((ushort)PacketID.STC_PlayerEnter,MakePacket<STC_PlayerEnter>);
			_makeFunc.Add((ushort)PacketID.STC_PlayerLeave, MakePacket<STC_PlayerLeave>);
			_makeFunc.Add((ushort)PacketID.STC_ExistPlayers, MakePacket<STC_ExistPlayers>);

            _handler.Add((ushort)PacketID.STC_OnConnect, PacketHandler.STC_OnConnectHandler);
            _handler.Add((ushort)PacketID.STC_RejectRoom, PacketHandler.STC_RejectRoomHandler);
            _handler.Add((ushort)PacketID.STC_RejectEnter_Full, PacketHandler.STC_RejectEnter_FullHandler);
            _handler.Add((ushort)PacketID.STC_RejectEnter_Exist, PacketHandler.STC_RejectEnter_ExistHandler);
            _handler.Add((ushort)PacketID.STC_PlayerEnter, PacketHandler.STC_PlayerEnterHandler);
            _handler.Add((ushort)PacketID.STC_PlayerLeave, PacketHandler.STC_PlayerLeaveHandler);
			_handler.Add((ushort)PacketID.STC_ExistPlayers, PacketHandler.STC_ExistPlayersHandler);
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

			_handler.Add((ushort)PacketID_Ingame.STC_SelectClass, PacketHandler.STC_SelectClassHandler);
			_handler.Add((ushort)PacketID_Ingame.STC_StartGame, PacketHandler.STC_StartGameHandler);
            _handler.Add((ushort)PacketID_Ingame.STC_PlayerMove, PacketHandler.STC_PlayerMoveHandler);
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
            return pkt;
        }
    }
}