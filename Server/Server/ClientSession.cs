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
        Room _room;
        public Room Room { get => _room; set { _room = value; } }

        public override void OnConnected(EndPoint endPoint)
        {
            throw new NotImplementedException();
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            throw new NotImplementedException();
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            throw new NotImplementedException();
        }

        public override void OnSend(int byteTransfered)
        {
            throw new NotImplementedException();
        }
    }

}