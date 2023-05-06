/******
작성자 : 공동 작성
작성 일자 : 23.05.03

최근 수정 일자 : 23.05.06
최근 수정 사항 : PlayerMove Handler 작성
 ******/

using ServerCore;
using UnityEngine;

namespace Client
{
	class PacketHandler
	{
		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 서버에 연결 성공 시 서버에서 전송한 패킷, playerId 설정
		/// </summary>
		public static void STC_OnConnectHandler(PacketSession session, IPacket packet)
		{
			STC_OnConnect pkt = packet as STC_OnConnect;
			ServerSession serverSession = session as ServerSession;

			serverSession.SessionId = pkt.playerId;

			GameManager.Network.Push(() => Debug.Log($"OnConnect : {pkt.playerId}"));
		}

		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 통신 테스트를 위해
		/// </summary>
		public static void STC_SetSuperHandler(PacketSession session, IPacket packet)
		{
			STC_SetSuper pkt = packet as STC_SetSuper;
			ServerSession serverSession = session as ServerSession;

			GameManager.Network.Push(() => Debug.Log("Set Super"));
		}

		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 플레이어 이동 동기화
		/// </summary>
		public static void STC_PlayerMoveHandler(PacketSession session, IPacket packet)
		{
			STC_PlayerMove pkt = packet as STC_PlayerMove;
			ServerSession serverSession = session as ServerSession;

			//내가 보낸 패킷 -> 아무것도 안함
            if (serverSession.SessionId == GameManager.Network.PlayerId)
                return;

            GameManager.Network.Push(() =>
			{
				GameManager.InGameData.Move(serverSession.SessionId, new Vector2(pkt.posX, pkt.posY));
			});
		}
	}
}