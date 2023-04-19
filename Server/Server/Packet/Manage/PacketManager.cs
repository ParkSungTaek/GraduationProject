/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.04.19
최근 수정 내용 : 클래스 추가
 ******/

using ServerCore;
using System;
using System.Collections.Generic;

namespace Server
{
    public class PacketManager
    {
        static PacketManager _instance = new PacketManager();
        public static PacketManager Instance { get => _instance; }

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
            _makeFunc.Add((ushort)PacketID.CTS_CreateRoom, MakePacket<CTS_CreateRoom>);
            _makeFunc.Add((ushort)PacketID.CTS_EnterRoom, MakePacket<CTS_EnterRoom>);

            _handler.Add((ushort)PacketID.CTS_CreateRoom, PacketHandler.CTS_CreateRoomHandler);
            _handler.Add((ushort)PacketID.CTS_EnterRoom, PacketHandler.CTS_EnterRoomHandler);

            #region Ingame
            _makeFunc.Add((ushort)PacketID_Ingame.CTS_PlayerMove, MakePacket<CTS_PlayerMove>);

            _handler.Add((ushort)PacketID_Ingame.CTS_PlayerMove, PacketHandler.CTS_PlayerMoveHandler);
            #endregion Ingame
        }

        /// <summary> 패킷 종류에 따라 handler 호출 </summary>
        public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action onRecvCallback = null)
        {
            ushort count = 0;

            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            count += 2;
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;

            Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
            if(_makeFunc.TryGetValue(id, out func))
            {
                IPacket packet = func.Invoke(session, buffer);

                if (onRecvCallback != null)
                    onRecvCallback.Invoke();
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