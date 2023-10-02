/******
작성자 : 이우열
작성 일자 : 23.10.01

최근 수정 일자 : 23.10.01
최근 수정 내용 : 로그인 세션 클래스 생성
 ******/

using ServerCore;
using System;
using System.Net;

namespace Server
{
    public class LoginServerSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint) 
        {
            Console.WriteLine("Login Server Connected");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer) 
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int byteTransfered) { }

        public override void OnDisconnected(EndPoint endPoint) 
        {
            Console.WriteLine("Login Server Disconnected");
            LoginSessionManager.Instance.Disconnected();
        }
    }

}