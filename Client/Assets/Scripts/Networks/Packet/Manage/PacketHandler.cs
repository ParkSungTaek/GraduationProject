/******
작성자 : 공동 작성
작성 일자 : 23.05.03

최근 수정 일자 : 23.05.03
최근 수정 사항 : OnConnect Handler 생성
 ******/

using ServerCore;

namespace Client
{
	class PacketHandler
	{
		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="packet"></param>
		public static void STC_OnConnectHandler(PacketSession session, IPacket packet)
		{
			STC_OnConnect pkt = packet as STC_OnConnect;
			ServerSession serverSession = session as ServerSession;

			serverSession.SessionId = pkt.playerId;

			GameManager.Network.Push(() => UnityEngine.Debug.Log($"OnConnect : {pkt.playerId}"));
		}

		public static void STC_SetSuperHandler(PacketSession session, IPacket packet)
		{
			STC_SetSuper pkt = packet as STC_SetSuper;
			ServerSession serverSession = session as ServerSession;

			GameManager.Network.Push(() => UnityEngine.Debug.Log("Set Super"));
		}
	}
}