/******
작성자 : 공동 작성
작성 일자 : 23.05.03

최근 수정 일자 : 23.10.02
최근 수정 사항 : 로그인 서버, 인증
 ******/

using ServerCore;
using UnityEngine;

namespace Client
{
	class PacketHandler
	{

		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 회원가입 성공 여부 처리 패킷
		/// </summary>
		public static void LTC_RegistAckHandler(PacketSession session, IPacket packet)
		{
            LTC_RegistAck pkt = packet as LTC_RegistAck;
            LoginSession loginSession = session as LoginSession;

			switch (pkt.errorCode)
			{
				case (ushort)LTC_RegistAck.ErrorCode.Success:
                    GameManager.Network.Push(() => {
                        GameManager.UI.CloseAllPopUpUI();
                        GameManager.UI.ShowPopUpUI<UI_RegistAuth>();
                    });
                    break;
                case (ushort)LTC_RegistAck.ErrorCode.Duplicate:
                    GameManager.Network.Push(() => {
                        GameManager.UI.CloseAllPopUpUI();
                        GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("중복 이메일");
                    });
                    break;
                case (ushort)LTC_RegistAck.ErrorCode.MailError:
                    GameManager.Network.Push(() => {
                        GameManager.UI.CloseAllPopUpUI();
                        GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("잘못된 메일 형식");
                    });
                    break;
			}

			GameManager.Login.RemoveTimer();

			loginSession.Disconnect();
        }

		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 이메일 인증 처리 결과 패킷 처리
		/// </summary>
		public static void LTC_RegistAuthAckHandler(PacketSession session, IPacket packet)
		{
			LoginSession loginSession = session as LoginSession;
			LTC_RegistAuthAck authPacket = packet as LTC_RegistAuthAck;

			switch (authPacket.errorCode)
			{
				case (ushort)LTC_RegistAuthAck.ErrorCode.Success:
                    GameManager.Network.Push(() =>
                    {
                        GameManager.UI.CloseAllPopUpUI();
                        GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("회원가입 성공");
                    });
					break;
                case (ushort)LTC_RegistAuthAck.ErrorCode.DBError:
					GameManager.Network.Push(() =>
					{
						GameManager.UI.CloseAllPopUpUI();
						GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("중복 이메일");
					});
					break;
				case (ushort)LTC_RegistAuthAck.ErrorCode.WrongCode:
                    GameManager.Network.Push(() =>
                    {
						GameManager.UI.ClosePopUpUI();
                        GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("인증번호가 틀렸습니다.");
                    });
					break;
                case (ushort)LTC_RegistAuthAck.ErrorCode.Expired:
					GameManager.Network.Push(() =>
					{
						GameManager.UI.CloseAllPopUpUI();
						GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("시간이 만료되었습니다.");
					});
					break;
			}

			GameManager.Login.RemoveTimer();
			loginSession.Disconnect();
		}

		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 로그인 성공 여부 패킷 처리
		/// </summary>
		public static void LTC_LoginAckHandler(PacketSession session, IPacket packet)
        {
            LTC_LoginAck pkt = packet as LTC_LoginAck;
            LoginSession loginSession = session as LoginSession;

            if (pkt.isSuccess)
            {
				GameManager.Network.Push(() => 
				{ 
					GameManager.UI.CloseAllPopUpUI();
					CTS_Auth authPacket = new CTS_Auth { email = GameManager.Network.Email };
					GameManager.Network.Connect(authPacket);
				});
            }
            else
            {
                GameManager.Network.Push(() => {
					GameManager.UI.CloseAllPopUpUI();
					GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("로그인 실패"); 
				});
            }

			GameManager.Login.RemoveTimer();

			loginSession.Disconnect();
        }

		/// <summary>
		/// 게임 서버의 인증 반환 패킷 처리
		/// </summary>
		public static void STC_AuthAckHandler(PacketSession session, IPacket packet)
        {
            STC_AuthAck pkt = packet as STC_AuthAck;
            ServerSession serverSession = session as ServerSession;

            if (pkt.isSuccess)
            {
                GameManager.Network.Push(() =>
                {
					SceneManager.LoadScene(Define.Scenes.Title);
                });
            }
            else
            {
                GameManager.Network.Push(() => {
					GameManager.Network.Disconnect();
                    GameManager.UI.CloseAllPopUpUI();
                    GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("로그인 실패");
                });
            }
        }


		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 중복 로그인 시 로그아웃 처리
		/// </summary>
		public static void STC_DuplicatedLoginHandler(PacketSession session, IPacket packet)
		{
			GameManager.Network.Disconnect();
			GameManager.Network.GoBackToLogin();
        }

        #region Create/Enter Room
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
        /// 없는 방이라 입장 실패 패킷 처리
        /// </summary>
        public static void STC_RejectEnter_StartHandler(PacketSession session, IPacket packet)
        {
            GameManager.Network.Push(() =>
            {
                GameManager.UI.CloseAllPopUpUI();
                GameManager.UI.ShowPopUpUI<UI_ClosableLog>().SetLog("이미 시작한 방입니다.");
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

			//다른 플레이어 입장
			if (pkt.email != GameManager.Network.Email)
				GameManager.Network.Push(() => GameManager.Room.EnterPlayer(new STC_ExistPlayers.PlayerInfo { playerId = pkt.playerId, email = pkt.email }));

        }

		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 플레이어 퇴장 패킷 처리
		/// </summary>
		public static void STC_PlayerLeaveHandler(PacketSession session, IPacket packet)
		{
			STC_PlayerLeave pkt = packet as STC_PlayerLeave;

			if (pkt.playerId != GameManager.Room.MyId)
				GameManager.Network.Push(() =>
				{
					GameManager.Room.LeavePlayer(pkt.playerId);
					GameManager.InGameData.OnLeavePlayer(pkt.playerId);
				});
		}

		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 최초 입장 시, 이미 존재하는 플레이어 목록 받음(지금 입장한 나도 포함됨)
		/// </summary>
		public static void STC_ExistPlayersHandler(PacketSession session, IPacket packet)
		{
			STC_ExistPlayers pkt = packet as STC_ExistPlayers;
			var players = pkt.Players;

			GameManager.Network.Push(() =>
            {
				UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnLoadScene;
				UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnLoadScene;
                SceneManager.LoadScene(Define.Scenes.Lobby);
			});

			void OnLoadScene(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
			{
				GameManager.Room.SetExistPlayers(players);
				UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnLoadScene;
			}
		}

        /// <summary>
        /// 작성자 : 박성택 <br/>
        /// 최초 입장 시, 이미 존재하는 방 이름 목록 받음
        /// </summary>
        public static void STC_ExistRoomsHandler(PacketSession session, IPacket packet)
        {
            STC_ExistRooms pkt = packet as STC_ExistRooms;

            GameManager.Network.Push(() =>
            {
				//GameManager.UI.ShowSceneUI<UI_TitleScene>().SetExistRoomsName(pkt.Rooms);
            });
        }
        

        #endregion Create/Enter Room

        #region Lobby
        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 호스트 변경 패킷
        /// </summary>
        public static void STC_SetSuperHandler(PacketSession session, IPacket packet)
		{
			STC_SetSuper pkt = packet as STC_SetSuper;
			ServerSession serverSession = session as ServerSession;

			GameManager.Network.Push(GameManager.Room.SetHost);
		}

		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 로비 -> 캐릭터 선택 씬 전환
		/// </summary>
		public static void STC_ReadyGameHandler(PacketSession session, IPacket packet)
		{
			GameManager.Network.Push(() => SceneManager.LoadScene(Define.Scenes.Game));
		}
        #endregion Lobby

        #region Ingame
		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 다른 플레이어의 클래스 선택 정보 공유받음 처리
		/// </summary>
		public static void STC_SelectClassHandler(PacketSession session, IPacket packet)
		{
			STC_SelectClass pkt = packet as STC_SelectClass;

			GameManager.Network.Push(() => GameManager.Room.SelectClass(pkt.PlayerId, (Define.Charcter)pkt.PlayerClass));
		}
        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 모든 플레이어 클래스 선택 완료 -> 게임 시작
        /// </summary>
        public static void STC_StartGameHandler(PacketSession session, IPacket packet)
		{
			GameManager.Network.Push(GameManager.Room.GameStart);
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
            if (pkt.playerId == GameManager.Room.MyId)
                return;

            GameManager.Network.Push(() =>
			{
				GameManager.InGameData.Move(pkt.playerId, new Vector2(pkt.posX, pkt.posY));
			});
		}

		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 플레이어 애니메이션 동기화
		/// </summary>
		public static void STC_PlayerAttackHandler(PacketSession session, IPacket packet)
		{
			STC_PlayerAttack pkt = packet as STC_PlayerAttack;

			if (pkt.playerId == GameManager.Room.MyId)
				return;

			GameManager.Network.Push(() =>
			{
				GameManager.InGameData.Attack(pkt.playerId, pkt.direction, pkt.skillType == 1);
			});
		}

		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 사제 버프
		/// </summary>
		public static void STC_PriestBuffHandler(PacketSession session, IPacket packet)
		{
			STC_PriestBuff buffPacket = packet as STC_PriestBuff;

			GameManager.Network.Push(() => GameManager.InGameData.AddBuff(buffPacket.buffRate));
		}

		/// <summary>
		/// 작성자 : 이우열 <br/>
		/// 아이템 동기화
		/// </summary>
		public static void STC_ItemUpdateHandler(PacketSession session, IPacket packet)
		{
			STC_ItemUpdate itemPacket = packet as STC_ItemUpdate;

			if (itemPacket.playerId == GameManager.Room.MyId)
				return;

			GameManager.Network.Push(() => GameManager.InGameData.SyncItemInfo(itemPacket.playerId, itemPacket.position, itemPacket.itemIdx));
		}

        /// <summary>
        /// 작성자 : 박성택 <br/>
        /// 몬스터 생성 동기화
        /// </summary>
        public static void STC_MosterCreateHandler(PacketSession session, IPacket packet)
        {
            STC_MosterCreate pkt = packet as STC_MosterCreate;
            ServerSession serverSession = session as ServerSession;

            
            GameManager.Network.Push(() =>
            {
                GameManager.InGameData.MonsterSpawn.CreateMonster(pkt.createIDX, pkt.typeNum, pkt.ID);
				
            });
        }
        /// <summary>
        /// 작성자 : 박성택 <br/>
        /// 몬스터 HP 동기화
        /// </summary>
        public static void STC_MonsterHPUpdate(PacketSession session, IPacket packet)
        {
            STC_MonsterHPUpdate pkt = packet as STC_MonsterHPUpdate;
            ServerSession serverSession = session as ServerSession;


            GameManager.Network.Push(() =>
            {
				GameManager.InGameData.MonsterSpawn.Monsters[pkt.ID].HPUpdate((int)pkt.updateHP);
            });
        }
        public static void STC_TowerUpdateHandler(PacketSession session, IPacket packet)
        {
            STC_TowerUpdate pkt = packet as STC_TowerUpdate;
            ServerSession serverSession = session as ServerSession;


            GameManager.Network.Push(() =>
            {
                GameManager.InGameData.Tower.HPUpdate((int)pkt.updateHP);
            });
        }
        #endregion Ingame
    }
}