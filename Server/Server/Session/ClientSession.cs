/******
작성자 : 이우열
작성 일자 : 23.04.19

최근 수정 일자 : 23.09.28
최근 수정 내용 : 연결 종료 시 세션 매니저에 알림
 ******/

using ServerCore;
using System;
using System.Net;

namespace Server
{
    public class ClientSession : PacketSession
    {
        public int SessionId { get; set; }
        public Room Room { get; set; }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");
            STC_OnConnect onConnectPacket = new STC_OnConnect();
            onConnectPacket.playerId = SessionId;
            Send(onConnectPacket.Write());

            STC_ExistRooms ExistRoomsPacket = new STC_ExistRooms();
            ExistRoomsPacket.Rooms = RoomManager.Instance.RoomNames();
            Send(ExistRoomsPacket.Write());
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int byteTransfered)
        {
            //Do Nothing
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            if(Room != null)
            {
                Room room = Room;
                room.Push(() => room.Leave(this));
                Room = null;
            }

            SessionManager.Instance.OnDisconnect(this);
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }
    }

}