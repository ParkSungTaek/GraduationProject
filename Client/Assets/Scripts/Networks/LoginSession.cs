/******
작성자 : 이우열
작성 일자 : 23.10.01

최근 수정 일자 : 23.10.01
최근 수정 사항 : 로그인 세션 클래스 생성
 ******/

using System;
using System.Net;
using ServerCore;

namespace Client
{
    class LoginSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint) { }

        public override void OnDisconnected(EndPoint endPoint) { }

        /// <summary> 받은 패킷 처리 </summary>
        public override void OnRecvPacket(ArraySegment<byte> buffer) 
        {
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numOfBytes) { }
    }
}
