/******
작성자 : 이우열
작성 일자 : 23.10.01

최근 수정 일자 : 23.10.03
최근 수정 내용 : 메일 인증 패킷 추가
 ******/

using ServerCore;
using System;
using System.Collections.Generic;

namespace LoginServer.Packet.Manage
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
            _makeFunc.Add((ushort)PacketID.CTL_Regist, MakePacket<CTL_Regist>);
            _makeFunc.Add((ushort)PacketID.CTL_ForceRegist, MakePacket<CTL_ForceRegist>);
            _makeFunc.Add((ushort)PacketID.CTL_RegistAuth, MakePacket<CTL_RegistAuth>);
            _makeFunc.Add((ushort)PacketID.CTL_Login, MakePacket<CTL_Login>);

            _handler.Add((ushort)PacketID.CTL_Regist, PacketHandler.CTL_RegistHandler);
            _handler.Add((ushort)PacketID.CTL_ForceRegist, PacketHandler.CTL_ForceRegistHandler);
            _handler.Add((ushort)PacketID.CTL_RegistAuth, PacketHandler.CTL_RegistAuthHandler);
            _handler.Add((ushort)PacketID.CTL_Login, PacketHandler.CTL_LoginHandler);


            _makeFunc.Add((ushort)PacketID.STL_AuthAck, MakePacket<STL_AuthAck>);

            _handler.Add((ushort)PacketID.STL_AuthAck, PacketHandler.STL_AuthAckHandler);
        }

        /// <summary> 패킷 종류에 따라 handler 호출 </summary>
        public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
        {
            ushort count = 0;

            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            count += 2;
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;

            if (id != (ushort)PacketID.STC_CheckAlive)
                ServerCore.Logger.Log($"OnRecv : {(PacketID)id}");

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