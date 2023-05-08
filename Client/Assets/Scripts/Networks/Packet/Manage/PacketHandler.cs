/******
작성자 : 공동 작성
작성 일자 : 23.05.03

최근 수정 일자 : 23.05.08
최근 수정 사항 : OnConnectHandler에 PopUp 닫기 추가, 방 생성/입장 관련 handler 추가
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

			GameManager.Network.Push(() => { GameManager.UI.CloseAllPopUpUI(); });
		}

		/// <summary> 
		/// 작성자 : 이우열 <br/>
		/// 방 생성 실패 패킷 처리
		/// </summary>
		public static void STC_RejectRoomHandler(PacketSession session, IPacket packet)
		{
			GameManager.Network.Push(() => 
			{ 
				GameManager.UI.CloseAllPopUpUI(); 
				GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("중복된 방 이름입니다."); 
			});
		}

		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 풀방이라 입장 실패 패킷 처리
		/// </summary>
		public static void STC_RejectEnter_FullHandler(PacketSession session, IPacket packet)
		{
			GameManager.Network.Push(() =>
			{
				GameManager.UI.CloseAllPopUpUI();
				GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("가득찬 방입니다.");
			});
		}

		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 없는 방이라 입장 실패 패킷 처리
		/// </summary>
		public static void STC_RejectEnter_ExistHandler(PacketSession session, IPacket packet)
		{
			GameManager.Network.Push(() =>
			{
				GameManager.UI.CloseAllPopUpUI();
				GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("존재하지 않는 방입니다.");
			});
		}

		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 플레이어 입장 성공 패킷 처리
		/// </summary>
		public static void STC_PlayerEnterHandler(PacketSession session, IPacket packet)
		{
			STC_PlayerEnter pkt = packet as STC_PlayerEnter;
			ServerSession serverSession = session as ServerSession;

			//방 생성 성공
			if(pkt.playerId == GameManager.Network.PlayerId)
			{
				GameManager.Network.Push(() => 
				{
					SceneManager.LoadScene(Define.Scenes.Loby);
				});
			}
			//다른 플레이어 입장
			else
			{

			}
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