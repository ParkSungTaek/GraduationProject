/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.10.02
최근 수정 내용 : 로그인 서버 추가, 인증 추가
 ******/

using System;
using ServerCore;

namespace Server
{
    class PacketHandler
    {
        #region login
        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 인증 확인 패킷 처리
        /// </summary>
        public static void LTS_AuthHandler(PacketSession session, IPacket packet)
        {
            LoginServerSession loginServerSession = session as LoginServerSession;
            LTS_Auth authPacket = packet as LTS_Auth;

            AuthManager.Instance.OnAuth(authPacket.email);
            
            STL_AuthAck authAck = new STL_AuthAck();
            authAck.email = authPacket.email;
            loginServerSession.Send(authAck.Write());
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 로그인 패킷 처리
        /// </summary>
        public static void CTS_AuthHandler(PacketSession session, IPacket packet)
        {
            ClientSession clientSession = session as ClientSession;
            CTS_Auth authPacket = packet as CTS_Auth;

            bool isSuccess = AuthManager.Instance.TryAuth(authPacket.email);

            if (isSuccess)
            {
                clientSession.email = authPacket.email;
                ClientSessionManager.Instance.OnLogin(clientSession);
            }

            STC_AuthAck authAck = new STC_AuthAck();
            authAck.isSuccess = isSuccess;

            clientSession.Send(authAck.Write());

            if (isSuccess == false)
            {
                clientSession.Disconnect();
            }
        }
        #endregion

        #region Lobby
        /// <summary>
        /// 작성자 : 이우열<br/>
        /// 방 생성 패킷 처리
        /// </summary>
        public static void CTS_CreateRoomHandler(PacketSession session, IPacket packet)
        {
            ClientSession clientSession = session as ClientSession;
            CTS_CreateRoom createPacket = packet as CTS_CreateRoom;

            RoomManager.Instance.Push(() => RoomManager.Instance.Create(clientSession, createPacket.roomName));
        }

        /// <summary>
        /// 작성자 : 이우열<br/>
        /// 방 입장 패킷 처리
        /// </summary>
        public static void CTS_EnterRoomHandler(PacketSession session, IPacket packet)
        {
            ClientSession clientSession = session as ClientSession;
            CTS_EnterRoom enterPacket = packet as CTS_EnterRoom;

            RoomManager.Instance.Push(() => RoomManager.Instance.EnterRoom(clientSession, enterPacket.roomName));
        }


        /// <summary>
        /// 작성자 : 박성택<br/>
        /// 빠른입장 혀용 또는 불가
        /// </summary>
        public static void CTS_AllowQuickEntryRoomHandler(PacketSession session, IPacket packet)
        {
            ClientSession clientSession = session as ClientSession;
            CTS_AllowQuickEntryRoom allowPacket = packet as CTS_AllowQuickEntryRoom;

            Room room = clientSession.Room;

            if (room != null)
            {
                RoomManager.Instance.Push(() => RoomManager.Instance.AllowQuickEnter(room, allowPacket.AllowQuickEntry));
            }
            
            //RoomManager.Instance.Push(() => RoomManager.Instance.AllowQuickEnter(allowPacket.roomName, allowPacket.AllowQuickEntry));
        }
        /// <summary>
        /// 작성자 : 박성택<br/>
        /// 빠른입장
        /// </summary>
        public static void CTS_QuickEnterRoomHandler(PacketSession session, IPacket packet)
        {
            ClientSession clientSession = session as ClientSession;
            CTS_QuickEnterRoom enterPacket = packet as CTS_QuickEnterRoom;
            string roomName = "aaa";///RoomManager.Instance.RandomQuickEnterRoomName();
            if (roomName != null)
            {
                RoomManager.Instance.Push(() => RoomManager.Instance.EnterRoom(clientSession, roomName));
            }
            ///빠른참가 방이 없음
            else
            {
                // 추가적인 행동 필요?
            }

        }
        
        /// <summary>
        /// 작성자 : 박성택<br/>
        /// 빠른입장
        /// </summary>
        public static void CTS_GetExistRoomsHandler(PacketSession session, IPacket packet)
        {
            
            ClientSession clientSession = session as ClientSession;
            
            //CTS_GetExistRooms enterPacket = packet as CTS_GetExistRooms;
            STC_ExistRooms existRooms = new STC_ExistRooms();
            
            existRooms.Rooms = RoomManager.Instance.RoomNames();

            clientSession.Send(existRooms.Write());

        }


        /// <summary>
        /// 작성자 : 이우열<br/>
        /// 방 퇴장 패킷 처리
        /// </summary>
        public static void CTS_LeaveRoomHandler(PacketSession session, IPacket packet)
        {
            ClientSession clientSession = session as ClientSession;
            CTS_LeaveRoom leavePacket = packet as CTS_LeaveRoom;

            if(clientSession.Room != null)
            {
                Room room = clientSession.Room;

                room.Push(() => room.Leave(clientSession));
                clientSession.Room = null;
            }
        }
        
        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 로비 -> 클래스 선택 전환 패킷 처리
        /// </summary>
        public static void CTS_ReadyGameHandler(PacketSession session, IPacket packet)
        {
            ClientSession clientSession = session as ClientSession;

            Room room = clientSession.Room;

            room.Push(() => { room.Ready(clientSession); });
        }
        #endregion Lobby

        #region Playable
        /// <summary>
        /// 작성자 : 이우열<br/>
        /// 클래스 선택 패킷 처리
        /// </summary>
        public static void CTS_SelectClassHandler(PacketSession session, IPacket packet)
        {
            ClientSession clientSession = session as ClientSession;
            CTS_SelectClass pkt = packet as CTS_SelectClass;

            Room room = clientSession.Room;

            room.Push(() => room.SelectClass(clientSession, (Define.PlayerClass)pkt.PlayerClass));
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 클 -> 서 플레이어 이동 패킷 처리
        /// </summary>
        public static void CTS_PlayerMoveHandler(PacketSession session, IPacket packet)
        {
            ClientSession clientSession = session as ClientSession;
            CTS_PlayerMove movePacket = packet as CTS_PlayerMove;

            if (clientSession.Room == null)
                return;

            Room room = clientSession.Room;
            room.Push(() => room.Move(clientSession, movePacket));
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 클 -> 서 플레이어 공격 애니메이션 패킷 처리
        /// </summary>
        public static void CTS_PlayerAttackHandler(PacketSession session, IPacket packet)
        {
            ClientSession clientSession = session as ClientSession;
            CTS_PlayerAttack attackPacket = packet as CTS_PlayerAttack;

            if (clientSession.Room == null)
                return;

            Room room = clientSession.Room;
            room.Push(() => room.Attack(clientSession, attackPacket));
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 사제 버프 처리
        /// </summary>
        public static void CTS_PriestBuffHandler(PacketSession session, IPacket packet)
        {
            ClientSession clientSession = session as ClientSession;
            CTS_PriestBuff buffPacket = packet as CTS_PriestBuff;

            if (clientSession.Room == null)
                return;

            Room room = clientSession.Room;
            room.Push(() => room.Buff(buffPacket));
        }

        /// <summary>
        /// 작성자 : 이우열 <br/>
        /// 아이템 동기화 처리
        /// </summary>
        public static void CTS_ItemUpdateHandler(PacketSession session, IPacket packet)
        {
            ClientSession clientSession = session as ClientSession;
            CTS_ItemUpdate itemPacket = packet as CTS_ItemUpdate;

            if (clientSession.Room == null)
                return;

            Room room = clientSession.Room;
            room.Push(() => room.ItemUpdate(clientSession, itemPacket));
        }
        #endregion Playable

        #region Non-Playable
        /// <summary>
        /// 작성자 : 박성택 <br/>
        /// 몬스터 체력 동기화 처리
        /// </summary>
        public static void CTS_MonsterHPUpdateHandler(PacketSession session, IPacket packet)
        {
            ClientSession clientSession = session as ClientSession;
            CTS_MonsterHPUpdate monsterHPPacket = packet as CTS_MonsterHPUpdate;

            if (clientSession.Room == null)
                return;

            Room room = clientSession.Room;
            room.Push(() => room.MonsterHPUpdate(monsterHPPacket));
        }

       
        public static void CTS_TowerDamageHandler(PacketSession session, IPacket packet)
        {
            ClientSession clientSession = session as ClientSession;
            CTS_TowerDamage towerDamage = packet as CTS_TowerDamage;

            if (clientSession.Room == null)
                return;

            Room room = clientSession.Room;
            room.Push(() => room.TowerHPUpdate(towerDamage));
        }

        #endregion Non-Playable
    }
}