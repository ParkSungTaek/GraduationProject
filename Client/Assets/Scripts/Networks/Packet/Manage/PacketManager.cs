/******
작성자 : 공동 작성
작성 일자 : 23.05.03

최근 수정 일자 : 23.05.03
최근 수정 내용 : OnConnectHandler 연결
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
			_makeFunc.Add((ushort)PacketID.STC_OnConnect, MakePacket<STC_OnConnect>);
			_makeFunc.Add((ushort)PacketID.STC_SetSuper, MakePacket<STC_SetSuper>);

			_handler.Add((ushort)PacketID.STC_OnConnect, PacketHandler.STC_OnConnectHandler);
			_handler.Add((ushort)PacketID.STC_SetSuper, PacketHandler.STC_SetSuperHandler);
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