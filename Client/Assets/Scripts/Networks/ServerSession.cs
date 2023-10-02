/******
작성자 : 공동 작성
작성 일자 : 23.05.03

최근 수정 일자 : 23.10.02
최근 수정 사항 : 연결 끊김 시 로그인 화면 전환
 ******/

using System;
using System.Net;
using ServerCore;

namespace Client
{
	class ServerSession : PacketSession
	{
		public int SessionId { get; set; }

		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"OnConnected : {endPoint}");			
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			if(GameManager.Network.Email != string.Empty)
            {
                GameManager.Network.Email = string.Empty;
				GameManager.Network.GoBackToLogin();
			}
		}

		/// <summary> 받은 패킷 처리 </summary>
		public override void OnRecvPacket(ArraySegment<byte> buffer)
		{
			PacketManager.Instance.OnRecvPacket(this, buffer);
		}

		public override void OnSend(int numOfBytes)
		{
			//Console.WriteLine($"Transferred bytes: {numOfBytes}");
		}
	}
}
