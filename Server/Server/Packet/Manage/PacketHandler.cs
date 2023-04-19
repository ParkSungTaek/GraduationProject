/******
작성자 : 공동 작성
작성 일자 : 23.04.19

최근 수정 일자 : 23.04.19
최근 수정 내용 : PlayerMove 패킷 처리 추가
 ******/

using ServerCore;

namespace Server
{
    class PacketHandler
    {
        /// <summary>
        /// 작성자 : 이우열<br/>
        /// 방 입장 패킷 처리
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
            CTS_CreateRoom enterPacket = packet as CTS_CreateRoom;

            RoomManager.Instance.Push(() => RoomManager.Instance.EnterRoom(clientSession, enterPacket.roomName));
        }

        #region Ingame
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
        #endregion Ingame
    }
}