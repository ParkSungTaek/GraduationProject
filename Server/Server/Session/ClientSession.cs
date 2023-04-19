/******
작성자 : 이우열
작성 일자 : 23.04.19

최근 수정 일자 : 23.04.19
최근 수정 내용 : 클래스 생성
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
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {

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
        }
    }

}